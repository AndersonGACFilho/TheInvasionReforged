using System.Collections.Generic;
using Code.Scenario.Starfield;
using UnityEngine;

[CreateAssetMenu(
    fileName = "StarfieldConfig",
    menuName = "The Invasion/Starfield Configuration"
)]
public class StarfieldConfig : ScriptableObject
{
    public int worldSeed = 12345;

    public List<StarLayer> layers = new();

    private void OnValidate()
    {
        SpaceBackgroundAssets.NotifyChanged();
    }
}
