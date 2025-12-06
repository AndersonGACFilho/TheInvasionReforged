#pragma once

#include "CoreMinimal.h"
#include "Modules/ModuleManager.h"

/**
 * FTIRCoreModule
 *
 * Primary module for the TIR project, providing foundational types and systems.
 *
 * Purpose:
 * - Initialize core subsystems (gameplay tags, log categories) on engine startup.
 * - Serve as the dependency root for other TIR modules.
 *
 * Contents:
 * - Types: Enums, Structs, Gameplay Tags
 * - Interfaces: Damageable, Movable, Poolable, Targetable
 * - Utils: Log Categories
 *
 * Lifecycle:
 * - StartupModule(): Initializes native gameplay tags.
 * - ShutdownModule(): Performs cleanup (currently no-op).
 */
class FTIRCoreModule : public IModuleInterface
{
public:
    /** Called when the module is loaded. Initializes native gameplay tags. */
    virtual void StartupModule() override;

    /** Called when the module is unloaded. Performs cleanup. */
    virtual void ShutdownModule() override;
};