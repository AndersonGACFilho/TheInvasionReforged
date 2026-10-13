using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public struct StarfieldLayerData
{
    public uint Enabled;
    public float ParallaxFactor;
    public float Density;
    public float MinSize;

    public float MaxSize;
    public float CellSize;
    public float Brightness;
    public float TwinkleIntensity;

    public float MinTwinkleFrequency;
    public float MaxTwinkleFrequency;
    public float TwinkleProbability;
    public float GlowIntensity;

    public float FlareProbability;
    public float FlareIntensity;
    public float FlareLength;
    
    public float GlowProbability;
    public uint SeedOffset;
    
    public float Padding0;
    public float Padding1;
    public float Padding2;

    public static StarfieldLayerData FromConfig(StarLayer layer)
    {
        return new StarfieldLayerData
        {
            Enabled = layer.enabled ? 1u : 0u,
            ParallaxFactor = layer.parallaxFactor,
            Density = layer.density,
            MinSize = layer.minSize,
            MaxSize = layer.maxSize,
            CellSize = layer.cellSize,
            Brightness = layer.brightness,
            TwinkleIntensity = layer.twinkleIntensity,
            MinTwinkleFrequency = layer.minTwinkleFrequency,
            MaxTwinkleFrequency = layer.maxTwinkleFrequency,
            TwinkleProbability = layer.twinkleProbability,
            GlowIntensity = layer.glowIntensity,
            GlowProbability = layer.glowProbability,
            SeedOffset = unchecked((uint)layer.seedOffset),
            FlareProbability = layer.flareProbability,
            FlareIntensity = layer.flareIntensity,
            FlareLength = layer.flareLength
        };
    }
}
