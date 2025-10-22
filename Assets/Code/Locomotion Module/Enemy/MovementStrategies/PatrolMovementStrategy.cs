using Locomotion.MovementStrategies;
using UnityEngine;

namespace Locomotion.Enemy.MovementStrategies
{
    /// <summary>
    /// Movement strategy for enemies that patrol between waypoints.
    /// </summary>
    [CreateAssetMenu(fileName = "PatrolMovementStrategy", menuName = "Enemy Movement Strategies/Patrol Movement Strategy")]
    public class PatrolMovementStrategy : MovementStrategy
    {
        /// <summary>
        /// Executes the patrol movement logic towards the target position.
        /// </summary>
        /// <param name="selfPosition">The current position of the entity.</param>
        /// <param name="targetPosition">The target position to patrol towards.</param>
        /// <param name="movementController">The movement controller responsible for handling movement actions.</param>
        public override void ExecuteMovement(Vector2 selfPosition, Vector2 targetPosition, IMovable movementController)
        {
            var direction = targetPosition - selfPosition;
            Debug.Log(direction);
            movementController.Move(direction.normalized);
        }
    }
}