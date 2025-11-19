#pragma once

#include "CoreMinimal.h"
#include "UObject/Interface.h"
#include "TIRDamageable.generated.h"


UINTERFACE(MinimalAPI, Blueprintable)
class UTirDamageable : public UInterface
{
	GENERATED_BODY()
};

/**
 * Interface for damageable actors.
 * Actors implementing this interface can take damage and report their health status.
 * @see ITirDamageable
 */
class TIRCORE_API ITirDamageable
{
	GENERATED_BODY()

public:
	/**
	 * Apply damage to this actor.
	 * @param Amount The amount of damage to apply.
	 * @param Instigator The actor responsible for the damage (optional).
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Combat")
	void TakeDamage(float Amount, AActor* Instigator);

	/**
	 * Get the current health of the actor.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Combat")
	float GetCurrentHealth() const;

	/**
	 * Check if the actor is dead.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Combat")
	bool IsDead() const;
};
