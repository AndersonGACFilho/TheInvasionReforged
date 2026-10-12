using System;
using UnityEngine;

[Serializable]
public class StarLayer
{
    [Header("Layer Settings")] 
    public string layerName;
    public bool enabled = true;
    
    [Range(0f, 1f)]
    public float parallaxFactor = 0.5f;

    [Range(0f, 1f)]
    public float density = 0.5f;

    [Min(0.001f)]
    public float minSize = 0.02f;

    [Min(0.001f)]
    public float maxSize = 0.08f;

    [Min(0.1f)]
    public float cellSize = 1f;

    public Gradient colorGradient = new Gradient();

    [Min(0f)]
    public float brightness = 1f;
    
    [Header("Twinkle Settings")]
    [Range(0f, 1f)]
    public float twinkleIntensity = 0.3f;

    [Min(0f)]
    public float minTwinkleFrequency = 0.2f;

    [Min(0f)]
    public float maxTwinkleFrequency = 1.5f;
    
    [Range(0f, 1f)]
    public float twinkleProbability = 0.5f;
    
    [Header("Flare Settings")]

    [Range(0f, 1f)]
    public float flareProbability = 0.05f;

    [Min(0f)]
    public float flareIntensity = 0.15f;

    [Min(1f)]
    public float flareLength = 5f;

    [Header("Glow Settings")]
    [Min(0f)]
    public float glowIntensity = 0.5f;
    [Range(0f, 1f)]
    public float glowProbability = 0.15f;

    [Header("Seed")]
    public int seedOffset;
}
