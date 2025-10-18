using UnityEngine;

namespace Code.Entities.Enemy.EnemyMovementStrategies
{
    /// <summary>
    /// Abstract base class for enemy movement strategies.
    /// </summary>
    /// <remarks>
    /// This class defines the interface for different movement strategies that enemies can use.
    /// Concrete implementations must provide the logic for executing movement towards a target.
    /// </remarks>
    public abstract class MovementStrategy : ScriptableObject
    {
        /// <summary>
        /// Executes the movement logic for the enemy towards the target.
        /// </summary>
        /// <param name="self"> the transform of the enemy entity. </param>
        /// <param name="target"> the transform of the target entity. </param>
        /// <param name="movementController"> the movement controller responsible for handling movement actions. </param>
        public abstract void ExecuteMovement(Transform self, Transform target, EntityMovement movementController);
    }
}