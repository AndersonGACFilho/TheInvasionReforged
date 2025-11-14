# Clean Architecture for Unreal Engine 5.7

**Project:** The Invasion Reforged  
**Engine:** Unreal Engine 5.7 + C++/Blueprints  
**Goal:** Clean, testable, multiplayer-ready code  
**Testing:** Unit tests for everything (TDD)

---

## Core Principles

### SOLID in Unreal Engine

**Single Responsibility**
One class, one job:

```cpp
// ❌ BAD: God class doing everything
UCLASS()
class APlayerShip : public APawn {
    GENERATED_BODY()
public:
    virtual void Tick(float DeltaTime) override {
        HandleInput();
        Move(DeltaTime);
        Shoot();
        TakeDamage();
        CollectScrap();
        LevelUp();
    }
};

// ✅ GOOD: Separated concerns using components
UCLASS()
class APlayerShip : public APawn {
    GENERATED_BODY()
    
    UPROPERTY(VisibleAnywhere)
    UMovementComponent* MovementComponent;
    
    UPROPERTY(VisibleAnywhere)
    UWeaponSystemComponent* WeaponSystem;
    
    UPROPERTY(VisibleAnywhere)
    UHealthComponent* HealthComponent;
    
    UPROPERTY(VisibleAnywhere)
    UScrapCollectorComponent* ScrapCollector;
};
```

#### Open/Closed
Extend, don't modify:

```cpp
// Weapon base class
UCLASS(Abstract, Blueprintable)
class UWeaponBase : public UObject {
    GENERATED_BODY()
public:
    UFUNCTION(BlueprintCallable)
    virtual void Fire(FVector Position, FVector Direction) PURE_VIRTUAL(UWeaponBase::Fire,);
    
    UFUNCTION(BlueprintCallable)
    virtual float GetCooldown() const PURE_VIRTUAL(UWeaponBase::GetCooldown, return 1.0f;);
};

// New weapons added without modifying existing code
UCLASS()
class UPlasmaBeamWeapon : public UWeaponBase {
    GENERATED_BODY()
public:
    virtual void Fire(FVector Position, FVector Direction) override {
        // Plasma beam implementation
    }
    
    virtual float GetCooldown() const override { return 0.5f; }
};

UCLASS()
class UIonCannonWeapon : public UWeaponBase {
    GENERATED_BODY()
public:
    virtual void Fire(FVector Position, FVector Direction) override {
        // Ion cannon implementation
    }
    
    virtual float GetCooldown() const override { return 2.0f; }
};
```

#### Liskov Substitution
Derived classes fully swappable:

```cpp
// Enemy interface
UINTERFACE(MinimalAPI, Blueprintable)
class UEnemy : public UInterface {
    GENERATED_BODY()
};

class IEnemy {
    GENERATED_BODY()
public:
    UFUNCTION(BlueprintNativeEvent, BlueprintCallable)
    void Initialize(FVector SpawnPosition);
    
    UFUNCTION(BlueprintNativeEvent, BlueprintCallable)
    void UpdateBehavior(float DeltaTime);
    
    UFUNCTION(BlueprintNativeEvent, BlueprintCallable)
    void TakeDamage(float Amount);
    
    UFUNCTION(BlueprintNativeEvent, BlueprintCallable)
    void Die();
};

// All enemies implement same interface
UCLASS()
class AFalconEnemy : public AActor, public IEnemy {
    GENERATED_BODY()
    // Implementation
};

UCLASS()
class ASentryEnemy : public AActor, public IEnemy {
    GENERATED_BODY()
    // Implementation
};

// Spawner doesn't care about concrete type
UCLASS()
class AEnemySpawner : public AActor {
    GENERATED_BODY()
public:
    UFUNCTION(BlueprintCallable)
    void SpawnEnemy(TSubclassOf<AActor> EnemyClass, FVector Position) {
        if (AActor* Enemy = GetWorld()->SpawnActor<AActor>(EnemyClass, Position, FRotator::ZeroRotator)) {
            if (IEnemy* EnemyInterface = Cast<IEnemy>(Enemy)) {
                EnemyInterface->Execute_Initialize(Enemy, Position);
            }
        }
    }
};
```

#### Interface Segregation
Small, focused interfaces:

```cpp
// ❌ BAD: Fat interface
UINTERFACE()
class UGameEntity : public UInterface {
    GENERATED_BODY()
};

class IGameEntity {
    GENERATED_BODY()
public:
    virtual void Move() = 0;
    virtual void Shoot() = 0;
    virtual void TakeDamage(float Amount) = 0;
    virtual void CollectScrap() = 0;
    virtual void LevelUp() = 0;
};

// ✅ GOOD: Segregated interfaces
UINTERFACE()
class IDamageable : public UInterface {
    GENERATED_BODY()
};

class IDamageable {
    GENERATED_BODY()
public:
    UFUNCTION(BlueprintNativeEvent)
    void TakeDamage(float Amount);
    
    UFUNCTION(BlueprintNativeEvent)
    float GetCurrentHealth() const;
};

UINTERFACE()
class ICollectable : public UInterface {
    GENERATED_BODY()
};

class ICollectable {
    GENERATED_BODY()
public:
    UFUNCTION(BlueprintNativeEvent)
    void OnCollected(AActor* Collector);
    
    UFUNCTION(BlueprintNativeEvent)
    int32 GetScrapValue() const;
};

UINTERFACE()
class ITargetable : public UInterface {
    GENERATED_BODY()
};

class ITargetable {
    GENERATED_BODY()
public:
    UFUNCTION(BlueprintNativeEvent)
    FVector GetTargetPosition() const;
    
    UFUNCTION(BlueprintNativeEvent)
    bool IsValidTarget() const;
};
```

#### Dependency Inversion
Depend on abstractions:

```cpp
// ❌ BAD: Concrete dependencies
UCLASS()
class APlayerShip : public APawn {
    GENERATED_BODY()
    
    UPROPERTY()
    UPlasmaBeamWeapon* CurrentWeapon; // Tightly coupled to specific weapon
};

// ✅ GOOD: Abstract dependencies
UCLASS()
class APlayerShip : public APawn {
    GENERATED_BODY()
    
    UPROPERTY(BlueprintReadWrite, EditAnywhere)
    UWeaponBase* CurrentWeapon; // Depends on abstraction
    
public:
    UFUNCTION(BlueprintCallable)
    void FireWeapon() {
        if (CurrentWeapon) {
            CurrentWeapon->Fire(GetActorLocation(), GetActorForwardVector());
        }
    }
};
```

---

## Project Architecture

### Layer Structure

```
Game/
├── Core/                       # Core systems (no dependencies)
│   ├── Interfaces/
│   │   ├── IDamageable.h
│   │   ├── ICollectable.h
│   │   ├── ITargetable.h
│   │   └── IPoolable.h
│   ├── Types/
│   │   ├── GameEnums.h
│   │   └── GameStructs.h
│   └── Constants/
│       └── GameConstants.h
│
├── Domain/                     # Game logic (depends on Core)
│   ├── Ship/
│   │   ├── PlayerShip.h/cpp
│   │   ├── ShipMovementComponent.h/cpp
│   │   └── ShipStatsData.h
│   ├── Weapons/
│   │   ├── WeaponBase.h/cpp
│   │   ├── PlasmaBeam.h/cpp
│   │   └── IonCannon.h/cpp
│   ├── Enemies/
│   │   ├── EnemyBase.h/cpp
│   │   ├── FalconEnemy.h/cpp
│   │   └── AtlasEnemy.h/cpp
│   └── Collectibles/
│       ├── ScrapPickup.h/cpp
│       └── UpgradePickup.h/cpp
│
├── Application/                # Game systems (orchestration)
│   ├── WaveSystem/
│   │   ├── WaveManager.h/cpp
│   │   └── WaveSpawner.h/cpp
│   ├── UpgradeSystem/
│   │   ├── UpgradeManager.h/cpp
│   │   └── UpgradeData.h
│   ├── ProgressionSystem/
│   │   ├── MetaProgressionManager.h/cpp
│   │   └── SaveGameData.h
│   └── PoolingSystem/
│       └── ObjectPool.h/cpp
│
├── Infrastructure/             # Unreal integration
│   ├── GameMode/
│   │   ├── TIRGameMode.h/cpp
│   │   └── TIRGameState.h/cpp
│   ├── PlayerController/
│   │   ├── TIRPlayerController.h/cpp
│   │   └── TIRPlayerState.h/cpp
│   ├── HUD/
│   │   └── TIRGameHUD.h/cpp
│   └── Subsystems/
│       ├── UpgradeSubsystem.h/cpp
│       └── SaveGameSubsystem.h/cpp
│
└── UI/                         # User interface
    ├── MainMenu/
    ├── HUD/
    ├── UpgradeSelection/
    └── PauseMenu/
```

---

## Component-Based Design

### Actor Components (Unreal's Composition System)

```cpp
// Ship with modular components
UCLASS()
class APlayerShip : public APawn {
    GENERATED_BODY()
    
protected:
    // Visual
    UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
    UStaticMeshComponent* ShipMesh;
    
    // Gameplay Components
    UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
    UShipMovementComponent* MovementComponent;
    
    UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
    UHealthComponent* HealthComponent;
    
    UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
    UWeaponSystemComponent* WeaponSystem;
    
    UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
    UScrapCollectorComponent* ScrapCollector;
    
    UPROPERTY(VisibleAnywhere, BlueprintReadOnly)
    UAbilitySystemComponent* AbilitySystem;
    
public:
    virtual void Tick(float DeltaTime) override {
        // Components handle their own update
    }
};
```

### Reusable Components

```cpp
// Health component - usable by any actor
UCLASS(ClassGroup=(Custom), meta=(BlueprintSpawnableComponent))
class UHealthComponent : public UActorComponent, public IDamageable {
    GENERATED_BODY()
    
protected:
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category="Health")
    float MaxHealth = 100.0f;
    
    UPROPERTY(BlueprintReadOnly, Category="Health")
    float CurrentHealth;
    
    UPROPERTY(BlueprintAssignable, Category="Events")
    FOnHealthChangedDelegate OnHealthChanged;
    
    UPROPERTY(BlueprintAssignable, Category="Events")
    FOnDeathDelegate OnDeath;
    
public:
    virtual void BeginPlay() override {
        CurrentHealth = MaxHealth;
    }
    
    UFUNCTION(BlueprintNativeEvent, BlueprintCallable)
    void TakeDamage(float Amount);
    virtual void TakeDamage_Implementation(float Amount) {
        CurrentHealth = FMath::Max(0.0f, CurrentHealth - Amount);
        OnHealthChanged.Broadcast(CurrentHealth, MaxHealth);
        
        if (CurrentHealth <= 0.0f) {
            OnDeath.Broadcast();
        }
    }
    
    UFUNCTION(BlueprintNativeEvent, BlueprintCallable)
    float GetCurrentHealth() const;
    virtual float GetCurrentHealth_Implementation() const {
        return CurrentHealth;
    }
};
```

---

## Data-Driven Design

### Data Assets for Configuration

```cpp
// Weapon configuration as data asset
UCLASS(BlueprintType)
class UWeaponData : public UDataAsset {
    GENERATED_BODY()
    
public:
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Weapon")
    FText WeaponName;
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Weapon")
    float BaseDamage = 10.0f;
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Weapon")
    float FireRate = 1.0f;
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Weapon")
    float Range = 1000.0f;
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="VFX")
    UParticleSystem* MuzzleFlash;
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="VFX")
    UParticleSystem* ImpactEffect;
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category="Audio")
    USoundBase* FireSound;
};

// Enemy configuration
UCLASS(BlueprintType)
class UEnemyData : public UDataAsset {
    GENERATED_BODY()
    
public:
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    FText EnemyName;
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    float MaxHealth = 50.0f;
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    float MoveSpeed = 300.0f;
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    int32 ScrapDropAmount = 5;
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    TSubclassOf<AActor> ActorClass;
};
```

### Struct Tables for Balancing

```cpp
// Upgrade progression table
USTRUCT(BlueprintType)
struct FUpgradeLevel : public FTableRowBase {
    GENERATED_BODY()
    
    UPROPERTY(EditAnywhere, BlueprintReadOnly)
    int32 Level;
    
    UPROPERTY(EditAnywhere, BlueprintReadOnly)
    int32 Cost;
    
    UPROPERTY(EditAnywhere, BlueprintReadOnly)
    float StatBonus;
    
    UPROPERTY(EditAnywhere, BlueprintReadOnly)
    FText Description;
};

// Usage in code
UCLASS()
class UUpgradeManager : public UGameInstanceSubsystem {
    GENERATED_BODY()
    
protected:
    UPROPERTY(EditDefaultsOnly)
    UDataTable* UpgradeTable; // References CSV or JSON
    
public:
    UFUNCTION(BlueprintCallable)
    bool CanAffordUpgrade(FName UpgradeName, int32 Level) {
        if (FUpgradeLevel* Row = UpgradeTable->FindRow<FUpgradeLevel>(UpgradeName, "")) {
            return PlayerScrap >= Row->Cost;
        }
        return false;
    }
};
```

---

## Event-Driven Architecture

### Delegates for Decoupling

```cpp
// Declare delegates
DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FOnScrapCollected, int32, Amount);
DECLARE_DYNAMIC_MULTICAST_DELEGATE_TwoParams(FOnHealthChanged, float, Current, float, Max);
DECLARE_DYNAMIC_MULTICAST_DELEGATE(FOnPlayerDied);

// Broadcast events
UCLASS()
class APlayerShip : public APawn {
    GENERATED_BODY()
    
public:
    UPROPERTY(BlueprintAssignable, Category="Events")
    FOnScrapCollected OnScrapCollected;
    
    UPROPERTY(BlueprintAssignable, Category="Events")
    FOnPlayerDied OnPlayerDied;
    
    void CollectScrap(int32 Amount) {
        TotalScrap += Amount;
        OnScrapCollected.Broadcast(Amount);
    }
};

// Listen to events
UCLASS()
class UGameHUD : public UUserWidget {
    GENERATED_BODY()
    
protected:
    UFUNCTION()
    void HandleScrapCollected(int32 Amount) {
        UpdateScrapCounter(Amount);
        PlayCollectionEffect();
    }
    
    virtual void NativeConstruct() override {
        Super::NativeConstruct();
        
        if (APlayerShip* Ship = Cast<APlayerShip>(GetOwningPlayerPawn())) {
            Ship->OnScrapCollected.AddDynamic(this, &UGameHUD::HandleScrapCollected);
        }
    }
};
```

### Gameplay Tags for State Management

```cpp
// Define tags in DefaultGameplayTags.ini
// GameplayTag.Player.State.Damaged
// GameplayTag.Player.State.Invincible
// GameplayTag.Weapon.Type.PlasmaBeam
// GameplayTag.Weapon.Type.IonCannon

#include "GameplayTagContainer.h"

UCLASS()
class APlayerShip : public APawn {
    GENERATED_BODY()
    
protected:
    UPROPERTY(BlueprintReadOnly)
    FGameplayTagContainer StateTags;
    
public:
    UFUNCTION(BlueprintCallable)
    void ApplyInvincibility(float Duration) {
        StateTags.AddTag(FGameplayTag::RequestGameplayTag("Player.State.Invincible"));
        
        FTimerHandle Timer;
        GetWorldTimerManager().SetTimer(Timer, [this]() {
            StateTags.RemoveTag(FGameplayTag::RequestGameplayTag("Player.State.Invincible"));
        }, Duration, false);
    }
    
    UFUNCTION(BlueprintCallable)
    bool CanTakeDamage() const {
        return !StateTags.HasTag(FGameplayTag::RequestGameplayTag("Player.State.Invincible"));
    }
};
```

---

## Testing Strategy

### Unit Tests with Automation Framework

```cpp
// ShipMovementTest.cpp
#include "Misc/AutomationTest.h"
#include "Ship/ShipMovementComponent.h"

IMPLEMENT_SIMPLE_AUTOMATION_TEST(FShipMovementTest, 
    "TheInvasionReforged.Ship.Movement", 
    EAutomationTestFlags::ApplicationContextMask | EAutomationTestFlags::ProductFilter)

bool FShipMovementTest::RunTest(const FString& Parameters) {
    // Arrange
    UShipMovementComponent* Movement = NewObject<UShipMovementComponent>();
    Movement->MaxSpeed = 500.0f;
    
    // Act
    Movement->AddInput(FVector(1, 0, 0), 1.0f);
    float ResultSpeed = Movement->GetVelocity().Size();
    
    // Assert
    TestTrue("Ship should move when input is applied", ResultSpeed > 0);
    TestTrue("Ship should not exceed max speed", ResultSpeed <= Movement->MaxSpeed);
    
    return true;
}

// Weapon cooldown test
IMPLEMENT_SIMPLE_AUTOMATION_TEST(FWeaponCooldownTest,
    "TheInvasionReforged.Weapon.Cooldown",
    EAutomationTestFlags::ApplicationContextMask | EAutomationTestFlags::ProductFilter)

bool FWeaponCooldownTest::RunTest(const FString& Parameters) {
    UPlasmaBeamWeapon* Weapon = NewObject<UPlasmaBeamWeapon>();
    
    // First shot should work
    TestTrue("First shot should fire", Weapon->TryFire());
    
    // Immediate second shot should fail
    TestFalse("Second shot should fail (on cooldown)", Weapon->TryFire());
    
    // Simulate cooldown passing
    Weapon->UpdateCooldown(Weapon->GetCooldown() + 0.1f);
    
    // Should be able to fire again
    TestTrue("Should fire after cooldown", Weapon->TryFire());
    
    return true;
}
```

### Blueprint Testing

Create test maps with automated scenarios:

```cpp
// AutomatedTestGameMode.h
UCLASS()
class AAutomatedTestGameMode : public AGameModeBase {
    GENERATED_BODY()
    
public:
    UFUNCTION(BlueprintImplementableEvent)
    void RunWaveSpawnTest();
    
    UFUNCTION(BlueprintImplementableEvent)
    void RunUpgradeSystemTest();
    
    UFUNCTION(BlueprintImplementableEvent)
    void RunBossPhaseTest();
};
```

---

## Performance Best Practices

### Object Pooling

```cpp
// Generic object pool
UCLASS()
class UObjectPool : public UObject {
    GENERATED_BODY()
    
protected:
    UPROPERTY()
    TArray<AActor*> AvailableObjects;
    
    UPROPERTY()
    TArray<AActor*> ActiveObjects;
    
    UPROPERTY()
    TSubclassOf<AActor> PooledClass;
    
public:
    UFUNCTION(BlueprintCallable)
    AActor* GetPooledObject() {
        if (AvailableObjects.Num() > 0) {
            AActor* Object = AvailableObjects.Pop();
            Object->SetActorHiddenInGame(false);
            Object->SetActorEnableCollision(true);
            ActiveObjects.Add(Object);
            return Object;
        }
        
        // Create new if pool empty
        AActor* NewObject = GetWorld()->SpawnActor<AActor>(PooledClass);
        ActiveObjects.Add(NewObject);
        return NewObject;
    }
    
    UFUNCTION(BlueprintCallable)
    void ReturnToPool(AActor* Object) {
        if (ActiveObjects.Remove(Object) > 0) {
            Object->SetActorHiddenInGame(true);
            Object->SetActorEnableCollision(false);
            AvailableObjects.Add(Object);
        }
    }
    
    void PrewarmPool(int32 Count) {
        for (int32 i = 0; i < Count; ++i) {
            AActor* Object = GetWorld()->SpawnActor<AActor>(PooledClass);
            ReturnToPool(Object);
        }
    }
};
```

### Instanced Static Meshes for VFX

```cpp
// Particle system using ISM for better performance
UCLASS()
class AOptimizedParticleSystem : public AActor {
    GENERATED_BODY()
    
protected:
    UPROPERTY(VisibleAnywhere)
    UInstancedStaticMeshComponent* ParticleInstances;
    
    TArray<FParticleData> Particles;
    
public:
    void SpawnParticle(FVector Location, FVector Velocity) {
        FTransform Transform;
        Transform.SetLocation(Location);
        int32 InstanceIndex = ParticleInstances->AddInstance(Transform);
        
        Particles.Add({InstanceIndex, Location, Velocity, 0.0f});
    }
    
    virtual void Tick(float DeltaTime) override {
        for (int32 i = Particles.Num() - 1; i >= 0; --i) {
            Particles[i].Lifetime += DeltaTime;
            Particles[i].Location += Particles[i].Velocity * DeltaTime;
            
            if (Particles[i].Lifetime > MaxLifetime) {
                ParticleInstances->RemoveInstance(Particles[i].InstanceIndex);
                Particles.RemoveAt(i);
            } else {
                ParticleInstances->UpdateInstanceTransform(
                    Particles[i].InstanceIndex,
                    FTransform(Particles[i].Location),
                    true, true, false
                );
            }
        }
    }
};
```

---

## Multiplayer Readiness

### Replication Setup

```cpp
// Replicated player ship
UCLASS()
class APlayerShip : public APawn {
    GENERATED_BODY()
    
protected:
    UPROPERTY(ReplicatedUsing=OnRep_Health)
    float Health;
    
    UPROPERTY(Replicated)
    int32 TotalScrap;
    
public:
    APlayerShip() {
        bReplicates = true;
        SetReplicateMovement(true);
    }
    
    virtual void GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const override {
        Super::GetLifetimeReplicatedProps(OutLifetimeProps);
        
        DOREPLIFETIME(APlayerShip, Health);
        DOREPLIFETIME(APlayerShip, TotalScrap);
    }
    
    UFUNCTION()
    void OnRep_Health() {
        // Update UI when health changes
        OnHealthChanged.Broadcast(Health, MaxHealth);
    }
    
    // Server-authoritative actions
    UFUNCTION(Server, Reliable, WithValidation)
    void ServerFireWeapon(FVector Direction);
    void ServerFireWeapon_Implementation(FVector Direction) {
        // Server handles actual firing
        FireWeapon(Direction);
    }
    bool ServerFireWeapon_Validate(FVector Direction) {
        return Direction.IsNormalized();
    }
};
```

---

## Summary Checklist

### For Each New Feature

- [ ] **Interface first** - Define contracts before implementation
- [ ] **Component-based** - Use actor components for modularity
- [ ] **Data-driven** - Configuration in Data Assets, not hard-coded
- [ ] **Event-driven** - Use delegates to decouple systems
- [ ] **Test-driven** - Write automation tests for core logic
- [ ] **Pool objects** - Use object pooling for frequently spawned actors
- [ ] **Replication-ready** - Consider multiplayer even if shipping single-player
- [ ] **Blueprint-friendly** - Expose key functions to Blueprints
- [ ] **Performance-conscious** - Profile and optimize hot paths

---

**This architecture ensures The Invasion Reforged is:**
- ✅ Maintainable (6 months from now, you'll understand it)
- ✅ Testable (automated tests catch bugs early)
- ✅ Scalable (easy to add content updates)
- ✅ Multiplayer-ready (even if shipping single-player first)
- ✅ Blueprint-friendly (artists/designers can iterate)

---

**Document Status:** Complete  
**Last Updated:** Nov 13, 2025  
**Owner:** Anderson Gonçalves

