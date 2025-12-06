#pragma once

#include "CoreMinimal.h"
#include "UObject/Interface.h"
#include "TIRMovable.generated.h"

UINTERFACE(MinimalAPI, Blueprintable)
class UTIRMovable : public UInterface
{
	GENERATED_BODY()
};

/**
 * ITIRMovable
 *
 * Interface defining a minimal movement contract for actors/components.
 *
 * Purpose:
 * - Standardize movement operations across different actor types.
 * - Allow both C++ and Blueprint implementations of movement logic.
 *
 * Implementation guidance:
 * - For BlueprintNativeEvent functions, implement the _Implementation method in C++:
 *     void MoveInDirection_Implementation(const FVector& Direction, float DeltaTime);
 *     void StopMovement_Implementation();
 *     void SetMovementSpeed_Implementation(float NewSpeed);
 *     float GetMovementSpeed_Implementation() const;
 * - Use frame-rate independent movement (multiply by DeltaTime).
 * - Keep authoritative movement on the server for networked games.
 *
 * Usage:
 * - Check `Implements<UTIRMovable>()` before calling.
 * - Use Execute_* functions for cross-UObject calls.
 */
class TIRCORE_API ITIRMovable
{
	GENERATED_BODY()

public:
	/**
	 * Move the actor/component in a given direction.
	 *
	 * Movement is frame-rate independent (uses DeltaTime internally or externally).
	 * Direction should be normalized; implementations may normalize internally.
	 *
	 * @param Direction The desired direction of travel (normalized).
	 * @param DeltaTime Time elapsed since last frame.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Movement")
	void MoveInDirection(const FVector& Direction, float DeltaTime);

	/**
	 * Immediately stop all movement.
	 *
	 * Implementations should halt velocity, path following, and movement components.
	 * Prefer a deterministic stop (set velocity to zero).
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Movement")
	void StopMovement();

	/**
	 * Set the movement speed.
	 *
	 * @param NewSpeed The new movement speed in world units per second.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Movement")
	void SetMovementSpeed(float NewSpeed);

	/**
	 * Retrieve the current movement speed.
	 *
	 * @return The current movement speed in world units per second.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Movement")
	float GetMovementSpeed() const;
};