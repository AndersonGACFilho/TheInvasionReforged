#pragma once

#include "CoreMinimal.h"
#include "UObject/Interface.h"
#include "TIRPoolable.generated.h"

UINTERFACE(MinimalAPI, Blueprintable)
class UTirPoolable : public UInterface
{
	GENERATED_BODY()
};

/**
 * Interface for actors managed by the Spawning/Pooling system.
 */
class TIRCORE_API ITirPoolable
{
	GENERATED_BODY()

public:
	/**
	 * Called when the object is retrieved from the pool (replacement for BeginPlay)
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Pooling")
	void OnAcquireFromPool();

	/**
	 * Called when the object is returned to the pool (replacement for Destroy)
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Pooling")
	void OnReturnToPool();
};
