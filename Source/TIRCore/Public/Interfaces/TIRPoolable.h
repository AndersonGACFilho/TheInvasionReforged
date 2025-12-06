#pragma once

#include "CoreMinimal.h"
#include "UObject/Interface.h"
#include "TIRPoolable.generated.h"

UINTERFACE(MinimalAPI, Blueprintable)
class UTIRPoolable : public UInterface
{
	GENERATED_BODY()
};

/**
 * ITIRPoolable
 *
 * Interface for objects managed by a spawning/pooling system.
 *
 * Purpose:
 * - Provide lifecycle hooks for objects when acquired from or returned to a pool.
 * - Enable efficient object reuse without repeated allocation/destruction overhead.
 *
 * Implementation guidance:
 * - For BlueprintNativeEvent functions, implement the _Implementation method in C++:
 *     void OnAcquireFromPool_Implementation();
 *     void OnReturnToPool_Implementation();
 *
 * - OnAcquireFromPool checklist:
 *   [ ] Reset health to max (if ITIRDamageable)
 *   [ ] Enable collision and visibility
 *   [ ] Reset timers and cooldowns
 *   [ ] Clear any runtime state from previous use
 *   [ ] Enable tick if needed
 *
 * - OnReturnToPool checklist:
 *   [ ] Stop all movement (if ITIRMovable)
 *   [ ] Disable collision and visibility
 *   [ ] Cancel active timers and effects
 *   [ ] Clear references to other actors
 *   [ ] Disable tick to save performance
 *
 * - Keep these functions lightweight and idempotent.
 *
 * Usage:
 * - Check `Implements<UTIRPoolable>()` before calling.
 * - Use Execute_* functions for cross-UObject calls.
 */
class TIRCORE_API ITIRPoolable
{
	GENERATED_BODY()

public:
	/**
	 * Called when the object is retrieved from the pool.
	 *
	 * Use this as a replacement for BeginPlay initialization for pooled objects.
	 * Reset all runtime state and enable functionality required while active.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Pooling")
	void OnAcquireFromPool();

	/**
	 * Called when the object is returned to the pool.
	 *
	 * Stop ongoing behavior, clear references, and prepare for reuse.
	 * Keep operations performant to avoid spikes when returning many objects.
	 */
	UFUNCTION(BlueprintNativeEvent, BlueprintCallable, Category = "TIR|Pooling")
	void OnReturnToPool();
};