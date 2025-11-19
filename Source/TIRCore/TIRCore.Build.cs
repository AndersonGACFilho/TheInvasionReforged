using UnrealBuildTool;

public class TIRCore : ModuleRules
{
    public TIRCore(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

        PublicDependencyModuleNames.AddRange(
            new string[]
            {
                "Core",
            }
        );

        PrivateDependencyModuleNames.AddRange(
            new string[]
            {
                "Core", 
                "CoreUObject", 
                "Engine", 
                "GameplayTags"
            }
        );
    }
}