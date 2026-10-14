using System;
using UnityEngine;

namespace Code.Scenario.Starfield
{
    public enum NebulaKind
    {
        /// <summary>Glowing gas: adds light over what is behind it.</summary>
        Emission,

        /// <summary>Dark dust: blocks what is behind it.</summary>
        Dust
    }

    /// <summary>
    /// One procedural nebula layer. Its depth among the star layers comes
    /// from <see cref="parallaxFactor"/>.
    /// </summary>
    [Serializable]
    public class NebulaLayer
    {
        [Header("Layer")]
        public string layerName;

        public bool enabled = true;

        public NebulaKind kind = NebulaKind.Emission;

        public NebulaPalette palette;

        [Tooltip(
            "Depth: how much the layer follows the camera. Layers are drawn " +
            "together with the star layers, from the lowest value to the highest."
        )]
        [Range(0f, 1f)]
        public float parallaxFactor = 0.05f;

        [Tooltip("Multiplies the palette colors.")]
        [Min(0f)]
        public float intensity = 0.6f;

        [Tooltip(
            "Size of the layer's blocks, in sprite pixels. 1 matches the " +
            "sprites' grid; higher values give a chunkier, lower resolution " +
            "cloud. Ignored when the renderer's pixelsPerUnit is 0."
        )]
        [Range(1, 8)]
        public int pixelScale = 2;

        [Header("Shape")]
        [Tooltip("Approximate size of the main cloud features, in world units.")]
        [Min(0.1f)]
        public float cloudSize = 12f;

        [Range(1, 6)]
        public int octaves = 4;

        [Tooltip("Domain warp: turns round blobs into gas-like filaments.")]
        [Range(0f, 3f)]
        public float warp = 1.2f;

        [Tooltip("Noise value below which there is no cloud.")]
        [Range(0f, 1f)]
        public float threshold = 0.4f;

        [Tooltip("How quickly the cloud goes from its edge to its densest tone.")]
        [Min(0.1f)]
        public float contrast = 2.6f;

        [Header("Emission")]
        [Tooltip("How much the gas dims what is behind it before adding its light.")]
        [Range(0f, 1f)]
        public float veil = 0.35f;

        [Header("Dust")]
        [Tooltip("How much the densest dust hides what is behind it.")]
        [Range(0f, 1f)]
        public float absorption = 0.85f;

        [Header("Flow")]
        [Tooltip(
            "How far the details drift during a cycle. 0 keeps the layer " +
            "still; small values read as slow breathing."
        )]
        [Min(0f)]
        public float flowRadius = 0.3f;

        [Tooltip(
            "Seconds for the details to return to their starting shape. " +
            "The overall shape never moves."
        )]
        [Min(1f)]
        public float cyclePeriod = 180f;

        [Header("Region")]
        [Tooltip(
            "Part of the region mask where this layer appears. 0 to 1 " +
            "covers the whole map; narrower ranges give each layer its own " +
            "areas."
        )]
        [Range(0f, 1f)]
        public float regionMin;

        [Range(0f, 1f)]
        public float regionMax = 1f;

        [Header("Seed")]
        public int seedOffset;
    }
}
