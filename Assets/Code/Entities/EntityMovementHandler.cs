using UnityEngine;

/// <summary>
/// Base class for handling entity movement.
/// </summary>
/// <remarks>
/// This class serves as a foundational component for managing movement-related functionalities
/// of entities within the game. It provides a reference to the EntityMovement component,
/// allowing derived classes to implement specific movement behaviors.
/// </remarks>
public class EntityMovementHandler : MonoBehaviour
{
    [Tooltip("Reference to the EntityMovement component.")]
    protected EntityMovement entityMovement;

    /// <summary>
    ///  Initializes the EntityMovementHandler component.
    /// </summary>
    public virtual void Awake()
    {
        entityMovement = gameObject.GetComponent<EntityMovement>();
        if(!entityMovement)
        {
            Debug.LogError("EntityMovement component not found on " + gameObject.name);
        }
    }
}
