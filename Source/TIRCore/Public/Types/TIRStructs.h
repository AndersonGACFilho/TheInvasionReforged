#pragma once

#include "CoreMinimal.h"
#include "GameplayTagContainer.h"
#include "TIREnums.h"
#include "TIRStructs.generated.h"

/**
 * FTIRDamageInfo
 *
 * Standardized payload for damage events passed between Combat, AI, and UI systems.
 *
 * Purpose:
 * - Encapsulate all damage-related data in a single struct for consistent handling.
 * - Enable rich damage context (source, type, location, tags) for gameplay logic.
 *
 * Usage:
 * - Create and populate when applying damage via ITIRDamageable.
 * - Pass to damage events, UI notifications, and analytics.
 */
USTRUCT(BlueprintType)
struct TIRCORE_API FTIRDamageInfo
{
	GENERATED_BODY()

	/** The raw damage amount to apply (before resistances/modifiers). */
	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category = "TIR|Combat")
	float Amount = 0.0f;

	/** The actor ultimately responsible for the damage (e.g., the player). */
	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category = "TIR|Combat")
	TObjectPtr<AActor> Instigator = nullptr;

	/** The actor that directly caused the damage (e.g., a projectile). */
	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category = "TIR|Combat")
	TObjectPtr<AActor> DamageCauser = nullptr;

	/** Classification of damage for resistance calculations. */
	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category = "TIR|Combat")
	ETIRDamageType DamageType = ETIRDamageType::Physical;

	/** Additional gameplay tags for conditional logic (e.g., critical, elemental). */
	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category = "TIR|Combat")
	FGameplayTagContainer DamageTags;

	/** Full hit result for physics-based damage (normals, bone, phys material). */
	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category = "TIR|Combat")
	FHitResult HitResult;

	/** Helper to get hit location from HitResult. */
	FVector GetHitLocation() const { return HitResult.ImpactPoint; }

	/** Helper to get hit normal from HitResult. */
	FVector GetHitNormal() const { return HitResult.ImpactNormal; }
};

/**
 * FTIRHealInfo
 *
 * Standardized payload for healing events.
 *
 * Purpose:
 * - Encapsulate healing data for consistent handling across systems.
 * - Support heal-over-time, instant heals, and conditional healing logic.
 *
 * Usage:
 * - Create and populate when applying heals via ITIRDamageable::ApplyHeal.
 * - Pass to healing events, UI notifications, and analytics.
 */
USTRUCT(BlueprintType)
struct TIRCORE_API FTIRHealInfo
{
	GENERATED_BODY()

	/** The raw heal amount to apply (before modifiers). */
	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category = "TIR|Combat")
	float Amount = 0.0f;

	/** The actor responsible for the healing (e.g., the player or a heal station). */
	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category = "TIR|Combat")
	TObjectPtr<AActor> Healer = nullptr;

	/** Additional gameplay tags for conditional logic (e.g., regen, lifesteal). */
	UPROPERTY(BlueprintReadWrite, EditAnywhere, Category = "TIR|Combat")
	FGameplayTagContainer HealTags;
};