using UnityEngine;

/// <summary>
/// Holds statistical data for an entity, such as movement speed.
/// </summary>
/// <remarks>
/// This class serves as a container for various stats related to an entity,
/// allowing for easy configuration and retrieval of these values.
/// </remarks>
public class EntityStats : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Movement speed of the entity.")]
    public float speed = 5f;
}
