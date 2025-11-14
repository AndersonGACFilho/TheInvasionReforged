# Clean Architecture: Unreal Engine 5.7

**Project:** The Invasion Reforged  
**Approach:** Component-based, data-driven, testable  
**Testing:** Automation Framework (TDD)

---

## Core Principles

### Component-Based Design

Unreal's composition system over inheritance. Each actor composed of focused, reusable components.

```cpp
// Actor with modular components
UCLASS()
class APlayerShip : public APawn {
    GENERATED_BODY()
    
    UPROPERTY(VisibleAnywhere)
    UStaticMeshComponent* ShipMesh;
    
    UPROPERTY(VisibleAnywhere)
    UShipMovementComponent* MovementComponent;
    
    UPROPERTY(VisibleAnywhere)
    UHealthComponent* HealthComponent;
    
    UPROPERTY(VisibleAnywhere)
    UWeaponSystemComponent* WeaponSystem;
};
```

### Interface-Driven

Small, focused interfaces enable testability and flexibility.

```cpp
// Segregated interfaces
UINTERFACE(MinimalAPI, Blueprintable)
class UDamageable : public UInterface {
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

// Enemies implement interface
UCLASS()
class AFalconEnemy : public AActor, public IDamageable {
    GENERATED_BODY()
    // Implementation
};
```

### Data-Driven Configuration

Data Assets replace hardcoded values. Add content without code changes.

```cpp
// Enemy stats as Data Asset
UCLASS(BlueprintType)
class UEnemyStatsData : public UDataAsset {
    GENERATED_BODY()
    
public:
    UPROPERTY(EditDefaultsOnly, Category="Stats")
    float MaxHealth = 100.0f;
    
    UPROPERTY(EditDefaultsOnly, Category="Stats")
    float MovementSpeed = 300.0f;
    
    UPROPERTY(EditDefaultsOnly, Category="Stats")
    float Damage = 10.0f;
    
    UPROPERTY(EditDefaultsOnly, Category="Behavior")
    TSubclassOf<UBehaviorTree> AIBehavior;
};

// Enemy uses data asset
UCLASS()
class AEnemyBase : public AActor {
    GENERATED_BODY()
    
    UPROPERTY(EditDefaultsOnly, Category="Configuration")
    UEnemyStatsData* StatsData;
    
    virtual void BeginPlay() override {
        HealthComponent->SetMaxHealth(StatsData->MaxHealth);
        MovementComponent->SetSpeed(StatsData->MovementSpeed);
    }
};
```

### Event-Driven Communication

Components communicate via delegates, not direct references.

```cpp
// Health component broadcasts events
DECLARE_DYNAMIC_MULTICAST_DELEGATE_TwoParams(FOnHealthChanged, float, CurrentHealth, float, MaxHealth);
DECLARE_DYNAMIC_MULTICAST_DELEGATE(FOnDeath);

UCLASS()
class UHealthComponent : public UActorComponent {
    GENERATED_BODY()
    
public:
    UPROPERTY(BlueprintAssignable)
    FOnHealthChanged OnHealthChanged;
    
    UPROPERTY(BlueprintAssignable)
    FOnDeath OnDeath;
    
    void TakeDamage(float Amount) {
        CurrentHealth = FMath::Max(0.0f, CurrentHealth - Amount);
        OnHealthChanged.Broadcast(CurrentHealth, MaxHealth);
        
        if (CurrentHealth <= 0.0f) {
            OnDeath.Broadcast();
        }
    }
};

// Other systems subscribe
void AEnemyBase::BeginPlay() {
    HealthComponent->OnDeath.AddDynamic(this, &AEnemyBase::HandleDeath);
}
```

---

## Project Structure

```
Source/TheInvasionReforged/
├── Public/
│   ├── Core/                  # Interfaces, enums, structs
│   │   ├── Interfaces/
│   │   │   ├── IDamageable.h
│   │   │   ├── ICollectable.h
│   │   │   └── ITargetable.h
│   │   └── Types/
│   │       ├── TIREnums.h
│   │       └── TIRStructs.h
│   │
│   ├── Characters/            # Player and enemies
│   │   ├── PlayerShip.h
│   │   ├── EnemyBase.h
│   │   └── Components/
│   │       ├── ShipMovementComponent.h
│   │       ├── HealthComponent.h
│   │       └── WeaponSystemComponent.h
│   │
│   ├── Weapons/               # Weapon system
│   │   ├── WeaponBase.h
│   │   ├── PlasmaBeamWeapon.h
│   │   └── WeaponData.h
│   │
│   ├── AI/                    # Enemy AI
│   │   ├── BTTask_*.h
│   │   └── BTService_*.h
│   │
│   ├── Progression/           # XP and upgrades
│   │   ├── UpgradeComponent.h
│   │   ├── MetaProgressionManager.h
│   │   └── UpgradeData.h
│   │
│   ├── Game/                  # Game framework
│   │   ├── TIRGameMode.h
│   │   ├── TIRGameState.h
│   │   ├── TIRPlayerController.h
│   │   └── Subsystems/
│   │       └── SaveGameSubsystem.h
│   │
│   └── UI/                    # HUD and menus
│       └── TIRGameHUD.h
│
└── Private/                   # Implementation files
    └── Tests/                 # Automation Framework tests
```

---

## Key Patterns

### Reusable Components

```cpp
// Health component - attach to any actor
UCLASS(ClassGroup=(Custom), meta=(BlueprintSpawnableComponent))
class UHealthComponent : public UActorComponent, public IDamageable {
    GENERATED_BODY()
    
protected:
    UPROPERTY(EditAnywhere, Category="Health")
    float MaxHealth = 100.0f;
    
    float CurrentHealth;
    
    UPROPERTY(BlueprintAssignable)
    FOnHealthChanged OnHealthChanged;
    
    UPROPERTY(BlueprintAssignable)
    FOnDeath OnDeath;
    
public:
    virtual void BeginPlay() override {
        CurrentHealth = MaxHealth;
    }
    
    virtual void TakeDamage_Implementation(float Amount) override {
        CurrentHealth = FMath::Max(0.0f, CurrentHealth - Amount);
        OnHealthChanged.Broadcast(CurrentHealth, MaxHealth);
        
        if (CurrentHealth <= 0.0f) {
            OnDeath.Broadcast();
        }
    }
};
```

### Data Asset Configuration

```cpp
// Weapon configuration as Data Asset
UCLASS(BlueprintType)
class UWeaponData : public UDataAsset {
    GENERATED_BODY()
    
public:
    UPROPERTY(EditDefaultsOnly)
    FName WeaponName;
    
    UPROPERTY(EditDefaultsOnly)
    float Damage = 10.0f;
    
    UPROPERTY(EditDefaultsOnly)
    float FireRate = 1.0f;
    
    UPROPERTY(EditDefaultsOnly)
    float Range = 1000.0f;
    
    UPROPERTY(EditDefaultsOnly)
    TSubclassOf<AActor> ProjectileClass;
};

// Weapon uses data asset - no hardcoded values
UCLASS()
class UWeaponBase : public UObject {
    GENERATED_BODY()
    
protected:
    UPROPERTY(EditDefaultsOnly)
    UWeaponData* WeaponData;
    
public:
    virtual void Fire(FVector Position, FVector Direction) {
        // Use WeaponData->Damage, WeaponData->FireRate, etc.
    }
};
```

### Gameplay Tags for State

```cpp
// Use gameplay tags instead of enums for extensibility
UCLASS()
class APlayerShip : public APawn {
    GENERATED_BODY()
    
    UPROPERTY(EditDefaultsOnly)
    FGameplayTagContainer CurrentUpgrades;
    
public:
    void AddUpgrade(FGameplayTag UpgradeTag) {
        CurrentUpgrades.AddTag(UpgradeTag);
        ApplyUpgradeEffects(UpgradeTag);
    }
    
    bool HasUpgrade(FGameplayTag UpgradeTag) const {
        return CurrentUpgrades.HasTag(UpgradeTag);
    }
};

// Define tags in DefaultGameplayTags.ini:
// +GameplayTagList=(Tag="Upgrade.Weapon.PlasmaBeam",DevComment="")
// +GameplayTagList=(Tag="Upgrade.Hull.Reinforced",DevComment="")
```

---

## Testing Strategy

### Unit Tests (Automation Framework)

```cpp
// Test health component in isolation
IMPLEMENT_SIMPLE_AUTOMATION_TEST(
    FHealthComponentTest, 
    "TheInvasionReforged.Components.HealthComponent", 
    EAutomationTestFlags::ApplicationContextMask | EAutomationTestFlags::ProductFilter
)

bool FHealthComponentTest::RunTest(const FString& Parameters) {
    // Arrange
    UHealthComponent* HealthComp = NewObject<UHealthComponent>();
    HealthComp->SetMaxHealth(100.0f);
    HealthComp->BeginPlay();
    
    // Act
    HealthComp->TakeDamage(30.0f);
    
    // Assert
    TestEqual("Health should be 70", HealthComp->GetCurrentHealth(), 70.0f);
    
    return true;
}
```

### Component Testing

```cpp
// Test weapon component
IMPLEMENT_SIMPLE_AUTOMATION_TEST(
    FWeaponSystemTest,
    "TheInvasionReforged.Weapons.WeaponSystem",
    EAutomationTestFlags::ApplicationContextMask | EAutomationTestFlags::ProductFilter
)

bool FWeaponSystemTest::RunTest(const FString& Parameters) {
    // Create test world
    UWorld* TestWorld = UWorld::CreateWorld(EWorldType::Game, false);
    
    // Create test actor with weapon component
    APlayerShip* Ship = TestWorld->SpawnActor<APlayerShip>();
    UWeaponSystemComponent* WeaponSystem = Ship->FindComponentByClass<UWeaponSystemComponent>();
    
    // Test firing
    WeaponSystem->Fire();
    TestTrue("Weapon should have fired", WeaponSystem->HasFired());
    
    // Cleanup
    TestWorld->DestroyWorld(false);
    
    return true;
}
```

---

## Performance Patterns

### Object Pooling

```cpp
// Pool for frequently spawned objects (projectiles, pickups)
UCLASS()
class UObjectPool : public UObject {
    GENERATED_BODY()
    
    UPROPERTY()
    TArray<AActor*> Pool;
    
    UPROPERTY()
    TSubclassOf<AActor> PooledClass;
    
public:
    AActor* GetPooledObject(UWorld* World) {
        for (AActor* Actor : Pool) {
            if (!Actor->IsActorTickEnabled()) {
                Actor->SetActorTickEnabled(true);
                return Actor;
            }
        }
        
        // Pool exhausted, create new
        AActor* NewActor = World->SpawnActor<AActor>(PooledClass);
        Pool.Add(NewActor);
        return NewActor;
    }
    
    void ReturnToPool(AActor* Actor) {
        Actor->SetActorHiddenInGame(true);
        Actor->SetActorTickEnabled(false);
    }
};
```

### Efficient Enemy Spawning

```cpp
// Spawn enemies using pooling
UCLASS()
class AWaveSpawner : public AActor {
    GENERATED_BODY()
    
    UPROPERTY()
    TMap<TSubclassOf<AEnemyBase>, UObjectPool*> EnemyPools;
    
public:
    AEnemyBase* SpawnEnemy(TSubclassOf<AEnemyBase> EnemyClass, FVector Location) {
        UObjectPool* Pool = EnemyPools.FindOrAdd(EnemyClass);
        AEnemyBase* Enemy = Cast<AEnemyBase>(Pool->GetPooledObject(GetWorld()));
        Enemy->SetActorLocation(Location);
        Enemy->Reset();
        return Enemy;
    }
};
```

---

## Best Practices

### Naming Conventions

```cpp
// Follow UE5 coding standard
UCLASS()           // U prefix for UObject-derived
ACLASS()           // A prefix for AActor-derived
FSTRUCT()          // F prefix for structs
EENUMERATION()     // E prefix for enums
IINTERFACE()       // I prefix for interfaces

// Member variables
UPROPERTY()
float MaxHealth;   // No prefix for members

// Local variables
float LocalHealth; // No prefix for locals

// Booleans
bool bIsAlive;     // b prefix for booleans
```

### Memory Management

```cpp
// Use UPROPERTY for UObject references (garbage collection)
UPROPERTY()
UHealthComponent* HealthComponent;

// Use TSharedPtr for non-UObject smart pointers
TSharedPtr<FSomeNonUObjectClass> SharedData;

// Use TWeakObjectPtr to avoid circular references
UPROPERTY()
TWeakObjectPtr<AActor> WeakReference;
```

### Blueprint Integration

```cpp
// Expose to Blueprint where appropriate
UCLASS(Blueprintable)
class APlayerShip : public APawn {
    GENERATED_BODY()
    
    // BlueprintCallable for Blueprint scripts to call
    UFUNCTION(BlueprintCallable, Category="Weapons")
    void FireWeapon();
    
    // BlueprintImplementableEvent for Blueprint to implement
    UFUNCTION(BlueprintImplementableEvent, Category="Effects")
    void OnLevelUp();
    
    // BlueprintNativeEvent for C++ with Blueprint override
    UFUNCTION(BlueprintNativeEvent, Category="Damage")
    void TakeDamage(float Amount);
    virtual void TakeDamage_Implementation(float Amount);
};
```

---

## Migration Notes (Unity → Unreal)

**ScriptableObjects → Data Assets**
- Create UDataAsset-derived classes
- EditDefaultsOnly for configuration
- Reference in actors via UPROPERTY

**MonoBehaviour → ActorComponent**
- Component-based architecture similar
- Use UActorComponent for reusable logic
- Attach to actors, not inheritance

**Prefabs → Blueprint Classes**
- Create Blueprint classes from C++ base classes
- Configure Data Assets in Blueprint defaults
- Instantiate via SpawnActor

**Coroutines → Timers**
- Use FTimerHandle and GetWorldTimerManager()
- Or use UE's Task Graph for async operations

---

**Last Updated:** November 13, 2025  
**Document Version:** 2.0

