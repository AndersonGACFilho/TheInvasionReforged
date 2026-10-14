using System.Collections.Generic;
using UnityEngine;

namespace Code.Scenario.Starfield
{
    /// <summary>
    /// Procedural nebula layers drawn between the star layers.
    /// </summary>
    /// <remarks>
    /// Kept apart from <see cref="StarfieldConfig"/> because nebulae are
    /// what changes between sectors of the map while the starfield stays
    /// the same: a sector swaps this asset and keeps its stars.
    /// </remarks>
    [CreateAssetMenu(
        fileName = "NebulaConfig",
        menuName = "The Invasion/Nebula Configuration"
    )]
    public class NebulaConfig : ScriptableObject
    {
        public int worldSeed = 24680;

        [Tooltip(
            "Size of the regions that split the map between nebula layers, " +
            "in world units."
        )]
        [Min(1f)]
        public float regionSize = 120f;

        [Tooltip(
            "Glow added around stars that sit in front of an emission cloud, " +
            "tinted by the cloud."
        )]
        [Min(0f)]
        public float starHalo = 0.6f;

        [Tooltip(
            "Extra star density inside emission regions. 0 leaves density " +
            "unchanged; 2 triples it."
        )]
        [Min(0f)]
        public float starCluster = 2f;

        [Tooltip(
            "How many times per second the flow advances. Matching the " +
            "sprites' animation rate keeps tone edges from shimmering. " +
            "0 animates every frame."
        )]
        [Min(0f)]
        public float animationFrameRate = 10f;

        public List<NebulaLayer> layers = new();

        private void OnValidate()
        {
            SpaceBackgroundAssets.NotifyChanged();
        }
    }
}
