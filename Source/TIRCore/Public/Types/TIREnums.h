#pragma once

#include "CoreMinimal.h"
#include "TIREnums.generated.h"

/**
 * Weapon categories for logic and UI.
 */
UENUM(BlueprintType)
enum class ETirWeaponType : uint8
{
	Primary     UMETA(DisplayName = "Primary Weapon"),
	Special     UMETA(DisplayName = "Special Ability"),
	Utility     UMETA(DisplayName = "Utility/Movement")
};

/**
 * Classification of damage for resistance calculations.
 */
UENUM(BlueprintType)
enum class ETirDamageType : uint8
{
	Physical    UMETA(DisplayName = "Physical Impact"),
	Energy      UMETA(DisplayName = "Energy/Plasma"),
	Explosive   UMETA(DisplayName = "Explosive"),
	Environment UMETA(DisplayName = "Environmental")
};

/**
 * Faction/Team definition for targeting logic.
 */
UENUM(BlueprintType)
enum class ETirTeam : uint8
{
	Player      UMETA(DisplayName = "Player"),
	Enemy       UMETA(DisplayName = "Enemy"),
	Neutral     UMETA(DisplayName = "Neutral")
};