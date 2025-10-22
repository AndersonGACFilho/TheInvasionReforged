using UnityEngine;

namespace Control_Module
{
    /// <summary>
    /// Base class for handling entity movement.
    /// </summary>
    /// <remarks>
    /// This class serves as a foundational component for managing movement-related functionalities
    /// of entities within the game. It provides a reference to the EntityMovement component,
    /// allowing derived classes to implement specific movement behaviors.
    /// </remarks>
    [RequireComponent(typeof(IMovable))]
    public class EntityController : MonoBehaviour
    {
        [Tooltip("Reference to the EntityMovement component.")]
        public IMovable EntityMovement;

        /// <summary>
        ///  Initializes the EntityController component.
        /// </summary>
        public virtual void Awake()
        {
            EntityMovement = gameObject.GetComponent<IMovable>() ?? GetComponentInChildren<IMovable>();

            if (EntityMovement == null)
            {
                Debug.LogError("IMovable(Movement Handler) component is missing on " + gameObject.name);
            }
        }
    }
}
