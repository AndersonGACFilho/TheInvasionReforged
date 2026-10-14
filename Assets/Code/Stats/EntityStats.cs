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
    [Header("Speed Stats")]
    [Tooltip("Acceleration of the entity.")]
    public float acceleration;
    [Tooltip("Movement speed of the entity.")]
    public float speed = 2f;
    [Tooltip("Maximum movement speed of the entity.")]
    public float maxSpeed = 5f;
    [Tooltip("Current movement speed of the entity.")]
    public float currentMovementSpeed = 0;
    public void SetMovementSpeed(float value)
    {
        currentMovementSpeed = Mathf.Clamp(
            value,
            0f,
            maxSpeed
        );
    }
}
