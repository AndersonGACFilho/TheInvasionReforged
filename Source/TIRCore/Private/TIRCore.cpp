/**
 * TIRCore Module Implementation
 *
 * Startup order:
 * - StartupModule() is called when the module is loaded.
 * - Native gameplay tags are registered before any gameplay code executes.
 */

#include "TIRCore.h"
#include "Types/TIRGameplayTags.h"

#define LOCTEXT_NAMESPACE "FTIRCoreModule"

void FTIRCoreModule::StartupModule()
{
	// Initialize native gameplay tags for C++ access.
	// Must be called before any code attempts to use FTIRGameplayTags::Get().
	FTIRGameplayTags::InitializeNativeTags();
}

void FTIRCoreModule::ShutdownModule()
{
	// Currently no cleanup required.
	// Add resource cleanup, unregistration, or shutdown logic here as needed.
}

#undef LOCTEXT_NAMESPACE

IMPLEMENT_MODULE(FTIRCoreModule, TIRCore)