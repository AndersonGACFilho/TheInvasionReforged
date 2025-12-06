#pragma once

#include "CoreMinimal.h"
#include "TIREnums.generated.h"

/**
 * Weapon categories for logic and UI.
 */
UENUM(BlueprintType)
enum class ETIRWeaponType : uint8
{
	Primary     UMETA(DisplayName = "Primary Weapon"),
	Special     UMETA(DisplayName = "Special Ability"),
	Utility     UMETA(DisplayName = "Utility/Movement")
};

/**
 * Classification of damage for resistance calculations.
 */
UENUM(BlueprintType)
enum class ETIRDamageType : uint8
{
	Physical    UMETA(DisplayName = "Physical Impact"),
	Energy      UMETA(DisplayName = "Energy/Plasma"),
	Explosive   UMETA(DisplayName = "Explosive"),
	Environment UMETA(DisplayName = "Environmental")
};

/**
 * Classification of healing for buff/modifier calculations.
 */
UENUM(BlueprintType)
enum class ETIRHealType : uint8
{
	Instant     UMETA(DisplayName = "Instant Heal"),
	Regeneration UMETA(DisplayName = "Regeneration Over Time"),
	Lifesteal   UMETA(DisplayName = "Lifesteal"),
	Pickup      UMETA(DisplayName = "Health Pickup")
};

/**
 * Faction/Team definition for targeting logic.
 */
UENUM(BlueprintType)
enum class ETIRTeam : uint8
{
	Player      UMETA(DisplayName = "Player"),
	Enemy       UMETA(DisplayName = "Enemy"),
	Neutral     UMETA(DisplayName = "Neutral")
};
