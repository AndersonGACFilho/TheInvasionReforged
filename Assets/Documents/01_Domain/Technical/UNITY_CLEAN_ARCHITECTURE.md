# Clean Architecture for Unity

**Project:** The Invasion Reforged  
**Engine:** Unity + C#  
**Goal:** Clean, testable, multiplayer-ready code  
**Testing:** Unit tests for everything (TDD)

---

## Core Principles

### SOLID in Unity

**Single Responsibility**
One class, one job:

```csharp
// ❌ BAD: God class doing everything
public class Player : MonoBehaviour {
    void Update() {
        HandleInput();
        Move();
        Shoot();
        TakeDamage();
        CollectScrap();
        LevelUp();
    }
}

// ✅ GOOD: Separated concerns
public class PlayerShipController : MonoBehaviour {
    private IPlayerInput _input;
    private IMovementSystem _movement;
    private IWeaponSystem _weapons;
    private IHealthSystem _health;
}
```

#### Open/Closed
Extend, don't modify:

```csharp
// Weapon base class
public abstract class WeaponBase : ScriptableObject {
    public abstract void Fire(Vector3 position, Vector3 direction);
    public abstract float GetCooldown();
}

// New weapons added without modifying existing code
public class PlasmaBeamWeapon : WeaponBase {
    public override void Fire(Vector3 position, Vector3 direction) {
        // Plasma beam implementation
    }
}

public class IonCannonWeapon : WeaponBase {
    public override void Fire(Vector3 position, Vector3 direction) {
        // Ion cannon implementation
    }
}
```

#### Liskov Substitution
Derived classes fully swappable:

```csharp
public interface IEnemy {
    void Initialize(Vector3 spawnPosition);
    void UpdateBehavior(float deltaTime);
    void TakeDamage(float amount);
    void Die();
}

// All enemies implement same interface
public class FalconEnemy : MonoBehaviour, IEnemy { ... }
public class SentryEnemy : MonoBehaviour, IEnemy { ... }
public class AtlasEnemy : MonoBehaviour, IEnemy { ... }

// Spawner doesn't care about concrete type
public class EnemySpawner {
    public void SpawnEnemy(IEnemy enemyPrefab, Vector3 position) {
        IEnemy enemy = Instantiate(enemyPrefab);
        enemy.Initialize(position);
    }
}
```

#### Interface Segregation
Small, focused interfaces:

```csharp
// ❌ BAD: Fat interface
public interface IGameEntity {
    void Move();
    void Shoot();
    void TakeDamage(float amount);
    void CollectScrap();
    void LevelUp();
}

// ✅ GOOD: Segregated interfaces
public interface IMovable {
    void Move(Vector3 direction, float speed);
}

public interface IDamageable {
    float CurrentHealth { get; }
    float MaxHealth { get; }
    void TakeDamage(float amount);
    void Die();
}

public interface ICollector {
    void CollectScrap(int amount);
}

public interface ILevelable {
    int CurrentLevel { get; }
    int CurrentXP { get; }
    void AddExperience(int amount);
}
```

#### Dependency Inversion Principle
Depend on abstractions, not concretions:

```csharp
// ❌ BAD: Tight coupling
public class PlayerShipController : MonoBehaviour {
    private KeyboardInput _input; // Concrete class
    
    void Update() {
        Vector2 move = _input.GetMovementInput();
    }
}

// ✅ GOOD: Dependency injection
public class PlayerShipController : MonoBehaviour {
    private IPlayerInput _input; // Abstract interface
    
    public void Initialize(IPlayerInput input) {
        _input = input; // Can be local, networked, AI, replay...
    }
    
    void Update() {
        Vector2 move = _input.GetMovementInput();
    }
}
```

---

## Multiplayer-Ready Architecture

### Network Abstraction Layer

```csharp
// Game state that will need network sync in future
[System.Serializable]
public struct GameState {
    public int currentWave;
    public float timeElapsed;
    public int totalEnemiesKilled;
    public int totalScrapCollected;
    
    // Serializable for network transmission
    public byte[] Serialize() { ... }
    public static GameState Deserialize(byte[] data) { ... }
}

[System.Serializable]
public struct PlayerState {
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 velocity;
    public float health;
    public float shield;
    public int scrapCount;
    public int level;
    
    // Network sync ready
    public byte[] Serialize() { ... }
    public static PlayerState Deserialize(byte[] data) { ... }
}
```

### Input Abstraction (Ready for Co-op)

```csharp
// Input interface (works for local, network, AI, replay)
public interface IPlayerInput {
    Vector2 GetMovementInput();
    bool GetDashPressed();
    bool GetAbility1Pressed();
    bool GetAbility2Pressed();
    bool GetAbility3Pressed();
}

// Local player implementation
public class LocalPlayerInput : IPlayerInput {
    public Vector2 GetMovementInput() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        return new Vector2(x, y);
    }
    
    public bool GetDashPressed() => Input.GetKeyDown(KeyCode.Space);
    public bool GetAbility1Pressed() => Input.GetKeyDown(KeyCode.Q);
    public bool GetAbility2Pressed() => Input.GetKeyDown(KeyCode.E);
    public bool GetAbility3Pressed() => Input.GetKeyDown(KeyCode.R);
}

// Future networked player implementation
public class NetworkedPlayerInput : IPlayerInput {
    private PlayerInputPacket _latestInput;
    
    public Vector2 GetMovementInput() => _latestInput.movement;
    public bool GetDashPressed() => _latestInput.dashPressed;
    // ... receives input from network
}

// AI implementation (for testing or single-player companions)
public class AIPlayerInput : IPlayerInput {
    private Vector2 _targetDirection;
    
    public Vector2 GetMovementInput() => _targetDirection;
    // ... AI logic
}
```

### Command Pattern (Network-Replayable)

```csharp
// All player actions as commands
public interface IPlayerCommand {
    void Execute(PlayerShipController player);
    uint GetTimestamp(); // For network sync
}

public class MoveCommand : IPlayerCommand {
    public Vector2 direction;
    public float deltaTime;
    public uint timestamp;
    
    public void Execute(PlayerShipController player) {
        player.Move(direction, deltaTime);
    }
    
    public uint GetTimestamp() => timestamp;
}

public class FireWeaponCommand : IPlayerCommand {
    public Vector3 position;
    public Vector3 direction;
    public uint timestamp;
    
    public void Execute(PlayerShipController player) {
        player.FireWeapon(position, direction);
    }
    
    public uint GetTimestamp() => timestamp;
}

// Command queue (can be replayed for client prediction/rollback)
public class CommandQueue {
    private Queue<IPlayerCommand> _commands = new Queue<IPlayerCommand>();
    
    public void Enqueue(IPlayerCommand command) {
        _commands.Enqueue(command);
    }
    
    public void ExecuteAll(PlayerShipController player) {
        while (_commands.Count > 0) {
            IPlayerCommand cmd = _commands.Dequeue();
            cmd.Execute(player);
        }
    }
}
```

### Event-Driven Decoupling (Network-Friendly)

```csharp
// Centralized event system (easy to replicate over network)
public static class GameEvents {
    // Player events
    public static event Action<int> OnPlayerLevelUp;
    public static event Action<int> OnScrapCollected;
    public static event Action<float> OnPlayerDamaged;
    public static event Action OnPlayerDied;
    
    // Enemy events
    public static event Action<EnemyType, Vector3> OnEnemyKilled;
    public static event Action<int> OnWaveCompleted;
    
    // Upgrade events
    public static event Action<UpgradeData> OnUpgradeApplied;
    public static event Action<UpgradeData> OnPermanentUpgradePurchased;
    
    // Boss events
    public static event Action<int> OnBossPhaseChange;
    public static event Action OnBossDefeated;
    
    // Invoke methods (can be gated for network authority)
    public static void PlayerLevelUp(int newLevel) {
        OnPlayerLevelUp?.Invoke(newLevel);
    }
    
    public static void ScrapCollected(int amount) {
        OnScrapCollected?.Invoke(amount);
    }
    
    // ... etc
}

// Systems listen to events
public class UIManager : MonoBehaviour {
    void OnEnable() {
        GameEvents.OnPlayerLevelUp += HandleLevelUp;
        GameEvents.OnScrapCollected += HandleScrapCollected;
    }
    
    void OnDisable() {
        GameEvents.OnPlayerLevelUp -= HandleLevelUp;
        GameEvents.OnScrapCollected -= HandleScrapCollected;
    }
    
    void HandleLevelUp(int newLevel) {
        // Update UI
    }
    
    void HandleScrapCollected(int amount) {
        // Update scrap counter
    }
}
```

---

## System Architecture

### Core Systems (All Unit Testable)

```csharp
// 1. Movement System (physics-based, network-ready)
public class MovementSystem : IMovementSystem {
    private float _speed;
    private float _inertia;
    private Vector3 _velocity;
    
    public void Move(Vector3 direction, float deltaTime) {
        // Apply inertia
        _velocity = Vector3.Lerp(_velocity, direction * _speed, _inertia * deltaTime);
    }
    
    public Vector3 GetVelocity() => _velocity;
    
    // Unit testable (no MonoBehaviour dependencies)
    [Test]
    public void Move_WithRightInput_IncreasesVelocityRight() {
        var movement = new MovementSystem(speed: 10f, inertia: 5f);
        movement.Move(Vector3.right, 0.1f);
        Assert.Greater(movement.GetVelocity().x, 0);
    }
}

// 2. Weapon System (command-based, replayable)
public class WeaponSystem : IWeaponSystem {
    private WeaponBase _currentWeapon;
    private float _cooldownTimer;
    private IProjectilePool _projectilePool;
    
    public bool CanFire() => _cooldownTimer <= 0;
    
    public void Fire(Vector3 position, Vector3 direction) {
        if (!CanFire()) return;
        
        _currentWeapon.Fire(position, direction);
        _cooldownTimer = _currentWeapon.GetCooldown();
    }
    
    public void UpdateCooldown(float deltaTime) {
        if (_cooldownTimer > 0) {
            _cooldownTimer -= deltaTime;
        }
    }
    
    // Unit testable
    [Test]
    public void Fire_WhenOnCooldown_DoesNotFire() {
        var weapon = new MockWeapon(cooldown: 1f);
        var system = new WeaponSystem(weapon);
        
        system.Fire(Vector3.zero, Vector3.forward);
        bool canFireAgain = system.CanFire();
        
        Assert.IsFalse(canFireAgain);
    }
}

// 3. Health System (network-syncable)
public class HealthSystem : IHealthSystem, IDamageable {
    private float _currentHealth;
    private float _maxHealth;
    
    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;
    public bool IsAlive => _currentHealth > 0;
    
    public void TakeDamage(float amount) {
        _currentHealth = Mathf.Max(0, _currentHealth - amount);
        
        if (_currentHealth <= 0) {
            Die();
        }
    }
    
    public void Heal(float amount) {
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth + amount);
    }
    
    public void Die() {
        GameEvents.PlayerDied();
    }
    
    // Unit testable
    [Test]
    public void TakeDamage_ReducesHealth() {
        var health = new HealthSystem(maxHealth: 100);
        health.TakeDamage(30);
        Assert.AreEqual(70, health.CurrentHealth);
    }
    
    [Test]
    public void TakeDamage_WhenHealthReachesZero_TriggersDeathEvent() {
        bool deathTriggered = false;
        GameEvents.OnPlayerDied += () => deathTriggered = true;
        
        var health = new HealthSystem(maxHealth: 100);
        health.TakeDamage(100);
        
        Assert.IsTrue(deathTriggered);
        Assert.IsFalse(health.IsAlive);
    }
}

// 4. Experience System (deterministic, testable)
public class ExperienceSystem : IExperienceSystem {
    private int _currentLevel;
    private int _currentXP;
    private int[] _xpThresholds; // Level-up thresholds
    
    public int CurrentLevel => _currentLevel;
    public int CurrentXP => _currentXP;
    public int XPToNextLevel => _xpThresholds[_currentLevel] - _currentXP;
    
    public void AddExperience(int amount) {
        _currentXP += amount;
        
        while (_currentXP >= _xpThresholds[_currentLevel]) {
            LevelUp();
        }
    }
    
    private void LevelUp() {
        _currentLevel++;
        GameEvents.PlayerLevelUp(_currentLevel);
    }
    
    // Unit testable
    [Test]
    public void AddExperience_WhenReachingThreshold_LevelsUp() {
        var xp = new ExperienceSystem(thresholds: new int[] { 0, 100, 250, 500 });
        xp.AddExperience(100);
        Assert.AreEqual(1, xp.CurrentLevel);
    }
    
    [Test]
    public void AddExperience_CanLevelUpMultipleTimes() {
        var xp = new ExperienceSystem(thresholds: new int[] { 0, 100, 250, 500 });
        xp.AddExperience(300);
        Assert.AreEqual(2, xp.CurrentLevel);
    }
}

// 5. Scrap Collection System (event-driven)
public class ScrapCollectionSystem : ICollector {
    private int _scrapCollected;
    private float _collectionRadius;
    
    public int CurrentScrap => _scrapCollected;
    
    public void CollectScrap(int amount) {
        _scrapCollected += amount;
        GameEvents.ScrapCollected(amount);
    }
    
    public int GetScrapAfterDeath() {
        // 70% retention on death
        return Mathf.FloorToInt(_scrapCollected * 0.7f);
    }
    
    public void Reset() {
        _scrapCollected = 0;
    }
    
    // Unit testable
    [Test]
    public void GetScrapAfterDeath_Returns70Percent() {
        var collector = new ScrapCollectionSystem();
        collector.CollectScrap(100);
        int retained = collector.GetScrapAfterDeath();
        Assert.AreEqual(70, retained);
    }
}
```

---

## Object Pooling (Performance-Critical for Multiplayer)

```csharp
// Generic object pool (testable, reusable)
public class ObjectPool<T> where T : Component, IPoolable {
    private Queue<T> _pool = new Queue<T>();
    private T _prefab;
    private Transform _parent;
    private int _initialSize;
    
    public ObjectPool(T prefab, int initialSize, Transform parent = null) {
        _prefab = prefab;
        _initialSize = initialSize;
        _parent = parent;
        
        // Pre-warm pool
        for (int i = 0; i < initialSize; i++) {
            T obj = Object.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
    
    public T Get() {
        T obj;
        
        if (_pool.Count > 0) {
            obj = _pool.Dequeue();
        } else {
            obj = Object.Instantiate(_prefab, _parent);
        }
        
        obj.gameObject.SetActive(true);
        obj.OnSpawn();
        return obj;
    }
    
    public void Return(T obj) {
        obj.OnDespawn();
        obj.gameObject.SetActive(false);
        _pool.Enqueue(obj);
    }
    
    public int ActiveCount => _initialSize - _pool.Count;
    public int PooledCount => _pool.Count;
}

// Poolable interface
public interface IPoolable {
    void OnSpawn();
    void OnDespawn();
}

// Example: Pooled projectile
public class Projectile : MonoBehaviour, IPoolable {
    private float _speed;
    private float _lifetime;
    private float _lifetimeTimer;
    
    public void OnSpawn() {
        _lifetimeTimer = _lifetime;
    }
    
    public void OnDespawn() {
        // Reset state
    }
    
    void Update() {
        transform.position += transform.forward * _speed * Time.deltaTime;
        
        _lifetimeTimer -= Time.deltaTime;
        if (_lifetimeTimer <= 0) {
            ProjectilePool.Instance.Return(this);
        }
    }
}

// Usage in weapon system
public class WeaponSystem {
    private ObjectPool<Projectile> _projectilePool;
    
    public void Initialize() {
        _projectilePool = new ObjectPool<Projectile>(
            projectilePrefab, 
            initialSize: 100
        );
    }
    
    public void Fire(Vector3 position, Vector3 direction) {
        Projectile proj = _projectilePool.Get();
        proj.transform.position = position;
        proj.transform.forward = direction;
    }
}
```

---

## Save/Load System (Versioned, Testable)

```csharp
// Save data structure (versioned for backward compatibility)
[System.Serializable]
public class SaveData {
    public int version = 1; // Increment when structure changes
    public PlayerSaveData playerData;
    public ProgressionSaveData progressionData;
    public SettingsSaveData settingsData;
    
    public SaveData() {
        playerData = new PlayerSaveData();
        progressionData = new ProgressionSaveData();
        settingsData = new SettingsSaveData();
    }
}

[System.Serializable]
public class PlayerSaveData {
    public int totalScrapCollected;
    public int totalEnemiesKilled;
    public float totalPlayTime;
}

[System.Serializable]
public class ProgressionSaveData {
    public Dictionary<string, int> permanentUpgradeLevels;
    public int highestWaveReached;
    public bool bossDefeated;
}

[System.Serializable]
public class SettingsSaveData {
    public float masterVolume;
    public float musicVolume;
    public float sfxVolume;
    public bool vhsFilterEnabled;
}

// Save/Load manager (testable with mock file system)
public class SaveLoadManager {
    private IFileSystem _fileSystem;
    private string _saveFilePath;
    
    public SaveLoadManager(IFileSystem fileSystem, string saveFilePath) {
        _fileSystem = fileSystem;
        _saveFilePath = saveFilePath;
    }
    
    public void Save(SaveData data) {
        try {
            string json = JsonUtility.ToJson(data, prettyPrint: true);
            _fileSystem.WriteAllText(_saveFilePath, json);
            Debug.Log("Game saved successfully");
        } catch (System.Exception e) {
            Debug.LogError($"Save failed: {e.Message}");
        }
    }
    
    public SaveData Load() {
        try {
            if (!_fileSystem.FileExists(_saveFilePath)) {
                return new SaveData(); // New game
            }
            
            string json = _fileSystem.ReadAllText(_saveFilePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            // Version migration if needed
            if (data.version < 1) {
                MigrateToVersion1(data);
            }
            
            Debug.Log("Game loaded successfully");
            return data;
        } catch (System.Exception e) {
            Debug.LogError($"Load failed: {e.Message}");
            return new SaveData(); // Fallback to new game
        }
    }
    
    private void MigrateToVersion1(SaveData data) {
        // Handle data structure changes
        data.version = 1;
    }
    
    // Unit testable with mock file system
    [Test]
    public void Save_CreatesFile() {
        var mockFS = new MockFileSystem();
        var saveManager = new SaveLoadManager(mockFS, "test.json");
        var data = new SaveData();
        
        saveManager.Save(data);
        
        Assert.IsTrue(mockFS.FileExists("test.json"));
    }
    
    [Test]
    public void Load_WhenFileDoesNotExist_ReturnsNewSaveData() {
        var mockFS = new MockFileSystem();
        var saveManager = new SaveLoadManager(mockFS, "test.json");
        
        SaveData loaded = saveManager.Load();
        
        Assert.AreEqual(1, loaded.version);
        Assert.IsNotNull(loaded.playerData);
    }
}

// File system abstraction (for testing)
public interface IFileSystem {
    bool FileExists(string path);
    void WriteAllText(string path, string content);
    string ReadAllText(string path);
}

// Real implementation
public class UnityFileSystem : IFileSystem {
    public bool FileExists(string path) => System.IO.File.Exists(path);
    public void WriteAllText(string path, string content) => System.IO.File.WriteAllText(path, content);
    public string ReadAllText(string path) => System.IO.File.ReadAllText(path);
}

// Mock for testing
public class MockFileSystem : IFileSystem {
    private Dictionary<string, string> _files = new Dictionary<string, string>();
    
    public bool FileExists(string path) => _files.ContainsKey(path);
    public void WriteAllText(string path, string content) => _files[path] = content;
    public string ReadAllText(string path) => _files[path];
}
```

---

## Demo Version Implementation

```csharp
// Demo configuration (ScriptableObject)
[CreateAssetMenu(fileName = "DemoConfig", menuName = "Game/Demo Configuration")]
public class DemoConfiguration : ScriptableObject {
    [Header("Demo Restrictions")]
    public bool isDemoVersion = true;
    
    [Header("Content Limits")]
    public int maxWave = 10;
    public int maxArenas = 3;
    public string[] allowedArenas = { "Aureon Prime", "Cryovex", "Voltra-9" };
    public int maxPermanentUpgradeTier = 3;
    
    [Header("UI Messages")]
    public string unlockFullGameMessage = "Unlock the full game to continue!";
    public string fullGameFeatures = "Full game includes:\n- 5 arenas\n- Boss fights\n- Unlimited progression\n- Future co-op multiplayer";
}

// Demo manager
public class DemoManager : MonoBehaviour {
    [SerializeField] private DemoConfiguration demoConfig;
    
    public bool IsDemoVersion => demoConfig.isDemoVersion;
    
    public bool IsWaveAllowed(int wave) {
        if (!IsDemoVersion) return true;
        return wave <= demoConfig.maxWave;
    }
    
    public bool IsArenaUnlocked(string arenaName) {
        if (!IsDemoVersion) return true;
        return System.Array.IndexOf(demoConfig.allowedArenas, arenaName) >= 0;
    }
    
    public bool CanUpgradeFurther(string upgradeName, int currentTier) {
        if (!IsDemoVersion) return true;
        return currentTier < demoConfig.maxPermanentUpgradeTier;
    }
    
    public void ShowUnlockPrompt() {
        // Show UI with Steam/itch.io links
        UIManager.Instance.ShowModal(
            title: "Demo Limit Reached",
            message: demoConfig.unlockFullGameMessage + "\n\n" + demoConfig.fullGameFeatures,
            buttons: new string[] { "Buy Full Game", "Continue Playing" }
        );
    }
}

// Usage in game systems
public class WaveManager : MonoBehaviour {
    [SerializeField] private DemoManager demoManager;
    private int _currentWave;
    
    void AdvanceWave() {
        _currentWave++;
        
        if (!demoManager.IsWaveAllowed(_currentWave)) {
            demoManager.ShowUnlockPrompt();
            // Return to main menu or reset
            return;
        }
        
        // Continue with wave
    }
}
```

---

## Unit Testing Examples

```csharp
// Test fixture for movement system
[TestFixture]
public class MovementSystemTests {
    [Test]
    public void Move_WithZeroInput_DoesNotChangeVelocity() {
        var movement = new MovementSystem(speed: 10f, inertia: 1f);
        movement.Move(Vector3.zero, 0.1f);
        Assert.AreEqual(Vector3.zero, movement.GetVelocity());
    }
    
    [Test]
    public void Move_WithForwardInput_IncreasesForwardVelocity() {
        var movement = new MovementSystem(speed: 10f, inertia: 5f);
        movement.Move(Vector3.forward, 0.1f);
        Assert.Greater(movement.GetVelocity().z, 0);
    }
    
    [Test]
    public void Move_AppliesInertia_GradualAcceleration() {
        var movement = new MovementSystem(speed: 10f, inertia: 2f);
        
        float vel1 = movement.GetVelocity().magnitude;
        movement.Move(Vector3.forward, 0.1f);
        float vel2 = movement.GetVelocity().magnitude;
        movement.Move(Vector3.forward, 0.1f);
        float vel3 = movement.GetVelocity().magnitude;
        
        Assert.Less(vel1, vel2);
        Assert.Less(vel2, vel3);
    }
}

// Test fixture for experience system
[TestFixture]
public class ExperienceSystemTests {
    [Test]
    public void AddExperience_IncreasesCurrentXP() {
        var xp = new ExperienceSystem(new int[] { 0, 100, 250 });
        xp.AddExperience(50);
        Assert.AreEqual(50, xp.CurrentXP);
    }
    
    [Test]
    public void AddExperience_WhenReachingThreshold_TriggersLevelUpEvent() {
        bool levelUpFired = false;
        GameEvents.OnPlayerLevelUp += (level) => levelUpFired = true;
        
        var xp = new ExperienceSystem(new int[] { 0, 100, 250 });
        xp.AddExperience(100);
        
        Assert.IsTrue(levelUpFired);
        Assert.AreEqual(1, xp.CurrentLevel);
    }
    
    [Test]
    public void XPToNextLevel_CalculatesCorrectly() {
        var xp = new ExperienceSystem(new int[] { 0, 100, 250 });
        xp.AddExperience(70);
        Assert.AreEqual(30, xp.XPToNextLevel);
    }
}

// Test fixture for save/load
[TestFixture]
public class SaveLoadManagerTests {
    [Test]
    public void Save_WritesDataToFile() {
        var mockFS = new MockFileSystem();
        var manager = new SaveLoadManager(mockFS, "test.json");
        var data = new SaveData();
        data.playerData.totalScrapCollected = 1000;
        
        manager.Save(data);
        
        Assert.IsTrue(mockFS.FileExists("test.json"));
    }
    
    [Test]
    public void Load_RestoresData() {
        var mockFS = new MockFileSystem();
        var manager = new SaveLoadManager(mockFS, "test.json");
        var originalData = new SaveData();
        originalData.playerData.totalScrapCollected = 1000;
        
        manager.Save(originalData);
        SaveData loadedData = manager.Load();
        
        Assert.AreEqual(1000, loadedData.playerData.totalScrapCollected);
    }
    
    [Test]
    public void Load_WhenFileDoesNotExist_ReturnsDefaultData() {
        var mockFS = new MockFileSystem();
        var manager = new SaveLoadManager(mockFS, "nonexistent.json");
        
        SaveData data = manager.Load();
        
        Assert.IsNotNull(data);
        Assert.AreEqual(1, data.version);
    }
}
```

---

## Folder Structure

```
Assets/
├── _Project/
│   ├── Scripts/
│   │   ├── Core/
│   │   │   ├── Systems/
│   │   │   │   ├── MovementSystem.cs
│   │   │   │   ├── WeaponSystem.cs
│   │   │   │   ├── HealthSystem.cs
│   │   │   │   ├── ExperienceSystem.cs
│   │   │   │   └── ScrapCollectionSystem.cs
│   │   │   ├── Interfaces/
│   │   │   │   ├── IMovementSystem.cs
│   │   │   │   ├── IWeaponSystem.cs
│   │   │   │   ├── IDamageable.cs
│   │   │   │   ├── IPlayerInput.cs
│   │   │   │   └── IPoolable.cs
│   │   │   └── Events/
│   │   │       └── GameEvents.cs
│   │   ├── Player/
│   │   │   ├── PlayerShipController.cs
│   │   │   └── LocalPlayerInput.cs
│   │   ├── Enemies/
│   │   │   ├── Base/
│   │   │   │   ├── IEnemy.cs
│   │   │   │   └── EnemyBase.cs
│   │   │   ├── FalconEnemy.cs
│   │   │   ├── SentryEnemy.cs
│   │   │   ├── AtlasEnemy.cs
│   │   │   ├── AegisNodeEnemy.cs
│   │   │   └── RavenIXEnemy.cs
│   │   ├── Weapons/
│   │   │   ├── Base/
│   │   │   │   └── WeaponBase.cs
│   │   │   ├── PlasmaBeamWeapon.cs
│   │   │   ├── IonCannonWeapon.cs
│   │   │   └── OrbitalSentinelWeapon.cs
│   │   ├── Managers/
│   │   │   ├── GameManager.cs
│   │   │   ├── WaveManager.cs
│   │   │   ├── SaveLoadManager.cs
│   │   │   └── DemoManager.cs
│   │   ├── Pooling/
│   │   │   ├── ObjectPool.cs
│   │   │   └── PoolManager.cs
│   │   └── Utilities/
│   │       ├── IFileSystem.cs
│   │       └── UnityFileSystem.cs
│   ├── Tests/
│   │   ├── PlayMode/
│   │   │   └── IntegrationTests.cs
│   │   └── EditMode/
│   │       ├── MovementSystemTests.cs
│   │       ├── WeaponSystemTests.cs
│   │       ├── HealthSystemTests.cs
│   │       ├── ExperienceSystemTests.cs
│   │       └── SaveLoadManagerTests.cs
│   ├── ScriptableObjects/
│   │   ├── Weapons/
│   │   ├── Upgrades/
│   │   └── Configuration/
│   │       └── DemoConfiguration.asset
│   ├── Prefabs/
│   │   ├── Player/
│   │   ├── Enemies/
│   │   ├── Projectiles/
│   │   └── VFX/
│   ├── Models/
│   │   ├── Ships/
│   │   ├── Enemies/
│   │   └── Environments/
│   ├── Materials/
│   │   ├── Ships/
│   │   ├── Enemies/
│   │   └── VFX/
│   ├── Shaders/
│   │   └── VHSFilter.shader
│   └── Scenes/
│       ├── MainMenu.unity
│       ├── Hangar.unity
│       ├── Arena_AureonPrime.unity
│       ├── Arena_Cryovex.unity
│       └── Arena_Voltra9.unity
└── Packages/
    └── manifest.json
```

---

## Next Steps

1. **Tomorrow (Nov 4):** Set up Unity project, install Test Framework
2. **Week 1:** Model low-poly ships, implement VHS filter
3. **Week 7:** Start coding with TDD (write tests first!)
4. **Throughout:** Maintain clean architecture for future co-op

**Key Principle:** Every system you build today should work seamlessly when you add multiplayer tomorrow.

