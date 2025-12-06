#pragma once

#include "CoreMinimal.h"
#include "UObject/Interface.h"
#include "Types/TIRStructs.h"
#include "TIRDamageable.generated.h"

UINTERFACE(MinimalAPI, Blueprintable)
class UTIRDamageable : public UInterface
{
	GENERATED_BODY()
};

/**
 * ITIRDamageable
 *
 * Interface for actors or components that can receive damage/healing and expose health state.
 *
 * Purpose:
 * - Standardize how damage/healing is applied and how health state is queried.
 * - Allow both C++ and Blueprint implementations of health logic.
 *
 * Implementation guidance:
 * - For BlueprintNativeEvent functions, implement the _Implementation method in C++:
 *     void ApplyDamage_Implementation(const FTIRDamageInfo& DamageInfo);
 *     void ApplyHeal_Implementation(float HealAmount);
 *     float GetCurrentHealth_Implementation() const;
 *     float GetMaxHealth_Implementation() const;
 *     bool IsDead_Implementation() const;
 * - Prefer authoritative damage application on the server.
 * - Validate damage source and amount before applying changes.
 * - For pooled objects, reset health in OnAcquireFromPool().
 *
 * Usage:
 * - Check `Implements<UTIRDamageable>()` before calling.
 * - Use Execute_* functions for cross-UObject calls:
 *     ITIRDamageable::Execute_ApplyDamage(TargetObject, DamageInfo);
 *     ITIRDamageable::Execute_ApplyHeal(TargetObject, HealAmount);
 */
class TIRCORE_API ITIRDamageable
{
	GENERATED_BODY()

public:
	/**
	 * Apply damage to the entity.
	 *
	 * Implementations should reduce health based on the provided damage info.
	 * Consider resistances, immunities, State_Invulnerable tag, and death handling.
	 *
	 * @param DamageInfo Struct containing damage amount, instigator, type, tags, and hit result.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Combat")
	void ApplyDamage(const FTIRDamageInfo& DamageInfo);

	/**
	 * Apply healing to the entity.
	 *
	 * Implementations should increase health up to max health.
	 * Consider healing modifiers, buffs, and any cap logic.
	 *
	 * @param HealAmount The amount of health to restore.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Combat")
	void ApplyHeal(float HealAmount);

	/**
	 * Retrieve the current health value.
	 *
	 * @return Current health as a float.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Combat")
	float GetCurrentHealth() const;

	/**
	 * Retrieve the maximum health value.
	 *
	 * @return Maximum health capacity as a float.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Combat")
	float GetMaxHealth() const;

	/**
	 * Returns whether the entity is considered dead.
	 *
	 * Implementations should return true when health <= 0 or State_Dead tag is present.
	 *
	 * @return true if dead; false otherwise.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Combat")
	bool IsDead() const;
};