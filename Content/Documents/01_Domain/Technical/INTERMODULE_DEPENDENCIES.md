# Intermodule Dependencies

**Project:** The Invasion Reforged  
**Engine:** Unreal Engine 5.7  
**Updated:** November 17, 2025  
**Version:** 1.0

---

## Overview

This document defines the dependency rules and relationships between all modules in the project. Understanding these dependencies is critical for:

- **Avoiding circular dependencies**
- **Minimizing compile times**
- **Maintaining clean architecture**
- **Enabling parallel development**

---

## Dependency Rules

### Core Principles

1. **TIRCore has ZERO dependencies** - It's the foundation
2. **No circular dependencies allowed** - Enforced at compile time
3. **Use interfaces for cross-module communication** - Defined in TIRCore
4. **Minimize public dependencies** - Use private when possible
5. **Visual/Audio modules never depend on gameplay** - One-way communication only

### Dependency Levels

```
Level 0 (Foundation):
    TIRCore
    
Level 1 (Core Systems):
    TIRInput → TIRCore
    TIRMovement → TIRCore
    TIRCombat → TIRCore
    
Level 2 (Gameplay Systems):
    TIRProgression → TIRCore, TIRCombat
    TIRAi → TIRCore, TIRMovement, TIRCombat
    TIRSpawning → TIRCore
    TIRCollectables → TIRCore
    
Level 3 (Environment & Meta):
    TIREnvironment → TIRCore, TIRSpawning
    TIRHangar → TIRCore, TIRProgression
    
Level 4 (Presentation):
    TIRVfx → TIRCore
    TIRUi → TIRCore, TIRProgression, TIRCombat
    
Level 5 (Framework):
    TIRGameFramework → All modules (orchestrates)
    TIRSaveGame → TIRCore, TIRProgression
    
Level 6 (Future):
    TIROnline → TIRCore, TIRProgression
```

---

## Module Dependency Matrix

| Module | Public Dependencies | Private Dependencies | Prohibited |
|--------|-------------------|---------------------|------------|
| **TIRCore** | *(none)* | *(none)* | All modules |
| **TIRInput** | TIRCore | EnhancedInput, Slate | TIRCombat, TIRMovement |
| **TIRMovement** | TIRCore | Engine (UFloatingPawnMovement) | TIRInput, TIRCombat |
| **TIRCombat** | TIRCore | TIRVfx (optional) | TIRAi, TIRProgression |
| **TIRProgression** | TIRCore, TIRCombat | - | TIRAi, TIRSpawning |
| **TIRAi** | TIRCore, TIRMovement | AIModule, TIRCombat | TIRProgression |
| **TIRSpawning** | TIRCore | - | TIRCombat, TIRAi |
| **TIRCollectables** | TIRCore | - | TIRProgression |
| **TIREnvironment** | TIRCore | TIRSpawning | TIRCombat, TIRAi |
| **TIRHangar** | TIRCore, TIRProgression | - | TIRCombat, TIRAi |
| **TIRVfx** | TIRCore | Niagara | **ALL gameplay modules** |
| **TIRUi** | TIRCore, TIRProgression | TIRCombat, UMG | TIRAi, TIRSpawning |
| **TIRGameFramework** | All modules | - | *(orchestrator)* |
| **TIRSaveGame** | TIRCore, TIRProgression | - | TIRCombat, TIRAi |
| **TIROnline** | TIRCore, TIRProgression | OnlineSubsystem | All others |

---

## Detailed Dependency Graphs

### TIRCore Dependencies

```
TIRCore (ZERO dependencies)
├── Unreal Engine Modules Only:
│   ├── Core
│   ├── CoreUObject
│   ├── Engine
│   └── GameplayTags
└── Contains:
    ├── Interfaces (ITIRDamageable, ITIRPoolable, etc.)
    ├── Gameplay Tags (FTIRGameplayTags)
    ├── Enums & Structs
    └── Base Data Asset classes
```

**Rule:** TIRCore must NEVER reference any project module.

---

### TIRCombat Dependencies

```
TIRCombat
├── Public Dependencies:
│   └── TIRCore (interfaces, tags)
│
└── Private Dependencies:
    └── TIRVfx (optional - for damage effects)
    
Depends ON: TIRCore
Depended BY: TIRProgression, TIRAi, TIRGameFramework, TIRUi
```

**Communication:**
- Uses `ITIRDamageable` interface from TIRCore
- Broadcasts damage events via delegates
- Other modules listen to events, don't call directly

---

### TIRAi Dependencies

```
TIRAi
├── Public Dependencies:
│   ├── TIRCore
│   ├── TIRMovement (for enemy movement)
│   ├── AIModule (Behavior Trees)
│   └── GameplayTasks
│
└── Private Dependencies:
    └── TIRCombat (for attacking/taking damage)
    
Depends ON: TIRCore, TIRMovement, AIModule
Depended BY: TIRGameFramework
```

**Why TIRMovement is public:**
- AI controllers need to command movement directly
- Shared movement components used by AI and players

**Why TIRCombat is private:**
- AI doesn't expose combat logic
- Combat is internal implementation detail

---

### TIRProgression Dependencies

```
TIRProgression
├── Public Dependencies:
│   ├── TIRCore
│   └── TIRCombat (for damage/health modifiers)
│
└── Private Dependencies: *(none)*
    
Depends ON: TIRCore, TIRCombat
Depended BY: TIRHangar, TIRUi, TIRSaveGame, TIRGameFramework
```

**Design Decision:**
- Progression modifies combat stats (damage, health)
- Uses TIRCombat components, but doesn't control combat logic

---

### TIRVfx Dependencies

```
TIRVfx
├── Public Dependencies:
│   └── TIRCore (for pooling interfaces)
│
├── Private Dependencies:
│   └── Niagara
│
└── PROHIBITED Dependencies:
    └── ALL gameplay modules (TIRCombat, TIRAi, etc.)
```

**Critical Rule:**
- VFX is a **pure presentation layer**
- Gameplay modules call TIRVfx
- TIRVfx **NEVER** calls back to gameplay
- Communication: One-way only

**Example:**
```cpp
// ✅ CORRECT
TIRCombat → spawns VFX via TIRVfx

// ❌ WRONG
TIRVfx → queries TIRCombat for damage values
```

---

### TIRInput Dependencies

```
TIRInput
├── Public Dependencies:
│   ├── TIRCore (for gameplay tags)
│   ├── EnhancedInput
│   └── GameplayTags
│
└── Private Dependencies:
    ├── Slate (for mobile input)
    └── SlateCore
    
Depends ON: TIRCore, EnhancedInput
Depended BY: TIRGameFramework
```

**Design:**
- Input is separate from movement/combat
- Uses gameplay tags to map actions
- No direct dependency on TIRMovement or TIRCombat

---

### TIRGameFramework Dependencies

```
TIRGameFramework (Special - Can depend on ALL)
├── Public Dependencies:
│   ├── TIRCore
│   ├── TIRCombat
│   ├── TIRMovement
│   ├── TIRProgression
│   ├── TIRSpawning
│   ├── TIRAi
│   ├── TIREnvironment
│   ├── TIRUi
│   └── TIRSaveGame
│
└── Private Dependencies:
    ├── EnhancedInput
    └── TIRInput
```

**Role:**
- Game Mode orchestrates systems
- Player Controller bridges input → gameplay
- Game State tracks global state
- **No business logic** - only orchestration

---

## Circular Dependency Prevention

### Prohibited Patterns

```
❌ CIRCULAR - Not Allowed
TIRCombat → TIRAi → TIRCombat

❌ CIRCULAR - Not Allowed  
TIRVfx → TIRCombat → TIRVfx

❌ CIRCULAR - Not Allowed
TIRSpawning → TIRAi → TIRSpawning
```

### Solutions

**Problem:** Module A needs to communicate with Module B, and B needs A.

**Solution 1: Interface in TIRCore**
```cpp
// TIRCore defines interface
class ITIRDamageable

// TIRCombat implements
class UTIRHealthComponent : public ITIRDamageable

// TIRAi uses interface (no dependency on TIRCombat)
ITIRDamageable* Target = Cast<ITIRDamageable>(Actor);
```

**Solution 2: Event Broadcasting**
```cpp
// TIRCombat broadcasts event
OnEnemyKilled.Broadcast(Enemy);

// TIRProgression listens (no dependency on TIRCombat)
HealthComp->OnDeath.AddDynamic(this, &UXPComponent::OnEnemyDied);
```

**Solution 3: Dependency Inversion**
```
Instead of: TIRAi → TIRSpawning → TIRAi
Use:        TIRAi → TIRCore ← TIRSpawning
           (both use interfaces from TIRCore)
```

---

## Build Configuration

### Module Load Order

Unreal loads modules in dependency order. Our structure:

```
1. TIRCore (no dependencies)
2. TIRInput, TIRMovement, TIRCombat (depend on TIRCore)
3. TIRProgression, TIRAi, TIRSpawning (depend on Level 1)
4. TIREnvironment, TIRHangar (depend on Level 2)
5. TIRVfx, TIRUi (presentation layer)
6. TIRGameFramework (orchestration)
7. TIRSaveGame, TIROnline (services)
```

### TheInvasionReforged.Target.cs

```csharp
using UnrealBuildTool;

public class TheInvasionReforgedTarget : TargetRules
{
    public TheInvasionReforgedTarget(TargetInfo Target) : base(Target)
    {
        Type = TargetType.Game;
        DefaultBuildSettings = BuildSettingsVersion.V5;
        IncludeOrderVersion = EngineIncludeOrderVersion.Latest;
        
        // Modules loaded in dependency order
        ExtraModuleNames.AddRange(new string[] 
        { 
            "TheInvasionReforged",  // Legacy module
            
            // Level 0
            "TIRCore",
            
            // Level 1
            "TIRInput",
            "TIRMovement",
            "TIRCombat",
            
            // Level 2
            "TIRProgression",
            "TIRAi",
            "TIRSpawning",
            "TIRCollectables",
            
            // Level 3
            "TIREnvironment",
            "TIRHangar",
            
            // Level 4
            "TIRVfx",
            "TIRUi",
            
            // Level 5
            "TIRGameFramework",
            "TIRSaveGame",
            
            // Level 6 (Future)
            // "TIROnline",
        });
    }
}
```

---

## Testing Dependencies

### Unit Test Modules

Each gameplay module has optional test module:

```
TIRCombatTests.Build.cs:
- Depends on: TIRCombat, TIRCore
- Depends on: UnrealEd (for automation framework)
- Type: UncookedOnly (not in shipping builds)
```

**Example:**
```csharp
public class TIRCombatTests : ModuleRules
{
    public TIRCombatTests(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
        
        PublicDependencyModuleNames.AddRange(new string[] 
        {
            "Core",
            "CoreUObject",
            "Engine",
            "TIRCore",
            "TIRCombat"
        });
        
        PrivateDependencyModuleNames.AddRange(new string[] 
        {
            "UnrealEd",
            "AutomationController"
        });
    }
}
```

---

## Dependency Validation

### Compile-Time Checks

Unreal Build Tool enforces:
- ✅ No circular dependencies
- ✅ Public vs Private separation
- ✅ Module load order

### Runtime Checks

Use `ENSURE` macros for interface validation:

```cpp
// Verify actor implements required interface
ITIRDamageable* Damageable = Cast<ITIRDamageable>(Target);
ensureMsgf(Damageable, TEXT("Target %s doesn't implement ITIRDamageable"), 
           *Target->GetName());
```

### Code Review Checklist

Before merging new module dependencies:

- [ ] Is this dependency necessary?
- [ ] Can we use an interface instead?
- [ ] Does this create a circular dependency?
- [ ] Should this be private instead of public?
- [ ] Does this increase compile times significantly?

---

## Common Dependency Patterns

### Pattern 1: Component Communication via Interfaces

```cpp
// TIRCore/Public/Interfaces/ITIRDamageable.h
UINTERFACE()
class UTIRDamageable : public UInterface { GENERATED_BODY() };

class ITIRDamageable
{
    GENERATED_BODY()
public:
    virtual void TakeDamage(float Amount, AActor* Instigator) = 0;
};

// TIRCombat implements
class UTIRHealthComponent : public UActorComponent, public ITIRDamageable
{
    virtual void TakeDamage(float Amount, AActor* Instigator) override;
};

// TIRAi uses (no dependency on TIRCombat)
if (ITIRDamageable* Target = Cast<ITIRDamageable>(Actor))
{
    Target->TakeDamage(Damage, this);
}
```

### Pattern 2: Event-Driven Communication

```cpp
// TIRCombat broadcasts
DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FOnEnemyKilled, AActor*, Enemy);

class UTIRHealthComponent : public UActorComponent
{
    UPROPERTY(BlueprintAssignable)
    FOnEnemyKilled OnDeath;
};

// TIRProgression listens
void UTIRExperienceComponent::BeginPlay()
{
    UTIRHealthComponent* Health = GetOwner()->FindComponentByClass<UTIRHealthComponent>();
    Health->OnDeath.AddDynamic(this, &UTIRExperienceComponent::OnEnemyDied);
}
```

### Pattern 3: Dependency Injection

```cpp
// Instead of: TIRSpawning → TIRAi (circular)
// Use: Inject AI class as parameter

class UTIRWaveManager : public UActorComponent
{
    UPROPERTY(EditDefaultsOnly)
    TArray<TSubclassOf<AActor>> EnemyClasses;  // AI classes injected via data
};
```

---

## Migration Path

### Current State (Week 7)
- All code in `TheInvasionReforged` module
- No separation

### Phase 1 (Week 7-8): Core Modules
1. Create TIRCore
2. Move interfaces, tags, base classes
3. Create TIRCombat, TIRMovement, TIRInput
4. Update .Build.cs files

### Phase 2 (Week 9-10): Gameplay Modules
1. Create TIRProgression, TIRAi, TIRSpawning
2. Verify no circular dependencies
3. Update dependencies in .Build.cs

### Phase 3 (Week 11-12): Polish Modules
1. Create TIRVfx, TIRUi, TIRGameFramework
2. Final dependency validation
3. Remove legacy `TheInvasionReforged` module

---

## Troubleshooting

### Error: Circular Dependency Detected

```
ERROR: Found circular dependency:
  TIRCombat -> TIRAi -> TIRCombat
```

**Solution:**
1. Identify shared concept (e.g., "Damageable")
2. Move to TIRCore as interface
3. Both modules depend on TIRCore (no circle)

### Error: Unresolved External Symbol

```
ERROR: Unresolved external symbol ITIRDamageable
```

**Solution:**
- Add `TIRCore` to PublicDependencyModuleNames in .Build.cs
- Include proper header: `#include "Interfaces/ITIRDamageable.h"`

### Warning: Long Compile Times

**Cause:** Too many public dependencies

**Solution:**
- Move dependencies from `PublicDependencyModuleNames` to `PrivateDependencyModuleNames`
- Use forward declarations in headers
- Only include full headers in .cpp files

---

## Best Practices

### DO ✅

- **Use interfaces from TIRCore** for cross-module communication
- **Keep modules focused** - Single responsibility
- **Minimize public dependencies** - Hide implementation details
- **Use events/delegates** for loose coupling
- **Document why** a dependency exists

### DON'T ❌

- **Create circular dependencies** - Enforce at design time
- **Make VFX/Audio depend on gameplay** - One-way only
- **Add dependencies "just in case"** - Only when needed
- **Expose implementation details** - Use private dependencies
- **Skip code reviews** for new dependencies

---

## Related Documentation

- [Module Architecture Overview](./MODULE_ARCHITECTURE.md)
- Individual Module Docs:
  - [TIRCore](./Modules/TIRCore.md)
  - [TIRCombat](./Modules/TIRCombat.md)
  - [TIRMovement](./Modules/TIRMovement.md)
  - [TIRInput](./Modules/TIRInput.md)
  - *(See `Modules/` directory for complete list)*

---

**Last Updated:** November 17, 2025  
**Document Version:** 1.0  
**Author:** Anderson (Lead Developer)

