#pragma once

#include "CoreMinimal.h"
#include "UObject/Interface.h"
#include "Types/TIREnums.h"
#include "TIRTargetable.generated.h"

UINTERFACE(MinimalAPI, Blueprintable)
class UTIRTargetable : public UInterface
{
	GENERATED_BODY()
};

/**
 * ITIRTargetable
 *
 * Interface for entities that can be targeted by AI, weapons, or homing systems.
 *
 * Purpose:
 * - Provide a unified way to query an entity's target location, validity, and team.
 * - Support friend/foe determination for targeting systems.
 *
 * Implementation guidance:
 * - For BlueprintNativeEvent functions, implement the _Implementation method in C++:
 *     FVector GetTargetLocation_Implementation() const;
 *     bool IsValidTarget_Implementation() const;
 *     ETIRTeam GetTeam_Implementation() const;
 *
 * - GetTargetLocation: Return a stable point (socket, collision center, actor origin).
 * - IsValidTarget: Consider health, visibility, invulnerability, and State_Dead tag.
 * - GetTeam: Return the appropriate ETIRTeam for friend/foe logic.
 *
 * - Keep authoritative logic on the server for networked games.
 * - Cache expensive computations where appropriate.
 *
 * Usage:
 * - Check `Implements<UTIRTargetable>()` before calling.
 * - Use Execute_* functions for cross-UObject calls.
 */
class TIRCORE_API ITIRTargetable
{
	GENERATED_BODY()

public:
	/**
	 * Get the world-space location representing this target.
	 *
	 * Return a stable, valid world location (socket, actor root, or collision center).
	 * Prefer stable points to avoid jitter for homing systems.
	 *
	 * @return World-space FVector of the target location.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Targeting")
	FVector GetTargetLocation() const;

	/**
	 * Returns whether this entity is currently a valid target.
	 *
	 * Consider: health > 0, visibility, not invulnerable, not dead.
	 * Return false for destroyed or out-of-scope targets.
	 *
	 * @return true if the entity can be targeted; false otherwise.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Targeting")
	bool IsValidTarget() const;

	/**
	 * Get this entity's team/faction identifier.
	 *
	 * Used by targeting systems to determine friend/foe relationships.
	 *
	 * @return ETIRTeam enum value representing the entity's team.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Targeting")
	ETIRTeam GetTeam() const;
};
