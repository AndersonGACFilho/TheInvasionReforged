using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "StarfieldConfig",
    menuName = "The Invasion/Starfield Configuration"
)]
public class StarfieldConfig : ScriptableObject
{
    public int worldSeed = 12345;

    public List<StarLayer> layers = new();
}
