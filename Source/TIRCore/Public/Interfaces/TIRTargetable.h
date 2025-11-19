// Source/TIRCore/Public/Interfaces/ITIRTargetable.h
#pragma once

#include "CoreMinimal.h"
#include "UObject/Interface.h"
#include "ITIRTargetable.generated.h"
#include "Types/TIREnums.h"

UINTERFACE(MinimalAPI, Blueprintable)
class UTIRTargetable : public UInterface
{
	GENERATED_BODY()
};

/**
 * Interface for entities that can be targeted by AI or Homing Missiles.
 */
class TIRCORE_API ITIRTargetable
{
	GENERATED_BODY()

public:
	/**
	 * Get the world location of the targetable entity.
	 * @return World location of the targetable entity.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Targeting")
	FVector GetTargetLocation() const;

	/**
	 * Check if this entity is a valid target.
	 * @return Whether this entity is a valid target.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Targeting")
	bool IsValidTarget() const;

	/**
	 * Get the team/faction of this targetable entity.
	 * @return The team/faction of this targetable entity.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Targeting")
	ETirTeam GetTeam() const;
};