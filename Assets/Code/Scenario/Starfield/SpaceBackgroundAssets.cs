using System;

namespace Code.Scenario.Starfield
{
    /// <summary>
    /// Tells background renderers that a configuration asset changed.
    /// </summary>
    /// <remarks>
    /// A renderer's OnValidate only fires for edits on the renderer itself,
    /// not on the ScriptableObjects it references. Configs and palettes
    /// raise this event from their own OnValidate so edits show up at once,
    /// in Play mode and in the editor preview.
    /// </remarks>
    public static class SpaceBackgroundAssets
    {
        public static event Action Changed;

        public static void NotifyChanged()
        {
            Changed?.Invoke();
        }
    }
}
