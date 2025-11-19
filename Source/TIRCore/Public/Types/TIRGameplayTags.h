#pragma once

#include "CoreMinimal.h"
#include "GameplayTagContainer.h"

class UGameplayTagsManager;

/**
 * Singleton containing native Gameplay Tags.
 * Allows safe C++ access to tags without string literals.
 */
struct TIRCORE_API FTirGameplayTags
{
public:
	/**
	 * Get the singleton instance of the gameplay tags.
	 * @return Singleton instance of the gameplay tags.
	 */
	static const FTirGameplayTags& Get() { return GameplayTags; }

	/**
	 * Initialize native gameplay tags.
	 * Called during module startup.
	 */
	static void InitializeNativeTags();

	// Input Tags
	FGameplayTag Input_Move;
	FGameplayTag Input_Fire;
	FGameplayTag Input_Dash;
	FGameplayTag Input_Ability_1;
	FGameplayTag Input_Ability_2;
	FGameplayTag Input_Ability_3;

	// Weapon Tags
	FGameplayTag Weapon_Type_PlasmaBeam;
	FGameplayTag Weapon_Type_IonCannon;
	FGameplayTag Weapon_State_Firing;
	FGameplayTag Weapon_State_Reloading;

	// State Tags
	FGameplayTag State_Dead;
	FGameplayTag State_Invulnerable;
	FGameplayTag State_Stunned;

	// Attribute Tags
	FGameplayTag Attribute_Health;
	FGameplayTag Attribute_MaxHealth;
	FGameplayTag Attribute_MovementSpeed;

protected:
	/**
	 * Add all tags to the Gameplay Tags Manager.
	 * @param Manager The Gameplay Tags Manager to add tags to.
	 */
	void AddAllTags(UGameplayTagsManager& Manager);

private:
	static FTirGameplayTags GameplayTags;
};