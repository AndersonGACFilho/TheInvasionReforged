using UnityEngine;

namespace Locomotion.MovementStrategies
{
    /// <summary>
    /// Abstract base class for movement strategies.
    /// </summary>
    /// <remarks>
    /// This class defines the interface for different movement strategies that entities can use.
    /// Concrete implementations must provide the logic for executing movement towards a target.
    /// </remarks>
    public abstract class MovementStrategy : ScriptableObject
    {
        public abstract void ExecuteMovement(Vector2 selfPosition, Vector2 targetPosition, IMovable movementController);
    }
}