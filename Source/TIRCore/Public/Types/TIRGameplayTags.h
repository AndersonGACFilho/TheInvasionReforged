#pragma once

#include "CoreMinimal.h"
#include "GameplayTagContainer.h"

class UGameplayTagsManager;

/**
 * FTIRGameplayTags
 *
 * Singleton containing native Gameplay Tags for the TIR project.
 *
 * Purpose:
 * - Provide safe C++ access to gameplay tags without string literals.
 * - Centralize tag definitions for consistency across the codebase.
 * - Support hierarchical tag queries via parent tags.
 *
 * Usage:
 * - Access tags via `FTIRGameplayTags::Get().TagName`.
 * - Use parent tags for category queries: `HasTag(Get().Input)` matches all input tags.
 * - Call `FTIRGameplayTags::InitializeNativeTags()` during module startup.
 *
 * Extension:
 * - Add new tags as public FGameplayTag members.
 * - Register them in AddAllTags() using UGameplayTagsManager::AddNativeGameplayTag().
 */
struct TIRCORE_API FTIRGameplayTags
{

public:
	/** Get the singleton instance of the gameplay tags. */
	static const FTIRGameplayTags& Get() { return GameplayTags; }

	/** Initialize native gameplay tags. Called during module startup. */
	static void InitializeNativeTags();

	// ─────────────────────────────────────────────────────────────────────────
	// Parent Tags (for hierarchical queries)
	// ─────────────────────────────────────────────────────────────────────────

	FGameplayTag Input;            // Parent: "Input"
	FGameplayTag Weapon;           // Parent: "Weapon"
	FGameplayTag Weapon_Type;      // Parent: "Weapon.Type"
	FGameplayTag Weapon_State;     // Parent: "Weapon.State"
	FGameplayTag State;            // Parent: "State"
	FGameplayTag Attribute;        // Parent: "Attribute"
	FGameplayTag Damage;           // Parent: "Damage"
	FGameplayTag Heal;             // Parent: "Heal"

	// ─────────────────────────────────────────────────────────────────────────
	// Input Tags
	// ─────────────────────────────────────────────────────────────────────────

	FGameplayTag Input_Move;
	FGameplayTag Input_Fire;
	FGameplayTag Input_Dash;
	FGameplayTag Input_Ability_1;
	FGameplayTag Input_Ability_2;
	FGameplayTag Input_Ability_3;

	// ─────────────────────────────────────────────────────────────────────────
	// Weapon Tags
	// ─────────────────────────────────────────────────────────────────────────

	FGameplayTag Weapon_Type_PlasmaBeam;
	FGameplayTag Weapon_Type_IonCannon;
	FGameplayTag Weapon_State_Firing;
	FGameplayTag Weapon_State_Reloading;

	// ─────────────────────────────────────────────────────────────────────────
	// State Tags
	// ─────────────────────────────────────────────────────────────────────────

	FGameplayTag State_Dead;
	FGameplayTag State_Invulnerable;
	FGameplayTag State_Stunned;

	// ─────────────────────────────────────────────────────────────────────────
	// Attribute Tags
	// ─────────────────────────────────────────────────────────────────────────

	FGameplayTag Attribute_Health;
	FGameplayTag Attribute_MaxHealth;
	FGameplayTag Attribute_MovementSpeed;

	// ─────────────────────────────────────────────────────────────────────────
	// Damage Tags (for conditional damage logic)
	// ─────────────────────────────────────────────────────────────────────────

	FGameplayTag Damage_Critical;
	FGameplayTag Damage_DOT;           // Damage over time
	FGameplayTag Damage_Backstab;

	// ─────────────────────────────────────────────────────────────────────────
	// Heal Tags (for conditional heal logic)
	// ─────────────────────────────────────────────────────────────────────────

	FGameplayTag Heal_Regen;
	FGameplayTag Heal_Lifesteal;
	FGameplayTag Heal_Pickup;

protected:
	void AddAllTags(UGameplayTagsManager& Manager);

private:
	static FTIRGameplayTags GameplayTags;
};