# Module Architecture: The Invasion Reforged

**Project:** The Invasion Reforged  
**Engine:** Unreal Engine 5.7 (Latest Release - Some Features Experimental)  
**Architecture:** Modular, Component-Based, GAS-Ready, Multiplayer-Ready  
**Testing:** Automation Framework (TDD)  
**Updated:** November 17, 2025  
**Status:** Living Document - Implementation in Progress

---

## Overview

This document defines the project's modular architecture following Unreal Engine 5 best practices, including:
- **Separation of concerns** (each module has a single purpose)
- **Testability** (isolated modules, clear interfaces)
- **Scalability** (prepared for GAS and multiplayer)
- **Maintainability** (organized code, explicit dependencies)

---

## Core Principles

### 1. Domain-Based Modularization
Each module represents a specific domain of functionality, not a technical layer.

### 2. Component-Based Architecture
Use Unreal's composition system instead of deep inheritance hierarchies.

### 3. Interface-Driven Design
Communication between modules via C++ interfaces, not direct coupling.

### 4. Data-Driven Configuration
Use Data Assets and Data Tables for configuration, not hardcoding.

### 5. Event-Driven Communication
Delegates and events for asynchronous communication between systems.

### 6. Testability First
Pure C++ code separated from Actors/Components for fast unit tests.

---

## Module Structure

```
Source/
├── TheInvasionReforged/              # Primary module (game module)
│   └── [Legacy code, should migrate to specific modules]
│
├── TIRCore/                           # Core - Types, interfaces, utilities
│   ├── Public/
│   │   ├── Interfaces/
│   │   │   ├── ITIRDamageable.h
│   │   │   ├── ITIRCollectable.h
│   │   │   ├── ITIRTargetable.h
│   │   │   ├── ITIRPoolable.h
│   │   │   └── ITIRUpgradeable.h
│   │   ├── Types/
│   │   │   ├── TIRGameplayTags.h
│   │   │   ├── TIREnums.h
│   │   │   └── TIRStructs.h
│   │   ├── Data/
│   │   │   └── TIRDataAsset.h      # Base class for all Data Assets
│   │   └── Utils/
│   │       ├── TIRMathLibrary.h
│   │       └── TIRLogCategories.h
│   └── Private/
│
├── TIRCombat/                         # Combat system
│   ├── Public/
│   │   ├── Components/
│   │   │   ├── TIRHealthComponent.h
│   │   │   ├── TIRDamageComponent.h
│   │   │   ├── TIRShieldComponent.h
│   │   │   └── TIRTargetingComponent.h
│   │   ├── Weapons/
│   │   │   ├── TIRWeaponBase.h
│   │   │   ├── TIRWeaponComponent.h
│   │   │   ├── TIRProjectile.h
│   │   │   └── Data/
│   │   │       └── TIRWeaponData.h
│   │   ├── Abilities/
│   │   │   ├── TIRAbilityBase.h
│   │   │   ├── TIRAbilityComponent.h
│   │   │   └── Data/
│   │   │       └── TIRAbilityData.h
│   │   └── Damage/
│   │       ├── TIRDamageCalculator.h   # Pure C++ - testable
│   │       └── TIRDamageTypes.h
│   └── Private/
│       └── Tests/
│           ├── HealthComponentTest.cpp
│           ├── DamageCalculatorTest.cpp
│           └── WeaponSystemTest.cpp
│
├── TIRMovement/                       # Movement system (extends Unreal's movement)
│   ├── Public/
│   │   ├── Components/
│   │   │   ├── TIRShipMovementComponent.h      # Extends UFloatingPawnMovement
│   │   │   ├── TIRDashComponent.h               # AbilityTask for dash
│   │   │   └── TIRSpacePhysicsComponent.h       # Zero-G physics modifiers
│   │   ├── Physics/
│   │   │   ├── TIRDriftPhysics.h                # Pure C++ - testable
│   │   │   └── TIRMovementPhysicsCalculator.h   # Custom physics calculations
│   │   └── Data/
│   │       └── TIRMovementData.h
│   └── Private/
│       └── Tests/
│           └── MovementPhysicsTest.cpp
│
│   # NOTE: Uses as base:
│   # - UFloatingPawnMovement (8-directional movement)
│   # - UPawnMovementComponent (base component)
│   # - Enhanced Input System (already in project)
│
├── TIRAi/                             # Artificial intelligence (uses Unreal AIModule)
│   ├── Public/
│   │   ├── Controllers/
│   │   │   └── TIREnemyAIController.h         # Extends AAIController
│   │   ├── BehaviorTree/
│   │   │   ├── Tasks/
│   │   │   │   ├── BTTask_FindTarget.h        # Extends UBTTaskNode
│   │   │   │   ├── BTTask_OrbitPlayer.h
│   │   │   │   └── BTTask_KamikazeRush.h
│   │   │   └── Services/
│   │   │       ├── BTService_UpdateTarget.h   # Extends UBTService
│   │   │       └── BTService_CheckDistance.h
│   │   ├── Strategies/                         # Pure C++ - testable
│   │   │   ├── TIRAIStrategyBase.h
│   │   │   ├── TIRSwarmStrategy.h
│   │   │   └── TIRKamikazeStrategy.h
│   │   └── Data/
│   │       └── TIREnemyData.h
│   └── Private/
│       └── Tests/
│           └── AIStrategyTest.cpp
│
│   # NOTE: Uses native Behavior Tree System:
│   # - UBehaviorTree (asset)
│   # - UBehaviorTreeComponent
│   # - UBlackboardComponent
│   # - AAIController
│   # Requires: AIModule in .Build.cs
│
├── TIRProgression/                    # Progression (XP + Upgrades)
│   ├── Public/
│   │   ├── Components/
│   │   │   ├── TIRExperienceComponent.h
│   │   │   ├── TIRUpgradeComponent.h
│   │   │   └── TIRMetaProgressionComponent.h
│   │   ├── Systems/                   # Pure C++ - testable
│   │   │   ├── TIRExperienceCalculator.h
│   │   │   ├── TIRUpgradeApplicator.h
│   │   │   └── TIRStatModifierSystem.h
│   │   ├── Data/
│   │   │   ├── TIRUpgradeData.h
│   │   │   ├── TIRStatModifierData.h
│   │   │   └── TIRProgressionCurve.h
│   │   └── Managers/
│   │       └── TIRMetaProgressionManager.h
│   └── Private/
│       └── Tests/
│           ├── ExperienceSystemTest.cpp
│           ├── UpgradeSystemTest.cpp
│           └── StatModifierTest.cpp
│
├── TIRSpawning/                       # Enemy/wave spawning system
│   ├── Public/
│   │   ├── Managers/
│   │   │   ├── TIRWaveManager.h
│   │   │   └── TIRSpawnManager.h
│   │   ├── Spawners/
│   │   │   ├── TIRSpawnPoint.h
│   │   │   └── TIRSpawnPattern.h      # Pure C++ - testable
│   │   ├── Pooling/
│   │   │   └── TIRObjectPool.h
│   │   └── Data/
│   │       ├── TIRWaveData.h
│   │       └── TIRSpawnPatternData.h
│   └── Private/
│       └── Tests/
│           ├── WaveSystemTest.cpp
│           └── ObjectPoolTest.cpp
│
├── TIRCollectables/                   # Pickups (scrap, power-ups)
│   ├── Public/
│   │   ├── Actors/
│   │   │   ├── TIRCollectableBase.h
│   │   │   ├── TIRScrapPickup.h
│   │   │   └── TIRPowerUpPickup.h
│   │   ├── Components/
│   │   │   └── TIRCollectorComponent.h
│   │   └── Data/
│   │       └── TIRCollectableData.h
│   └── Private/
│       └── Tests/
│           └── CollectableSystemTest.cpp
│
├── TIREnvironment/                    # Arenas and environment
│   ├── Public/
│   │   ├── Arenas/
│   │   │   ├── TIRArenaBase.h
│   │   │   └── TIRArenaModifier.h     # Special effects per arena
│   │   ├── Hazards/
│   │   │   └── TIREnvironmentalHazard.h
│   │   └── Data/
│   │       └── TIRArenaData.h
│   └── Private/
│
├── TIRHangar/                         # Hangar hub (meta game)
│   ├── Public/
│   │   ├── Actors/
│   │   │   ├── TIRHangarPilot.h
│   │   │   ├── TIRUpgradeTerminal.h
│   │   │   └── TIRShipCockpit.h
│   │   ├── Components/
│   │   │   └── TIRInteractionComponent.h
│   │   └── Managers/
│   │       └── TIRHangarManager.h
│   └── Private/
│
├── TIRVfx/                            # Visual effects
│   ├── Public/
│   │   ├── Components/
│   │   │   └── TIRVFXComponent.h
│   │   ├── Pooling/
│   │   │   └── TIRVFXPool.h
│   │   └── Data/
│   │       └── TIRVFXData.h
│   └── Private/
│
├── TIRUi/                             # User interface
│   ├── Public/
│   │   ├── Widgets/
│   │   │   ├── TIRGameHUD.h
│   │   │   ├── TIRUpgradeSelectionWidget.h
│   │   │   ├── TIRPostRunWidget.h
│   │   │   └── TIRMainMenuWidget.h
│   │   ├── Components/
│   │   │   └── TIRUIComponent.h
│   │   └── Data/
│   │       └── TIRUIData.h
│   └── Private/
│
├── TIRInput/                          # Input system (Enhanced Input System wrapper)
│   ├── Public/
│   │   ├── Components/
│   │   │   └── TIREnhancedInputComponent.h     # Extends UEnhancedInputComponent
│   │   ├── Data/
│   │   │   ├── TIRInputConfig.h                # Config Asset
│   │   │   ├── DA_InputMappingContext.h        # Data Asset (IMC)
│   │   │   └── IA_*.h                          # Input Actions (Move, Dash, Fire)
│   │   ├── Processors/
│   │   │   ├── TIRMobileInputProcessor.h       # Touch input handling
│   │   │   └── TIRDeadZoneModifier.h           # Custom input modifiers
│   │   └── Subsystems/
│   │       └── TIRInputSubsystem.h             # Manage input contexts
│   └── Private/
│
│   # NOTE: Uses Enhanced Input System (UE 5.1+):
│   # - UEnhancedInputComponent
│   # - UInputMappingContext
│   # - UInputAction
│   # - Input Modifiers & Triggers
│   # Project already has dependency: EnhancedInput in .Build.cs
│
├── TIRSaveGame/                       # Save/load system
│   ├── Public/
│   │   ├── TIRSaveGame.h
│   │   ├── TIRSaveGameSubsystem.h
│   │   └── Serialization/
│   │       └── TIRSerializationHelpers.h # Pure C++ - testable
│   └── Private/
│       └── Tests/
│           └── SaveSystemTest.cpp
│
├── TIRGameFramework/                  # Game mode, game state, etc.
│   ├── Public/
│   │   ├── TIRGameMode.h
│   │   ├── TIRGameState.h
│   │   ├── TIRPlayerController.h
│   │   ├── TIRPlayerState.h
│   │   └── TIRGameInstance.h
│   └── Private/
│
├── TIROnline/                         # Online features (leaderboards - POST-LAUNCH)
│   ├── Public/
│   │   ├── Subsystems/
│   │   │   ├── TIRLeaderboardSubsystem.h
│   │   │   └── TIROnlineSubsystem.h
│   │   ├── Interfaces/
│   │   │   ├── ITIRLeaderboardAPI.h
│   │   │   └── ITIRAuthProvider.h
│   │   └── AntiCheat/
│   │       └── TIRScoreValidator.h    # Pure C++ - testable
│   └── Private/
│       └── Tests/
│           └── ScoreValidatorTest.cpp
│
└── TIRUtilities/                      # Shared utilities
    ├── Public/
    │   ├── TIRBlueprintFunctionLibrary.h
    │   ├── TIRMathLibrary.h           # Pure C++ - testable
    │   └── TIRDebugHelpers.h
    └── Private/
        └── Tests/
            └── MathLibraryTest.cpp
```

---

## Native Unreal Engine Components (Base)

### Principle: Extend, Don't Reinvent

The project **uses and extends** Unreal Engine's native components instead of recreating existing functionality. This ensures:
- ✅ Code tested and optimized by Epic Games
- ✅ Compatibility with editor tools
- ✅ Networking/Replication already implemented
- ✅ Blueprint support by default
- ✅ Less code to maintain

---

### Movement Components (TIRMovement depends on these)

#### 1. UFloatingPawnMovement
**Native component ideal for space shooter with 8-directional movement**

```cpp
// TIRMovement/Public/Components/TIRShipMovementComponent.h
#pragma once

#include "CoreMinimal.h"
#include "GameFramework/FloatingPawnMovement.h"
#include "TIRShipMovementComponent.generated.h"

/**
 * Extends UFloatingPawnMovement to add drift physics and zero-G
 * Base class already implements:
 * - Acceleration/Deceleration
 * - Max Speed
 * - Turning Boost
 * - Network Replication
 */
UCLASS(ClassGroup=(TIR), meta=(BlueprintSpawnableComponent))
class TIRMOVEMENT_API UTIRShipMovementComponent : public UFloatingPawnMovement
{
    GENERATED_BODY()

public:
    UTIRShipMovementComponent();

    virtual void TickComponent(float DeltaTime, enum ELevelTick TickType, 
                               FActorComponentTickFunction* ThisTickFunction) override;

    // Custom space physics
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Space Physics")
    float DriftMultiplier = 0.3f;
    
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Space Physics")
    bool bEnableZeroGravityPhysics = true;
    
    UPROPERTY(EditAnywhere, BlueprintReadWrite, Category = "Space Physics")
    float InertiaDamping = 0.95f;

protected:
    void ApplyDriftPhysics(float DeltaTime);
    void ApplyInertia(float DeltaTime);
    
private:
    FVector LastVelocity;
    FVector InertiaVelocity;
};
```

**Why use UFloatingPawnMovement?**
- ✅ Already has smooth acceleration/deceleration
- ✅ Supports omnidirectional movement (perfect for space shooter)
- ✅ No gravity by default
- ✅ Already optimized for networking
- ✅ Works with `AddMovementInput()`

**Alternatives considered:**
- ❌ `UCharacterMovementComponent` - too heavy, made for ground characters
- ❌ `UProjectileMovementComponent` - no input handling
- ❌ Custom from scratch - reinvent the wheel

---

#### 2. UPawnMovementComponent
**Base class of all movement components**

Our `UTIRShipMovementComponent` inherits indirectly via `UFloatingPawnMovement`:

```
UActorComponent
    └── UMovementComponent
        └── UNavMovementComponent (optional pathfinding)
            └── UPawnMovementComponent
                └── UFloatingPawnMovement
                    └── UTIRShipMovementComponent (ours)
```

**What we get for free:**
- `IsPawnControlled()` - check if pawn is being controlled
- `GetPawnOwner()` - access to the pawn
- `Velocity` - current velocity (replicated)
- `UpdatedComponent` - reference to the component being moved
- Automatic network replication

---

#### 3. Dash Component (Custom, but uses native systems)

```cpp
// TIRMovement/Public/Components/TIRDashComponent.h
#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "TIRDashComponent.generated.h"

/**
 * Dash component that works WITH UFloatingPawnMovement
 * Doesn't replace it, just modifies temporarily
 */
UCLASS(ClassGroup=(TIR), meta=(BlueprintSpawnableComponent))
class TIRMOVEMENT_API UTIRDashComponent : public UActorComponent
{
    GENERATED_BODY()

public:
    UTIRDashComponent();

    UFUNCTION(BlueprintCallable, Category = "Dash")
    bool CanDash() const;
    
    UFUNCTION(BlueprintCallable, Category = "Dash")
    void PerformDash(FVector Direction);

protected:
    virtual void BeginPlay() override;
    virtual void TickComponent(float DeltaTime, enum ELevelTick TickType,
                               FActorComponentTickFunction* ThisTickFunction) override;

    UPROPERTY(EditDefaultsOnly, Category = "Dash")
    float DashDistance = 1000.0f;
    
    UPROPERTY(EditDefaultsOnly, Category = "Dash")
    float DashDuration = 0.2f;
    
    UPROPERTY(EditDefaultsOnly, Category = "Dash")
    float DashCooldown = 1.0f;
    
private:
    UPROPERTY()
    UFloatingPawnMovement* MovementComponent;
    
    bool bIsDashing = false;
    float DashTimeRemaining = 0.0f;
    float CooldownRemaining = 0.0f;
    FVector DashDirection;
    float OriginalMaxSpeed;
};
```

---

### Input System (TIRInput depends on this)

#### Enhanced Input System (UE 5.1+)

**The project ALREADY USES Enhanced Input** (see `TheInvasionReforged.Build.cs` line 12):

```csharp
PublicDependencyModuleNames.AddRange(new string[] { 
    "Core", "CoreUObject", "Engine", "InputCore", "EnhancedInput" 
});
```

**Enhanced Input Architecture:**

```
Input Device → Input Action → Input Mapping Context → Enhanced Input Component → Game Logic
```

**Main components:**

1. **UInputAction** - Represents an action (e.g., "Move", "Fire", "Dash")
2. **UInputMappingContext** - Maps keys to actions
3. **UEnhancedInputComponent** - Component that processes input
4. **Input Modifiers** - Modify input before processing (deadzone, scaling)
5. **Input Triggers** - Define when input is activated (pressed, released, hold)

---

#### TIRInput - Enhanced Input Wrapper

```cpp
// TIRInput/Public/Components/TIREnhancedInputComponent.h
#pragma once

#include "CoreMinimal.h"
#include "EnhancedInputComponent.h"
#include "TIREnhancedInputComponent.generated.h"

/**
 * Wrapper of UEnhancedInputComponent with project-specific helpers
 */
UCLASS(ClassGroup=(TIR))
class TIRINPUT_API UTIREnhancedInputComponent : public UEnhancedInputComponent
{
    GENERATED_BODY()

public:
    // Helper template for binding with less boilerplate
    template<class UserClass, typename FuncType>
    void BindActionByTag(const FGameplayTag& InputTag, ETriggerEvent TriggerEvent, 
                         UserClass* Object, FuncType Func);
    
    // Support for dynamic input contexts
    void PushInputContext(UInputMappingContext* Context, int32 Priority = 0);
    void PopInputContext(UInputMappingContext* Context);
};
```

**Data Asset para Input Actions:**

```cpp
// TIRInput/Public/Data/TIRInputConfig.h
#pragma once

#include "CoreMinimal.h"
#include "Engine/DataAsset.h"
#include "GameplayTagContainer.h"
#include "TIRInputConfig.generated.h"

class UInputAction;

USTRUCT(BlueprintType)
struct FTIRInputAction
{
    GENERATED_BODY()
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    UInputAction* InputAction = nullptr;
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Meta = (Categories = "Input"))
    FGameplayTag InputTag;
};

/**
 * Data Asset that maps Input Actions to Gameplay Tags
 * Facilitates programmatic binding
 */
UCLASS(BlueprintType)
class TIRINPUT_API UTIRInputConfig : public UDataAsset
{
    GENERATED_BODY()

public:
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    TArray<FTIRInputAction> NativeInputActions;
    
    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly)
    TArray<FTIRInputAction> AbilityInputActions;
    
    const UInputAction* FindNativeInputActionForTag(const FGameplayTag& InputTag) const;
};
```

**Example usage in Player Controller:**

```cpp
// TIRGameFramework/Private/TIRPlayerController.cpp
void ATIRPlayerController::SetupInputComponent()
{
    Super::SetupInputComponent();
    
    // Cast to our custom component
    UTIREnhancedInputComponent* TIRInput = Cast<UTIREnhancedInputComponent>(InputComponent);
    if (!TIRInput)
    {
        return;
    }
    
    // Get Input Config from Game Instance or Project Settings
    const UTIRInputConfig* InputConfig = GetInputConfig();
    
    // Bind using Gameplay Tags (more flexible than hardcoded actions)
    const FTIRGameplayTags& GameplayTags = FTIRGameplayTags::Get();
    
    // Movement
    if (const UInputAction* MoveAction = InputConfig->FindNativeInputActionForTag(GameplayTags.Input_Move))
    {
        TIRInput->BindAction(MoveAction, ETriggerEvent::Triggered, this, &ATIRPlayerController::Input_Move);
    }
    
    // Dash
    if (const UInputAction* DashAction = InputConfig->FindNativeInputActionForTag(GameplayTags.Input_Dash))
    {
        TIRInput->BindAction(DashAction, ETriggerEvent::Started, this, &ATIRPlayerController::Input_Dash);
    }
    
    // Abilities
    if (const UInputAction* Ability1Action = InputConfig->FindNativeInputActionForTag(GameplayTags.Input_Ability_1))
    {
        TIRInput->BindAction(Ability1Action, ETriggerEvent::Started, this, &ATIRPlayerController::Input_Ability1);
    }
}

void ATIRPlayerController::Input_Move(const FInputActionValue& InputValue)
{
    const FVector2D MoveVector = InputValue.Get<FVector2D>();
    
    if (APawn* ControlledPawn = GetPawn())
    {
        // AddMovementInput já é parte do Pawn base class
        ControlledPawn->AddMovementInput(FVector::ForwardVector, MoveVector.Y);
        ControlledPawn->AddMovementInput(FVector::RightVector, MoveVector.X);
    }
}
```

---

### Input Mapping Context (Asset)

**Created in Editor (Content Browser):**

```
Content/Input/
├── IMC_Default.uasset              # Default Input Mapping Context
├── IMC_Hangar.uasset               # Context for hangar hub
├── IMC_Combat.uasset               # Context for combat
└── Actions/
    ├── IA_Move.uasset              # Input Action - Move (Vector2D)
    ├── IA_Dash.uasset              # Input Action - Dash (Button)
    ├── IA_Fire.uasset              # Input Action - Fire (Button)
    └── IA_Ability1.uasset          # Input Action - Singularity Ray (Button)
```

**Configuration in Editor:**
1. Right click → Input → Input Action
2. Define Value Type (Boolean, Axis1D, Axis2D, Axis3D)
3. Create Input Mapping Context
4. Map keys/buttons to actions
5. Add Input Modifiers (Negate, SwizzleAxis, DeadZone)
6. Add Input Triggers (Pressed, Released, Hold, Tap)

---

### Mobile Input (Touch)

```cpp
// TIRInput/Public/Processors/TIRMobileInputProcessor.h
#pragma once

#include "CoreMinimal.h"
#include "Framework/Application/IInputProcessor.h"

/**
 * Processes touch input and converts to Enhanced Input
 * Registered in Game Instance
 */
class TIRINPUT_API FTIRMobileInputProcessor : public IInputProcessor
{
public:
    FTIRMobileInputProcessor();
    
    // IInputProcessor interface
    virtual void Tick(const float DeltaTime, FSlateApplication& SlateApp, 
                     TSharedRef<ICursor> Cursor) override;
    
    virtual bool HandleTouchStartedEvent(const TSharedPtr<FSlateUser>& SlateUser,
                                        const FPointerEvent& PointerEvent) override;
    
    virtual bool HandleTouchMovedEvent(const TSharedPtr<FSlateUser>& SlateUser,
                                      const FPointerEvent& PointerEvent) override;
    
    virtual bool HandleTouchEndedEvent(const TSharedPtr<FSlateUser>& SlateUser,
                                      const FPointerEvent& PointerEvent) override;

private:
    // Virtual joystick for movement
    FVector2D JoystickPosition;
    FVector2D JoystickCenter;
    bool bIsMoving = false;
    
    // Touch ID tracking
    TMap<int32, FVector2D> ActiveTouches;
};
```

---

### Build Dependencies (Updated)

#### TIRMovement.Build.cs

```csharp
public class TIRMovement : ModuleRules
{
    public TIRMovement(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
        
        PublicDependencyModuleNames.AddRange(new string[] 
        {
            "Core",
            "CoreUObject",
            "Engine",
            "TIRCore"
            // UFloatingPawnMovement está em Engine
        });
        
        // No additional dependencies - everything comes from Engine module
    }
}
```

#### TIRInput.Build.cs

```csharp
public class TIRInput : ModuleRules
{
    public TIRInput(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
        
        PublicDependencyModuleNames.AddRange(new string[] 
        {
            "Core",
            "CoreUObject",
            "Engine",
            "InputCore",           // Input base classes
            "EnhancedInput",       // Enhanced Input System (UE 5.1+)
            "GameplayTags",        // Para tag-based input binding
            "TIRCore"
        });
        
        PrivateDependencyModuleNames.AddRange(new string[] 
        {
            "Slate",              // For mobile input processor
            "SlateCore"
        });
    }
}
```

---

### Gameplay Tags for Input (DefaultGameplayTags.ini)

```ini
; Config/DefaultGameplayTags.ini

[/Script/GameplayTags.GameplayTagsSettings]
+GameplayTagList=(Tag="Input.Move",DevComment="WASD/Left Stick - Movement")
+GameplayTagList=(Tag="Input.Dash",DevComment="Space/A Button - Dash ability")
+GameplayTagList=(Tag="Input.Fire",DevComment="Mouse/Right Trigger - Auto-fire (toggle)")
+GameplayTagList=(Tag="Input.Ability.1",DevComment="Q/X Button - Singularity Ray")
+GameplayTagList=(Tag="Input.Ability.2",DevComment="E/Y Button - Reflect Field")
+GameplayTagList=(Tag="Input.Ability.3",DevComment="R/B Button - Void Step")
+GameplayTagList=(Tag="Input.Pause",DevComment="ESC/Start - Pause menu")
```

---

### Official References

**Movement:**
- [UFloatingPawnMovement Documentation](https://dev.epicgames.com/documentation/en-us/unreal-engine/API/Runtime/Engine/UFloatingPawnMovement)
- [Movement Components Overview](https://dev.epicgames.com/documentation/en-us/unreal-engine/movement-components-in-unreal-engine)

**Enhanced Input:**
- [Enhanced Input System](https://dev.epicgames.com/documentation/en-us/unreal-engine/enhanced-input-in-unreal-engine)
- [Input Actions](https://dev.epicgames.com/documentation/en-us/unreal-engine/input-actions-in-unreal-engine)
- [Input Mapping Contexts](https://dev.epicgames.com/documentation/en-us/unreal-engine/enhanced-input-in-unreal-engine#inputmappingcontexts)
- [Input Modifiers and Triggers](https://dev.epicgames.com/documentation/en-us/unreal-engine/enhanced-input-action-modifiers-and-triggers-in-unreal-engine)

**Example Projects:**
- **Lyra Sample** - Uses Enhanced Input extensively
- **Ancient Game** - Similar input system

---

### Other Useful Native Components

#### UActorComponent Base Classes

All our custom components inherit from Unreal base classes:

| Our Class | Inherits From | Reason |
|--------------|----------|--------|
| `UTIRHealthComponent` | `UActorComponent` | Simple component without transform |
| `UTIRShipMovementComponent` | `UFloatingPawnMovement` | Movement with physics |
| `UTIRWeaponComponent` | `UActorComponent` | Weapon logic |
| `UTIREnhancedInputComponent` | `UEnhancedInputComponent` | Input processing |

**Why use UActorComponent?**
- ✅ Automatic lifecycle (BeginPlay, Tick, EndPlay)
- ✅ Replication support
- ✅ Serialization for save games
- ✅ Blueprint exposure
- ✅ Component tags and queries

---

#### USceneComponent vs UActorComponent

**UActorComponent** (without transform):
- Used for pure logic
- Examples: HealthComponent, WeaponSystemComponent, ExperienceComponent
- Lighter (no transform overhead)

**USceneComponent** (with transform):
- Used when position/rotation/scale is needed
- Examples: CameraComponent, SpringArmComponent, StaticMeshComponent
- Can have child components

```cpp
// DON'T need transform - use UActorComponent
UCLASS()
class UTIRHealthComponent : public UActorComponent { };

// NEED transform - use USceneComponent
UCLASS()
class UTIRWeaponMountPoint : public USceneComponent { };
```

---

#### Data Assets (Configuration)

Unreal provides **UDataAsset** as base for configuration:

```cpp
// Example: Enemy configuration
UCLASS(BlueprintType)
class UTIREnemyData : public UPrimaryDataAsset
{
    GENERATED_BODY()

public:
    UPROPERTY(EditDefaultsOnly, Category = "Stats")
    float MaxHealth = 100.0f;
    
    UPROPERTY(EditDefaultsOnly, Category = "Stats")
    float MovementSpeed = 300.0f;
    
    UPROPERTY(EditDefaultsOnly, Category = "Combat")
    float Damage = 10.0f;
    
    UPROPERTY(EditDefaultsOnly, Category = "AI")
    TObjectPtr<UBehaviorTree> BehaviorTree;
    
    UPROPERTY(EditDefaultsOnly, Category = "Visual")
    TObjectPtr<UStaticMesh> Mesh;
    
    // Primary Asset ID for Asset Manager
    virtual FPrimaryAssetId GetPrimaryAssetId() const override
    {
        return FPrimaryAssetId("EnemyData", GetFName());
    }
};
```

**Advantages of UDataAsset:**
- ✅ Editable in Content Browser
- ✅ Version control
- ✅ Designers can edit without touching code
- ✅ Supports Asset Manager (async loading)
- ✅ Blueprint-friendly

---

#### Subsystems (UE 5+)

**Game Instance Subsystem** - Persists throughout the entire session:

```cpp
// TIRInput/Public/Subsystems/TIRInputSubsystem.h
#pragma once

#include "CoreMinimal.h"
#include "Subsystems/GameInstanceSubsystem.h"
#include "TIRInputSubsystem.generated.h"

/**
 * Manages input contexts and global input settings
 * Persists between levels
 */
UCLASS()
class TIRINPUT_API UTIRInputSubsystem : public UGameInstanceSubsystem
{
    GENERATED_BODY()

public:
    virtual void Initialize(FSubsystemCollectionBase& Collection) override;
    virtual void Deinitialize() override;
    
    UFUNCTION(BlueprintCallable, Category = "Input")
    void SetInputContext(UInputMappingContext* Context, int32 Priority);
    
    UFUNCTION(BlueprintCallable, Category = "Input")
    void RemoveInputContext(UInputMappingContext* Context);

protected:
    UPROPERTY()
    TArray<UInputMappingContext*> ActiveContexts;
};
```

**Local Player Subsystem** - One per local player (useful for split-screen):

```cpp
UCLASS()
class UTIRPlayerInputSubsystem : public ULocalPlayerSubsystem
{
    GENERATED_BODY()
    // Input settings specific to each player
};
```

**World Subsystem** - One per world/level:

```cpp
// TIRSpawning/Public/Subsystems/TIRSpawningSubsystem.h
UCLASS()
class UTIRSpawningSubsystem : public UWorldSubsystem
{
    GENERATED_BODY()

public:
    UFUNCTION(BlueprintCallable)
    void StartWave(int32 WaveNumber);
    
    UFUNCTION(BlueprintCallable)
    AActor* SpawnEnemy(TSubclassOf<AActor> EnemyClass, FVector Location);
    
protected:
    UPROPERTY()
    TArray<UTIRObjectPool*> EnemyPools;
};
```

---

#### Gameplay Tasks (AIModule)

For complex asynchronous actions:

```cpp
// TIRAi/Public/Tasks/TIRGameplayTask_OrbitTarget.h
#pragma once

#include "CoreMinimal.h"
#include "GameplayTask.h"
#include "TIRGameplayTask_OrbitTarget.generated.h"

/**
 * Gameplay Task for enemy to orbit the player
 * Executes async, can be cancelled
 */
UCLASS()
class TIRAI_API UTIRGameplayTask_OrbitTarget : public UGameplayTask
{
    GENERATED_BODY()

public:
    UFUNCTION(BlueprintCallable, Category = "AI|Tasks",
              meta = (DefaultToSelf = "OwningController", BlueprintInternalUseOnly = "TRUE"))
    static UTIRGameplayTask_OrbitTarget* OrbitTarget(
        AAIController* OwningController,
        AActor* Target,
        float OrbitRadius = 500.0f,
        float OrbitSpeed = 300.0f);

protected:
    virtual void Activate() override;
    virtual void TickTask(float DeltaTime) override;
    virtual void OnDestroy(bool bInOwnerFinished) override;

    UPROPERTY()
    AAIController* MyController;
    
    UPROPERTY()
    AActor* TargetActor;
    
    float Radius;
    float Speed;
    float CurrentAngle;
};
```

**Used in Behavior Tree Task:**

```cpp
// BTTask_OrbitPlayer.cpp
EBTNodeResult::Type UBTTask_OrbitPlayer::ExecuteTask(UBehaviorTreeComponent& OwnerComp, uint8* NodeMemory)
{
    AAIController* Controller = OwnerComp.GetAIOwner();
    AActor* Target = GetTarget(OwnerComp);
    
    // Launch gameplay task async
    UTIRGameplayTask_OrbitTarget* OrbitTask = UTIRGameplayTask_OrbitTarget::OrbitTarget(
        Controller, Target, OrbitRadius, OrbitSpeed);
    
    OrbitTask->ReadyForActivation();
    
    return EBTNodeResult::InProgress;
}
```

---

#### Timers (FTimerManager)

**Don't use Tick for delays!** Use Timer Manager:

```cpp
// TIRCombat/Private/Components/TIRWeaponComponent.cpp
void UTIRWeaponComponent::Fire()
{
    if (!CanFire())
    {
        return;
    }
    
    // Execute shot
    SpawnProjectile();
    
    // Timer for cooldown (DON'T use Tick)
    FTimerHandle CooldownTimer;
    GetWorld()->GetTimerManager().SetTimer(
        CooldownTimer,
        this,
        &UTIRWeaponComponent::ResetCooldown,
        FireRate,
        false  // no loop
    );
}

void UTIRWeaponComponent::ResetCooldown()
{
    bIsOnCooldown = false;
}
```

**Timer with loop:**

```cpp
// Spawn enemies every 5 seconds
GetWorld()->GetTimerManager().SetTimer(
    SpawnTimer,
    this,
    &ATIRWaveManager::SpawnNextEnemy,
    5.0f,
    true  // loop
);
```

---

#### Object Pooling with UObject

**No need to create a complex system - use TArray:**

```cpp
// TIRSpawning/Public/Pooling/TIRObjectPool.h
UCLASS()
class UTIRObjectPool : public UObject
{
    GENERATED_BODY()

public:
    void Initialize(UWorld* World, TSubclassOf<AActor> Class, int32 PoolSize);
    
    AActor* Acquire();
    void Release(AActor* Actor);

protected:
    UPROPERTY()
    TArray<TObjectPtr<AActor>> PooledActors;
    
    UPROPERTY()
    TSubclassOf<AActor> ActorClass;
    
    UPROPERTY()
    UWorld* WorldContext;
    
    TQueue<AActor*> InactiveActors;
};
```

**Why UObject?**
- ✅ Automatic garbage collection
- ✅ UPROPERTY() prevents actors from being deleted
- ✅ Serialization for save games
- ✅ Blueprint callable

---

#### Niagara vs Cascade (VFX)

**Use Niagara (modern, UE 5+):**

```cpp
// TIRVfx/Public/Components/TIRVFXComponent.h
#include "NiagaraComponent.h"
#include "NiagaraFunctionLibrary.h"

void UTIRVFXComponent::SpawnExplosionEffect(FVector Location)
{
    if (ExplosionEffect)
    {
        UNiagaraFunctionLibrary::SpawnSystemAtLocation(
            GetWorld(),
            ExplosionEffect,
            Location,
            FRotator::ZeroRotator,
            FVector::OneVector,
            true,  // auto destroy
            true,  // auto activate
            ENCPoolMethod::AutoRelease  // automatic pooling!
        );
    }
}
```

**Niagara has built-in pooling!**
- ✅ `ENCPoolMethod::AutoRelease` - Niagara manages pool
- ✅ Better performance than Cascade
- ✅ GPU particles
- ✅ More features (ribbons, meshes, lights)

---

### Official Resources - Native Components

**Core Components:**
- [Actor Components](https://dev.epicgames.com/documentation/en-us/unreal-engine/components-in-unreal-engine)
- [Data Assets](https://dev.epicgames.com/documentation/en-us/unreal-engine/data-assets-in-unreal-engine)
- [Subsystems](https://dev.epicgames.com/documentation/en-us/unreal-engine/programming-subsystems-in-unreal-engine)

**AI:**
- [Behavior Trees](https://dev.epicgames.com/documentation/en-us/unreal-engine/behavior-tree-in-unreal-engine)
- [Gameplay Tasks](https://dev.epicgames.com/documentation/en-us/unreal-engine/gameplay-tasks-in-unreal-engine)

**VFX:**
- [Niagara System](https://dev.epicgames.com/documentation/en-us/unreal-engine/niagara-overview-for-unreal-engine)
- **Note:** Niagara pooling features (`ENCPoolMethod::AutoRelease`) available in UE 5.1+. Verify on target platform.

**Performance:**
- [Performance and Profiling](https://dev.epicgames.com/documentation/en-us/unreal-engine/performance-and-profiling-in-unreal-engine)

---

## Module Dependencies

### Dependency Diagram

```
                         ┌─────────────────────────────────┐
                         │   UNREAL ENGINE MODULES         │
                         │                                 │
                         │  • Engine (Core, UObject)       │
                         │  • GameplayTags                 │
                         │  • EnhancedInput                │
                         │  • UFloatingPawnMovement        │
                         │  • AIModule (BehaviorTree)      │
                         └─────────────────────────────────┘
                                      │
                                      ▼
                    TIRCore (Base - all depend on it)
                    • Interfaces (ITIRDamageable, etc)
                    • Gameplay Tags (FTIRGameplayTags)
                    • Structs & Enums
                         │
        ┌────────────────┼────────────────┐
        │                │                │
    TIRCombat      TIRMovement       TIRInput
    • Health       • extends          • wraps
    • Damage         UFloating          UEnhanced
    • Weapons        PawnMovement       InputComponent
        │                │                │
        └────────┬───────┴────────────────┘
                 │
            TIRProgression
            • XP System
            • Upgrades
                 │
        ┌────────┼────────┐
        │        │        │
    TIRSpawning TIRAi  TIRCollectables
    • Waves    • uses   • Pickups
    • Pooling    BT      • Scrap
        │        │        │
        └────────┼────────┘
                 │
          TIREnvironment
          • Arenas
          • Hazards
                 │
        ┌────────┼────────┐
        │        │        │
    TIRHangar  TIRVfx   TIRUi
        │        │        │
        └────────┼────────┘
                 │
         TIRGameFramework
         • Game Mode/State
         • Player Controller
                 │
            TIRSaveGame
                 │
            TIROnline (POST-LAUNCH)
```

### Dependency Rules

1. **TIRCore** doesn't depend on any module (base)
2. **TIRGameFramework** can depend on any module
3. **Domain modules** only depend on TIRCore and related modules
4. **Circular dependencies** are PROHIBITED
5. **Interfaces** in TIRCore for communication between modules
6. **ALWAYS prefer extending native Unreal components instead of creating from scratch**

---

## Checklist: Native vs Custom

### ✅ USE Native Components (Extend, Don't Reinvent)

| Sistema | Componente Nativo | Nossa Classe | Motivo |
|---------|-------------------|--------------|--------|
| **Movement** | `UFloatingPawnMovement` | `UTIRShipMovementComponent extends` | Já tem aceleração, networking, input |
| **Input** | `UEnhancedInputComponent` | `UTIREnhancedInputComponent extends` | Sistema moderno, remapeável |
| **AI Controller** | `AAIController` | `ATIREnemyAIController extends` | Blackboard, Behavior Tree integration |
| **BT Task** | `UBTTaskNode` | `UBTTask_OrbitPlayer extends` | Lifecycle, debugging tools |
| **BT Service** | `UBTService` | `UBTService_UpdateTarget extends` | Auto-tick, blackboard access |
| **Data Config** | `UPrimaryDataAsset` | `UTIREnemyData extends` | Asset Manager, async loading |
| **VFX** | `UNiagaraSystem` | Use direto | Pooling automático, GPU particles |
| **Audio** | `UAudioComponent` | Use direto | 3D spatialization, attenuation |
| **Subsystem** | `UGameInstanceSubsystem` | `UTIRInputSubsystem extends` | Managed lifecycle |

### ⚠️ CREATE Custom (Pure C++ - Testable)

| System | Custom Class | Reason |
|---------|--------------|--------|
| **Damage Calculation** | `FTIRDamageCalculator` | Pure logic, unit testable |
| **XP System Logic** | `FTIRExperienceCalculator` | Pure logic, unit testable |
| **AI Strategy** | `FTIRAIStrategy` | Pure logic, unit testable |
| **Stats Modifiers** | `FTIRStatModifierSystem` | Pure logic, unit testable |
| **Wave Pattern** | `FTIRWavePattern` | Pure logic, unit testable |

**Golden rule:**
- **Business logic** → Pure C++ (struct/class without UCLASS)
- **Unreal integration** → Extends native component

### 🔄 WRAP Native Components (Add Features)

| Sistema | Nativo Base | Nossa Wrapper | Feature Adicionada |
|---------|-------------|---------------|-------------------|
| `UTIRHealthComponent` | `UActorComponent` | Extends | Damage events, shield system |
| `UTIRWeaponComponent` | `UActorComponent` | Extends | Auto-targeting, projectile pooling |
| `UTIRObjectPool` | `UObject` | Extends | Generic pooling for any actor |
| `UTIREnhancedInputComponent` | `UEnhancedInputComponent` | Extends | Tag-based binding helpers |

---

## Build Configuration (.Build.cs)

### ⚠️ IMPORTANT: Main Module Already Exists

The project already has the main module `TheInvasionReforged` configured with:

```csharp
// Source/TheInvasionReforged/TheInvasionReforged.Build.cs (EXISTING)
public class TheInvasionReforged : ModuleRules
{
    public TheInvasionReforged(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
    
        PublicDependencyModuleNames.AddRange(new string[] { 
            "Core", 
            "CoreUObject", 
            "Engine", 
            "InputCore", 
            "EnhancedInput"   // ✅ ALREADY CONFIGURED!
        });

        PrivateDependencyModuleNames.AddRange(new string[] {  });
    }
}
```

**Current status:**
- ✅ Enhanced Input is already in the project
- ⚠️ Code has not yet been migrated to separate modules
- 📁 `Locomotion/Components/` folder exists but is empty

**Migration plan:**
1. Create new modules (TIRCore, TIRMovement, TIRInput, etc)
2. Implement functionality in the new modules
3. Gradually move code from the main module
4. Keep `TheInvasionReforged` as "legacy" module until migration is complete

---

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
        
        // Sem dependências de outros módulos do jogo
    }
}
```

### TIRCombat.Build.cs

```csharp
public class TIRCombat : ModuleRules
{
    public TIRCombat(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
        
        PublicDependencyModuleNames.AddRange(new string[] 
        {
            "Core",
            "CoreUObject",
            "Engine",
            "TIRCore"
        });
        
        PrivateDependencyModuleNames.AddRange(new string[] 
        {
            "TIRVfx"  // Para efeitos visuais de dano
        });
    }
}
```

#### TIRAi.Build.cs

```csharp
public class TIRAi : ModuleRules
{
    public TIRAi(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
        
        PublicDependencyModuleNames.AddRange(new string[] 
        {
            "Core",
            "CoreUObject",
            "Engine",
            "AIModule",           // Behavior Tree, AAIController, etc
            "GameplayTasks",      // Para async tasks (movement, etc)
            "TIRCore",
            "TIRMovement"         // Para acessar movimento dos inimigos
        });
        
        PrivateDependencyModuleNames.AddRange(new string[] 
        {
            "TIRCombat"          // Para AI atacar/tomar dano
        });
    }
}
```

#### TIRGameFramework.Build.cs

```csharp
public class TIRGameFramework : ModuleRules
{
    public TIRGameFramework(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = PCHUsageMode.UseExplicitOrSharedPCHs;
        
        PublicDependencyModuleNames.AddRange(new string[] 
        {
            "Core",
            "CoreUObject",
            "Engine",
            "TIRCore",
            "TIRCombat",
            "TIRMovement",
            "TIRProgression",
            "TIRSpawning",
            "TIRAi",
            "TIREnvironment",
            "TIRUi",
            "TIRSaveGame"
        });
        
        PrivateDependencyModuleNames.AddRange(new string[] 
        {
            "InputCore",
            "EnhancedInput",
            "TIRInput"
        });
    }
}
```

---

## Code Patterns by Module

### TIRCore - Interfaces

```cpp
// TIRCore/Public/Interfaces/ITIRDamageable.h
#pragma once

#include "CoreMinimal.h"
#include "UObject/Interface.h"
#include "ITIRDamageable.generated.h"

UINTERFACE(MinimalAPI, Blueprintable)
class UTIRDamageable : public UInterface
{
    GENERATED_BODY()
};

class TIRCORE_API ITIRDamageable
{
    GENERATED_BODY()

public:
    UFUNCTION(BlueprintNativeEvent, Category = "Combat")
    void TakeDamage(float Amount, AActor* DamageDealer);
    
    UFUNCTION(BlueprintNativeEvent, Category = "Combat")
    float GetCurrentHealth() const;
    
    UFUNCTION(BlueprintNativeEvent, Category = "Combat")
    bool IsDead() const;
};
```

### TIRCore - Gameplay Tags

```cpp
// TIRCore/Public/Types/TIRGameplayTags.h
#pragma once

#include "CoreMinimal.h"
#include "GameplayTagContainer.h"

/**
 * Singleton to manage project Gameplay Tags
 */
struct TIRCORE_API FTIRGameplayTags
{
public:
    static const FTIRGameplayTags& Get() 
    { 
        static FTIRGameplayTags GameplayTags; 
        return GameplayTags; 
    }
    
    // Weapon Tags
    FGameplayTag Weapon_Primary_PlasmaBeam;
    FGameplayTag Weapon_Primary_OrbitalSentinel;
    FGameplayTag Weapon_Primary_IonCannon;
    
    // Ability Tags
    FGameplayTag Ability_SingularityRay;
    FGameplayTag Ability_ReflectField;
    FGameplayTag Ability_VoidStep;
    
    // Upgrade Tags
    FGameplayTag Upgrade_Hull_Reinforced;
    FGameplayTag Upgrade_Hull_Regenerative;
    FGameplayTag Upgrade_Weapon_Damage;
    FGameplayTag Upgrade_Weapon_FireRate;
    
    // State Tags
    FGameplayTag State_Invulnerable;
    FGameplayTag State_Stunned;
    FGameplayTag State_Dashing;
    
protected:
    FTIRGameplayTags();
    void InitializeNativeTags();
};
```

### TIRCombat - Health Component

```cpp
// TIRCombat/Public/Components/TIRHealthComponent.h
#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "Interfaces/ITIRDamageable.h"
#include "TIRHealthComponent.generated.h"

DECLARE_DYNAMIC_MULTICAST_DELEGATE_TwoParams(FOnHealthChanged, float, CurrentHealth, float, MaxHealth);
DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FOnDeath, AActor*, Killer);

UCLASS(ClassGroup=(TIR), meta=(BlueprintSpawnableComponent))
class TIRCOMBAT_API UTIRHealthComponent : public UActorComponent, public ITIRDamageable
{
    GENERATED_BODY()

public:
    UTIRHealthComponent();

    // ITIRDamageable interface
    virtual void TakeDamage_Implementation(float Amount, AActor* DamageDealer) override;
    virtual float GetCurrentHealth_Implementation() const override { return CurrentHealth; }
    virtual bool IsDead_Implementation() const override { return CurrentHealth <= 0.0f; }

    // Health management
    UFUNCTION(BlueprintCallable, Category = "Health")
    void Heal(float Amount);
    
    UFUNCTION(BlueprintCallable, Category = "Health")
    void SetMaxHealth(float NewMaxHealth);
    
    UFUNCTION(BlueprintPure, Category = "Health")
    float GetMaxHealth() const { return MaxHealth; }
    
    UFUNCTION(BlueprintPure, Category = "Health")
    float GetHealthPercentage() const { return CurrentHealth / MaxHealth; }

    // Events
    UPROPERTY(BlueprintAssignable, Category = "Health")
    FOnHealthChanged OnHealthChanged;
    
    UPROPERTY(BlueprintAssignable, Category = "Health")
    FOnDeath OnDeath;

protected:
    virtual void BeginPlay() override;

    UPROPERTY(EditDefaultsOnly, BlueprintReadOnly, Category = "Health")
    float MaxHealth = 100.0f;
    
    UPROPERTY(VisibleAnywhere, BlueprintReadOnly, Category = "Health")
    float CurrentHealth;
    
    UPROPERTY(EditDefaultsOnly, Category = "Health")
    bool bInvulnerable = false;
};
```

### TIRCombat - Damage Calculator (Pure C++ - Testable)

```cpp
// TIRCombat/Public/Damage/TIRDamageCalculator.h
#pragma once

#include "CoreMinimal.h"

/**
 * Pure C++ class for damage calculations
 * Fully testable without UObject dependency
 */
class TIRCOMBAT_API FTIRDamageCalculator
{
public:
    struct FDamageParams
    {
        float BaseDamage = 0.0f;
        float DamageMultiplier = 1.0f;
        float ArmorReduction = 0.0f;
        float CriticalChance = 0.0f;
        float CriticalMultiplier = 2.0f;
    };
    
    struct FDamageResult
    {
        float FinalDamage = 0.0f;
        bool bWasCritical = false;
    };
    
    static FDamageResult CalculateDamage(const FDamageParams& Params);
    static float ApplyArmor(float IncomingDamage, float ArmorValue);
    static bool RollCritical(float CritChance);
};
```

```cpp
// TIRCombat/Private/Damage/TIRDamageCalculator.cpp
#include "Damage/TIRDamageCalculator.h"
#include "Math/UnrealMathUtility.h"

FTIRDamageCalculator::FDamageResult FTIRDamageCalculator::CalculateDamage(const FDamageParams& Params)
{
    FDamageResult Result;
    
    // Base damage with multiplier
    float Damage = Params.BaseDamage * Params.DamageMultiplier;
    
    // Critical check
    Result.bWasCritical = RollCritical(Params.CriticalChance);
    if (Result.bWasCritical)
    {
        Damage *= Params.CriticalMultiplier;
    }
    
    // Apply armor
    Damage = ApplyArmor(Damage, Params.ArmorReduction);
    
    Result.FinalDamage = FMath::Max(0.0f, Damage);
    return Result;
}

float FTIRDamageCalculator::ApplyArmor(float IncomingDamage, float ArmorValue)
{
    // Formula: Damage * (100 / (100 + Armor))
    float Reduction = 100.0f / (100.0f + ArmorValue);
    return IncomingDamage * Reduction;
}

bool FTIRDamageCalculator::RollCritical(float CritChance)
{
    return FMath::RandRange(0.0f, 100.0f) <= CritChance;
}
```

### TIRCombat - Unit Test

```cpp
// TIRCombat/Private/Tests/DamageCalculatorTest.cpp
#include "Misc/AutomationTest.h"
#include "Damage/TIRDamageCalculator.h"

IMPLEMENT_SIMPLE_AUTOMATION_TEST(
    FDamageCalculatorBasicTest,
    "TheInvasionReforged.Combat.DamageCalculator.BasicDamage",
    EAutomationTestFlags::ApplicationContextMask | EAutomationTestFlags::ProductFilter
)

bool FDamageCalculatorBasicTest::RunTest(const FString& Parameters)
{
    // Arrange
    FTIRDamageCalculator::FDamageParams Params;
    Params.BaseDamage = 100.0f;
    Params.DamageMultiplier = 1.5f;
    Params.ArmorReduction = 0.0f;
    Params.CriticalChance = 0.0f;
    
    // Act
    auto Result = FTIRDamageCalculator::CalculateDamage(Params);
    
    // Assert
    TestEqual("Damage should be 150", Result.FinalDamage, 150.0f);
    TestFalse("Should not be critical", Result.bWasCritical);
    
    return true;
}

IMPLEMENT_SIMPLE_AUTOMATION_TEST(
    FDamageCalculatorArmorTest,
    "TheInvasionReforged.Combat.DamageCalculator.ArmorReduction",
    EAutomationTestFlags::ApplicationContextMask | EAutomationTestFlags::ProductFilter
)

bool FDamageCalculatorArmorTest::RunTest(const FString& Parameters)
{
    // Arrange
    FTIRDamageCalculator::FDamageParams Params;
    Params.BaseDamage = 100.0f;
    Params.DamageMultiplier = 1.0f;
    Params.ArmorReduction = 50.0f; // 50% reduction
    Params.CriticalChance = 0.0f;
    
    // Act
    auto Result = FTIRDamageCalculator::CalculateDamage(Params);
    
    // Assert - With 50 armor, should reduce to ~66.67 (100 / (100 + 50) * 100)
    TestEqual("Armor should reduce damage", Result.FinalDamage, 66.67f, 0.1f);
    
    return true;
}
```

### TIRProgression - Experience Component

```cpp
// TIRProgression/Public/Components/TIRExperienceComponent.h
#pragma once

#include "CoreMinimal.h"
#include "Components/ActorComponent.h"
#include "TIRExperienceComponent.generated.h"

DECLARE_DYNAMIC_MULTICAST_DELEGATE_OneParam(FOnLevelUp, int32, NewLevel);
DECLARE_DYNAMIC_MULTICAST_DELEGATE_ThreeParams(FOnExperienceGained, float, CurrentXP, float, RequiredXP, int32, Level);

UCLASS(ClassGroup=(TIR), meta=(BlueprintSpawnableComponent))
class TIRPROGRESSION_API UTIRExperienceComponent : public UActorComponent
{
    GENERATED_BODY()

public:
    UTIRExperienceComponent();

    UFUNCTION(BlueprintCallable, Category = "Progression")
    void AddExperience(float Amount);
    
    UFUNCTION(BlueprintPure, Category = "Progression")
    int32 GetCurrentLevel() const { return CurrentLevel; }
    
    UFUNCTION(BlueprintPure, Category = "Progression")
    float GetCurrentXP() const { return CurrentXP; }
    
    UFUNCTION(BlueprintPure, Category = "Progression")
    float GetRequiredXP() const { return RequiredXP; }
    
    UFUNCTION(BlueprintPure, Category = "Progression")
    float GetXPPercentage() const { return CurrentXP / RequiredXP; }

    UPROPERTY(BlueprintAssignable, Category = "Progression")
    FOnLevelUp OnLevelUp;
    
    UPROPERTY(BlueprintAssignable, Category = "Progression")
    FOnExperienceGained OnExperienceGained;

protected:
    virtual void BeginPlay() override;
    
    void CheckLevelUp();
    float CalculateRequiredXP(int32 Level) const;

    UPROPERTY(EditDefaultsOnly, Category = "Progression")
    float BaseXPRequired = 100.0f;
    
    UPROPERTY(EditDefaultsOnly, Category = "Progression")
    float XPScalingFactor = 1.2f;
    
    UPROPERTY(VisibleAnywhere, Category = "Progression")
    int32 CurrentLevel = 1;
    
    UPROPERTY(VisibleAnywhere, Category = "Progression")
    float CurrentXP = 0.0f;
    
    UPROPERTY(VisibleAnywhere, Category = "Progression")
    float RequiredXP = 100.0f;
};
```

### TIRSpawning - Object Pool

```cpp
// TIRSpawning/Public/Pooling/TIRObjectPool.h
#pragma once

#include "CoreMinimal.h"
#include "UObject/NoExportTypes.h"
#include "Interfaces/ITIRPoolable.h"
#include "TIRObjectPool.generated.h"

UCLASS()
class TIRSPAWNING_API UTIRObjectPool : public UObject
{
    GENERATED_BODY()

public:
    void Initialize(UWorld* World, TSubclassOf<AActor> ActorClass, int32 InitialSize);
    
    UFUNCTION(BlueprintCallable, Category = "ObjectPool")
    AActor* GetPooledActor();
    
    UFUNCTION(BlueprintCallable, Category = "ObjectPool")
    void ReturnToPool(AActor* Actor);
    
    UFUNCTION(BlueprintPure, Category = "ObjectPool")
    int32 GetActiveCount() const { return ActiveActors.Num(); }
    
    UFUNCTION(BlueprintPure, Category = "ObjectPool")
    int32 GetInactiveCount() const { return InactiveActors.Num(); }

protected:
    AActor* CreateNewActor();

    UPROPERTY()
    TArray<AActor*> ActiveActors;
    
    UPROPERTY()
    TArray<AActor*> InactiveActors;
    
    UPROPERTY()
    TSubclassOf<AActor> PooledClass;
    
    UPROPERTY()
    UWorld* WorldContext;
};
```

---

## GAS (Gameplay Ability System) Preparation

### When to Add GAS?

**Recommendation:** Implement GAS in **Week 17-19** (Post-Launch Update 2) when adding:
- Boss Phase 3 (complex mechanics)
- New enemies with special abilities
- Status effects system (stun, slow, etc)

### Structure with GAS

```
TIRAbilitySystem/                      # Novo módulo GAS
├── Public/
│   ├── Attributes/
│   │   ├── TIRAttributeSet.h          # Health, Damage, Speed, etc
│   │   └── TIRCombatAttributeSet.h
│   ├── Abilities/
│   │   ├── TIRGameplayAbility.h       # Base class
│   │   ├── TIRAbility_Dash.h
│   │   └── TIRAbility_SingularityRay.h
│   ├── Effects/
│   │   ├── TIRGameplayEffect.h        # Base class
│   │   └── TIRGameplayEffectData.h
│   └── Components/
│       └── TIRAbilitySystemComponent.h
```

### Migration to GAS

```cpp
// Before (Current)
UTIRHealthComponent* HealthComp;
HealthComp->TakeDamage(10.0f, Attacker);

// After (With GAS)
UTIRAbilitySystemComponent* ASC;
FGameplayEffectSpecHandle DamageSpec = MakeDamageEffect(10.0f);
ASC->ApplyGameplayEffectSpecToSelf(*DamageSpec.Data.Get());
```

---

## Multiplayer Preparation

### Multiplayer Principles

1. **Server Authority**: All important decisions on the server
2. **Client Prediction**: Movement and instant actions on the client
3. **Replication**: Synchronize critical state
4. **RPC**: Client-server communication

### Multiplayer-Ready Components

```cpp
// TIRCombat/Public/Components/TIRHealthComponent.h
UCLASS(ClassGroup=(TIR), meta=(BlueprintSpawnableComponent))
class TIRCOMBAT_API UTIRHealthComponent : public UActorComponent
{
    GENERATED_BODY()

public:
    // Replication setup
    virtual void GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const override;

    // Server RPC to apply damage
    UFUNCTION(Server, Reliable, WithValidation)
    void ServerTakeDamage(float Amount, AActor* DamageDealer);

protected:
    // Replicated health
    UPROPERTY(ReplicatedUsing = OnRep_CurrentHealth)
    float CurrentHealth;
    
    UFUNCTION()
    void OnRep_CurrentHealth();
    
    // Replicated max health
    UPROPERTY(Replicated)
    float MaxHealth;
};
```

```cpp
// TIRCombat/Private/Components/TIRHealthComponent.cpp
void UTIRHealthComponent::GetLifetimeReplicatedProps(TArray<FLifetimeProperty>& OutLifetimeProps) const
{
    Super::GetLifetimeReplicatedProps(OutLifetimeProps);
    
    DOREPLIFETIME(UTIRHealthComponent, CurrentHealth);
    DOREPLIFETIME(UTIRHealthComponent, MaxHealth);
}

void UTIRHealthComponent::ServerTakeDamage_Implementation(float Amount, AActor* DamageDealer)
{
    // Server-side validation
    if (!GetOwner()->HasAuthority())
    {
        return;
    }
    
    // Apply damage
    CurrentHealth = FMath::Max(0.0f, CurrentHealth - Amount);
    
    // Broadcast events (will replicate via OnRep)
    OnHealthChanged.Broadcast(CurrentHealth, MaxHealth);
    
    if (CurrentHealth <= 0.0f)
    {
        OnDeath.Broadcast(DamageDealer);
    }
}

bool UTIRHealthComponent::ServerTakeDamage_Validate(float Amount, AActor* DamageDealer)
{
    // Validation: reasonable damage amount
    return Amount >= 0.0f && Amount < 10000.0f;
}

void UTIRHealthComponent::OnRep_CurrentHealth()
{
    // Client-side response to health change
    OnHealthChanged.Broadcast(CurrentHealth, MaxHealth);
}
```

---

## Test Configuration

### DefaultEngine.ini

```ini
[/Script/AutomationController.AutomationControllerSettings]
bSuppressLogErrors=False
bSuppressLogWarnings=False
bTreatLogWarningsAsTestErrors=False

[/Script/Engine.AutomationTestSettings]
AutomationTestmap=/Game/Tests/TestMaps/EmptyTestMap
```

### Run Tests

```powershell
# Via Editor
Session Frontend > Automation > Run Tests

# Via Command Line
UnrealEditor.exe "D:\GameProjects\TheInvasionReforged\TheInvasionReforged.uproject" -ExecCmds="Automation RunTests TheInvasionReforged" -unattended -nopause -testexit="Automation Test Queue Empty"
```

---

## Implementation Checklist

### Phase 1: Core Modules (Week 7-8)
- [ ] TIRCore - Interfaces, types, tags
- [ ] TIRCombat - Health, damage, weapons (basic)
- [ ] TIRMovement - Ship movement, drift physics
- [ ] TIRInput - Enhanced Input setup
- [ ] Unit tests for pure C++ classes

### Phase 2: Gameplay Modules (Week 9-10)
- [ ] TIRProgression - XP system, upgrade component
- [ ] TIRSpawning - Wave manager, object pooling
- [ ] TIRAi - Basic behavior trees
- [ ] TIRCollectables - Scrap pickup
- [ ] TIREnvironment - Arena setup

### Phase 3: Meta & Polish (Week 11-12)
- [ ] TIRHangar - Hangar hub, terminals
- [ ] TIRUi - HUD, menus, upgrade selection
- [ ] TIRVfx - Visual effects pooling
- [ ] TIRSaveGame - Save/load system
- [ ] TIRGameFramework - Game mode, game state

### Phase 4: Post-Launch (Week 17+)
- [ ] TIRAbilitySystem - Migrate to GAS
- [ ] TIROnline - Leaderboards
- [ ] Multiplayer setup (optional)

---

## Best Practices

### 1. Naming Conventions
- **Modules**: `TIR` + `DomainName` (e.g., TIRCombat)
- **Classes**: `UTIR` + `ClassName` (e.g., UTIRHealthComponent)
- **Interfaces**: `ITIR` + `InterfaceName` (e.g., ITIRDamageable)
- **Structs**: `FTIR` + `StructName` (e.g., FTIRDamageParams)
- **Enums**: `ETIR` + `EnumName` (e.g., ETIRWeaponType)

### 2. Header Organization
```cpp
#pragma once

// Engine includes
#include "CoreMinimal.h"
#include "Components/ActorComponent.h"

// Project includes
#include "Interfaces/ITIRDamageable.h"

// Generated include (always last)
#include "TIRHealthComponent.generated.h"
```

### 3. Forward Declarations
```cpp
// Header (.h) - Use forward declarations
class UTIRWeaponData;
class ATIRProjectile;

// Implementation (.cpp) - Include headers
#include "Weapons/TIRWeaponData.h"
#include "Weapons/TIRProjectile.h"
```

### 4. API Macros
```cpp
// Public API (exposed to other modules)
class TIRCOMBAT_API UTIRHealthComponent : public UActorComponent
{
    // ...
};

// Private implementation (not exposed)
class FTIRDamageCalculatorImpl
{
    // No API macro
};
```

### 5. Logging
```cpp
// TIRCore/Public/Utils/TIRLogCategories.h
DECLARE_LOG_CATEGORY_EXTERN(LogTIRCombat, Log, All);
DECLARE_LOG_CATEGORY_EXTERN(LogTIRMovement, Log, All);

// Usage
UE_LOG(LogTIRCombat, Warning, TEXT("Health component took %f damage"), DamageAmount);
```

---

## Resources and References

### Official Unreal Documentation
- [Gameplay Ability System](https://dev.epicgames.com/documentation/en-us/unreal-engine/gameplay-ability-system-for-unreal-engine)
- [Networking and Multiplayer](https://dev.epicgames.com/documentation/en-us/unreal-engine/networking-and-multiplayer-in-unreal-engine)
- [Automation System](https://dev.epicgames.com/documentation/en-us/unreal-engine/automation-system-overview-for-unreal-engine)
- [Automation Technical Guide](https://dev.epicgames.com/documentation/en-us/unreal-engine/automation-technical-guide-for-unreal-engine)
- [Plugin Development](https://dev.epicgames.com/documentation/en-us/unreal-engine/plugins-in-unreal-engine)

### Modular Project Examples
- Lyra Sample Game (Epic Games)
- Action RPG Sample (Epic Games)
- ShooterGame Sample (Epic Games)

### Testing
- [Gauntlet Automation Framework](https://dev.epicgames.com/documentation/en-us/unreal-engine/gauntlet-automation-framework-in-unreal-engine)

---

## Related Documentation

See also:
- [Module Details](./Modules/) - Individual module documentation
- [Intermodule Dependencies](./INTERMODULE_DEPENDENCIES.md) - Detailed dependency graph and rules

---

**Last Updated:** November 17, 2025  
**Document Version:** 1.1  
**Author:** Anderson (Lead Developer)
