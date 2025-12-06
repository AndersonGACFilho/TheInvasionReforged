#include "Types/TIRGameplayTags.h"
#include "GameplayTagsManager.h"
#include "Utils/TIRLogCategories.h"

FTIRGameplayTags FTIRGameplayTags::GameplayTags;

void FTIRGameplayTags::InitializeNativeTags()
{
	UGameplayTagsManager& Manager = UGameplayTagsManager::Get();
	GameplayTags.AddAllTags(Manager);
	UE_LOG(LogTIRCore, Log, TEXT("Native Gameplay Tags Initialized"));
}

void FTIRGameplayTags::AddAllTags(UGameplayTagsManager& Manager)
{
	/**
	 * Helper macro for registering native gameplay tags.
	 *
	 * @param TagVariable The FGameplayTag member to assign.
	 * @param TagName     The hierarchical tag name (e.g., "Input.Move").
	 * @param TagComment  Description shown in the editor and used for documentation.
	 */
	#define ADD_TAG(TagVariable, TagName, TagComment) \
		TagVariable = Manager.AddNativeGameplayTag(FName(TagName), FString(TEXT(TagComment)));

	// ─────────────────────────────────────────────────────────────────────────
	// Parent Tags (for hierarchical queries)
	// ─────────────────────────────────────────────────────────────────────────
	ADD_TAG(Input, "Input", "Parent tag for all input actions");
	ADD_TAG(Weapon, "Weapon", "Parent tag for all weapon-related tags");
	ADD_TAG(Weapon_Type, "Weapon.Type", "Parent tag for weapon type classification");
	ADD_TAG(Weapon_State, "Weapon.State", "Parent tag for weapon runtime states");
	ADD_TAG(State, "State", "Parent tag for actor states");
	ADD_TAG(Attribute, "Attribute", "Parent tag for GAS attributes");
	ADD_TAG(Damage, "Damage", "Parent tag for damage modifiers");
	ADD_TAG(Heal, "Heal", "Parent tag for healing modifiers");

	// ─────────────────────────────────────────────────────────────────────────
	// Input Tags
	// ─────────────────────────────────────────────────────────────────────────
	ADD_TAG(Input_Move, "Input.Move", "Movement input (WASD/Left Stick)");
	ADD_TAG(Input_Fire, "Input.Fire", "Fire weapon input");
	ADD_TAG(Input_Dash, "Input.Dash", "Dash ability input");
	ADD_TAG(Input_Ability_1, "Input.Ability.1", "Primary ability input");
	ADD_TAG(Input_Ability_2, "Input.Ability.2", "Secondary ability input");
	ADD_TAG(Input_Ability_3, "Input.Ability.3", "Ultimate/Utility ability input");

	// ─────────────────────────────────────────────────────────────────────────
	// Weapon Tags
	// ─────────────────────────────────────────────────────────────────────────
	ADD_TAG(Weapon_Type_PlasmaBeam, "Weapon.Type.PlasmaBeam", "Standard rapid-fire plasma weapon");
	ADD_TAG(Weapon_Type_IonCannon, "Weapon.Type.IonCannon", "Heavy charged ion weapon");
	ADD_TAG(Weapon_State_Firing, "Weapon.State.Firing", "Weapon is currently firing");
	ADD_TAG(Weapon_State_Reloading, "Weapon.State.Reloading", "Weapon is cooling down or reloading");

	// ─────────────────────────────────────────────────────────────────────────
	// State Tags
	// ─────────────────────────────────────────────────────────────────────────
	ADD_TAG(State_Dead, "State.Dead", "Actor is dead");
	ADD_TAG(State_Invulnerable, "State.Invulnerable", "Actor cannot take damage");
	ADD_TAG(State_Stunned, "State.Stunned", "Actor cannot move or act");

	// ─────────────────────────────────────────────────────────────────────────
	// Attribute Tags
	// ─────────────────────────────────────────────────────────────────────────
	ADD_TAG(Attribute_Health, "Attribute.Health", "Current health value");
	ADD_TAG(Attribute_MaxHealth, "Attribute.MaxHealth", "Maximum health capacity");
	ADD_TAG(Attribute_MovementSpeed, "Attribute.MovementSpeed", "Movement speed modifier");

	// ─────────────────────────────────────────────────────────────────────────
	// Damage Tags
	// ─────────────────────────────────────────────────────────────────────────
	ADD_TAG(Damage_Critical, "Damage.Critical", "Critical hit damage modifier");
	ADD_TAG(Damage_DOT, "Damage.DOT", "Damage over time effect");
	ADD_TAG(Damage_Backstab, "Damage.Backstab", "Damage from behind the target");

	// ─────────────────────────────────────────────────────────────────────────
	// Heal Tags
	// ─────────────────────────────────────────────────────────────────────────
	ADD_TAG(Heal_Regen, "Heal.Regen", "Passive health regeneration");
	ADD_TAG(Heal_Lifesteal, "Heal.Lifesteal", "Health gained from dealing damage");
	ADD_TAG(Heal_Pickup, "Heal.Pickup", "Health from collectible pickups");

	#undef ADD_TAG
}