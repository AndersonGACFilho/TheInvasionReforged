# TIRCore Module

**Module Type:** Foundation  
**Dependencies:** None (Engine modules only)  
**Depended By:** All project modules  
**Updated:** November 17, 2025

---

## Purpose

TIRCore is the foundation module that provides:
- **Shared interfaces** for cross-module communication
- **Gameplay tags** for data-driven systems
- **Common types** (structs, enums, base classes)
- **Utility functions** shared across the project

**Critical Rule:** TIRCore has ZERO dependencies on other project modules.

---

## Module Structure

```
TIRCore/
├── Public/
│   ├── Interfaces/
│   │   ├── ITIRDamageable.h          # Damage receiver interface
│   │   ├── ITIRCollectable.h         # Pickup interface
│   │   ├── ITIRTargetable.h          # AI targeting interface
│   │   ├── ITIRPoolable.h            # Object pooling interface
│   │   └── ITIRUpgradeable.h         # Progression system interface
│   │
│   ├── Types/
│   │   ├── TIRGameplayTags.h         # Centralized gameplay tags
│   │   ├── TIREnums.h                # Common enumerations
│   │   └── TIRStructs.h              # Shared data structures
│   │
│   ├── Data/
│   │   └── TIRDataAsset.h            # Base class for all Data Assets
│   │
│   └── Utils/
│       ├── TIRMathLibrary.h          # Math utilities
│       └── TIRLogCategories.h        # Logging categories
│
└── Private/
    └── Utils/
        └── TIRLogCategories.cpp       # Log category definitions
```

---

## Key Components

### Interfaces

#### ITIRDamageable
**Purpose:** Any actor that can take damage  
**Used By:** TIRCombat, TIRAi, TIREnvironment

**Methods:**
- `TakeDamage(float Amount, AActor* Instigator)`
- `GetCurrentHealth()`
- `IsDead()`

---

#### ITIRPoolable
**Purpose:** Actors that can be pooled for performance  
**Used By:** TIRSpawning, TIRVfx

**Methods:**
- `OnAcquireFromPool()` - Called when retrieved from pool
- `OnReturnToPool()` - Called when returned to pool

---

#### ITIRTargetable
**Purpose:** Entities that can be targeted by AI  
**Used By:** TIRAi, TIRCombat

**Methods:**
- `GetTargetPriority()` - Used for AI target selection
- `IsValidTarget()` - Check if can be targeted
- `GetTargetLocation()` - Position for aiming

---

### Gameplay Tags

Centralized gameplay tag definitions accessed via singleton:

```cpp
const FTIRGameplayTags& Tags = FTIRGameplayTags::Get();
```

**Categories:**
- `Input.*` - Input actions
- `Weapon.*` - Weapon types and states
- `Ability.*` - Player abilities
- `Upgrade.*` - Progression upgrades
- `State.*` - Actor states (invulnerable, stunned, etc.)
- `Enemy.*` - Enemy types and behaviors
- `Damage.*` - Damage types

**Configuration:** See `Config/DefaultGameplayTags.ini`

---

### Common Types

#### Enums
- `ETIRWeaponType` - Weapon categories
- `ETIRDamageType` - Damage type classification
- `ETIREnemyType` - Enemy categories
- `ETIRUpgradeRarity` - Upgrade quality levels

#### Structs
- `FTIRDamageInfo` - Damage event data
- `FTIRUpgradeDefinition` - Upgrade configuration
- `FTIRWaveConfiguration` - Wave spawn data

---

### Logging Categories

All modules use standardized logging:

```cpp
DECLARE_LOG_CATEGORY_EXTERN(LogTIRCore, Log, All);
DECLARE_LOG_CATEGORY_EXTERN(LogTIRCombat, Log, All);
DECLARE_LOG_CATEGORY_EXTERN(LogTIRMovement, Log, All);
DECLARE_LOG_CATEGORY_EXTERN(LogTIRAi, Log, All);
DECLARE_LOG_CATEGORY_EXTERN(LogTIRProgression, Log, All);
DECLARE_LOG_CATEGORY_EXTERN(LogTIRSpawning, Log, All);
DECLARE_LOG_CATEGORY_EXTERN(LogTIRUi, Log, All);
```

**Usage:**
```cpp
UE_LOG(LogTIRCombat, Warning, TEXT("Health depleted for %s"), *ActorName);
```

---

## Build Configuration

### TIRCore.Build.cs

```csharp
public class TIRCore : ModuleRules
{
    public TIRCore(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
        
        PublicDependencyModuleNames.AddRange(new string[] 
        {
            "Core",
            "CoreUObject",
            "Engine",
            "GameplayTags"
        });
        
        // NO project module dependencies
    }
}
```

---

## Design Principles

### 1. Zero Project Dependencies
TIRCore must never reference other project modules. This ensures:
- No circular dependencies possible
- Fast compilation
- Clear foundation layer

### 2. Interface-First Design
All cross-module communication uses interfaces defined here:
- Modules implement interfaces
- Modules call via interface pointers
- No direct module-to-module coupling

### 3. Data-Driven Configuration
Prefer configuration over hardcoding:
- Gameplay tags in .ini files
- Data Assets for configuration
- Blueprint-accessible where appropriate

---

## Common Patterns

### Pattern: Implementing an Interface

```cpp
// TIRCombat/Public/Components/TIRHealthComponent.h
#include "Interfaces/ITIRDamageable.h"

UCLASS()
class UTIRHealthComponent : public UActorComponent, public ITIRDamageable
{
    GENERATED_BODY()
    
    // ITIRDamageable implementation
    virtual void TakeDamage_Implementation(float Amount, AActor* Instigator) override;
    virtual float GetCurrentHealth_Implementation() const override;
    virtual bool IsDead_Implementation() const override;
};
```

### Pattern: Using an Interface

```cpp
// TIRAi/Private/SomeAICode.cpp
#include "Interfaces/ITIRDamageable.h"

void AttackTarget(AActor* Target)
{
    if (ITIRDamageable* Damageable = Cast<ITIRDamageable>(Target))
    {
        ITIRDamageable::Execute_TakeDamage(Target, 10.0f, this);
    }
}
```

### Pattern: Using Gameplay Tags

```cpp
#include "Types/TIRGameplayTags.h"

void CheckAbility()
{
    const FTIRGameplayTags& Tags = FTIRGameplayTags::Get();
    
    if (AbilityTag == Tags.Ability_SingularityRay)
    {
        // Execute ability
    }
}
```

---

## Migration Notes

### From Legacy Module

When migrating code from `TheInvasionReforged` module:

1. **Identify shared concepts** - Interfaces, tags, types
2. **Move to TIRCore** - Create in appropriate subdirectory
3. **Update includes** - Point to new TIRCore headers
4. **Update .Build.cs** - Add TIRCore dependency

### Adding New Interfaces

Checklist:
- [ ] Is this used by multiple modules? (If not, keep in specific module)
- [ ] Does it create a circular dependency? (Move to TIRCore fixes this)
- [ ] Is it a UINTERFACE? (Must be for Blueprint support)
- [ ] Are all methods BlueprintNativeEvent? (For Blueprint override)
- [ ] Is it documented? (Purpose, usage, example)

---

## Testing

### Interface Testing

Interfaces are tested via consuming modules (TIRCombat, TIRAi, etc.)

### Gameplay Tag Testing

```cpp
// Verify tags are registered
IMPLEMENT_SIMPLE_AUTOMATION_TEST(
    FTIRGameplayTagsTest,
    "TheInvasionReforged.Core.GameplayTags",
    EAutomationTestFlags::ApplicationContextMask | EAutomationTestFlags::ProductFilter
)

bool FTIRGameplayTagsTest::RunTest(const FString& Parameters)
{
    const FTIRGameplayTags& Tags = FTIRGameplayTags::Get();
    
    TestTrue("Weapon tag is valid", Tags.Weapon_Primary_PlasmaBeam.IsValid());
    TestTrue("Ability tag is valid", Tags.Ability_SingularityRay.IsValid());
    
    return true;
}
```

---

## Future Enhancements

### Phase 1 (Current)
- ✅ Basic interfaces (Damageable, Poolable, Targetable)
- ✅ Core gameplay tags
- ✅ Logging categories

### Phase 2 (Week 9-10)
- [ ] Add ITIRInteractable interface (for hangar terminals)
- [ ] Add meta-progression tags
- [ ] Add status effect interfaces

### Phase 3 (Post-Launch)
- [ ] GAS integration (Attribute Sets, Gameplay Effects)
- [ ] Network replication helpers
- [ ] Advanced math utilities

---

## Related Documentation

- [Intermodule Dependencies](../INTERMODULE_DEPENDENCIES.md)
- [Module Architecture Overview](../MODULE_ARCHITECTURE.md)
- Consuming Modules:
  - [TIRCombat](./TIRCombat.md)
  - [TIRAi](./TIRAi.md)
  - [TIRSpawning](./TIRSpawning.md)

---

## References

- [Unreal Engine Interfaces](https://dev.epicgames.com/documentation/en-us/unreal-engine/interfaces-in-unreal-engine)
- [Gameplay Tags](https://dev.epicgames.com/documentation/en-us/unreal-engine/using-gameplay-tags-in-unreal-engine)

---

**Last Updated:** November 17, 2025  
**Document Version:** 1.0  
**Author:** Anderson (Lead Developer)

