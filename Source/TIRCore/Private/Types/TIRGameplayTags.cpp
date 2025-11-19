// Source/TIRCore/Private/Types/TIRGameplayTags.cpp
#include "Types/TIRGameplayTags.h"
#include "GameplayTagsManager.h"
#include "Utils/TIRLogCategories.h"

FTirGameplayTags FTirGameplayTags::GameplayTags;

void FTirGameplayTags::InitializeNativeTags()
{
    UGameplayTagsManager& Manager = UGameplayTagsManager::Get();
    GameplayTags.AddAllTags(Manager);
    UE_LOG(LogTIRCore, Log, TEXT("Native Gameplay Tags Initialized"));
}

void FTirGameplayTags::AddAllTags(UGameplayTagsManager& Manager)
{
    // Helper macro for adding tags
    #define ADD_TAG(TagVariable, TagName, TagComment) \
        TagVariable = Manager.AddNativeGameplayTag(FName(TagName), FString(TEXT(TagComment)));

    // Input
    ADD_TAG(Input_Move, "Input.Move", "Movement input (WASD/Left Stick)");
    ADD_TAG(Input_Fire, "Input.Fire", "Fire weapon input");
    ADD_TAG(Input_Dash, "Input.Dash", "Dash ability input");
    ADD_TAG(Input_Ability_1, "Input.Ability.1", "Primary ability input");
    ADD_TAG(Input_Ability_2, "Input.Ability.2", "Secondary ability input");
    ADD_TAG(Input_Ability_3, "Input.Ability.3", "Ultimate/Utility ability input");

    // Weapons
    ADD_TAG(Weapon_Type_PlasmaBeam, "Weapon.Type.PlasmaBeam", "Standard rapid-fire plasma weapon");
    ADD_TAG(Weapon_Type_IonCannon, "Weapon.Type.IonCannon", "Heavy charged ion weapon");
    ADD_TAG(Weapon_State_Firing, "Weapon.State.Firing", "Weapon is currently firing");
    ADD_TAG(Weapon_State_Reloading, "Weapon.State.Reloading", "Weapon is cooling down or reloading");

    // State
    ADD_TAG(State_Dead, "State.Dead", "Actor is dead");
    ADD_TAG(State_Invulnerable, "State.Invulnerable", "Actor cannot take damage");
    ADD_TAG(State_Stunned, "State.Stunned", "Actor cannot move or act");

    // Attributes
    ADD_TAG(Attribute_Health, "Attribute.Health", "Current health value");
    ADD_TAG(Attribute_MaxHealth, "Attribute.MaxHealth", "Maximum health capacity");
    ADD_TAG(Attribute_MovementSpeed, "Attribute.MovementSpeed", "Movement speed modifier");

    #undef ADD_TAG
}