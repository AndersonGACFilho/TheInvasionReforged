#include "TIRCore.h"

#include "Types/TIRGameplayTags.h"

#define LOCTEXT_NAMESPACE "FTIRCoreModule"

void FTIRCoreModule::StartupModule()
{
	FTirGameplayTags::InitializeNativeTags();
}

void FTIRCoreModule::ShutdownModule()
{
    
}

#undef LOCTEXT_NAMESPACE
    
IMPLEMENT_MODULE(FTIRCoreModule, TIRCore)