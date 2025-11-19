// Source/TIRCore/Public/Types/TIRStructs.h
#pragma once

#include "CoreMinimal.h"
#include "GameplayTagContainer.h"
#include "TIRWeaponType.h"
#include "TIRStructs.generated.h"

/**
 * Standardized payload for damage events.
 * Passed between Combat, AI, and UI systems.
 */
USTRUCT(BlueprintType)
struct FTirDamageInfo
{
	GENERATED_BODY()

	UPROPERTY(BlueprintReadWrite, EditAnywhere)
	float Amount = 0.0f;

	UPROPERTY(BlueprintReadWrite, EditAnywhere)
	AActor* Instigator = nullptr;

	UPROPERTY(BlueprintReadWrite, EditAnywhere)
	AActor* DamageCauser = nullptr;

	UPROPERTY(BlueprintReadWrite, EditAnywhere)
	ETirDamageType DamageType = ETirDamageType::Physical;

	UPROPERTY(BlueprintReadWrite, EditAnywhere)
	FGameplayTagContainer DamageTags;

	UPROPERTY(BlueprintReadWrite, EditAnywhere)
	FVector HitLocation = FVector::ZeroVector;
};