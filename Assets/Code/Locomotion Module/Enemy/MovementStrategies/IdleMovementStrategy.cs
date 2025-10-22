using Locomotion.MovementStrategies;
using UnityEngine;

namespace Locomotion.Enemy.MovementStrategies
{
    /// <summary>
    /// Movement strategy for enemies that remain idle and do not move.
    /// </summary>
    [CreateAssetMenu(fileName = "IdleMovementStrategy", menuName = "Enemy Movement Strategies/Idle Movement Strategy")]
    public class IdleMovementStrategy : MovementStrategy
    {
        /// <summary>
        /// Executes the idle movement logic, which involves stopping all movement.
        /// </summary>
        /// <param name="selfPosition">The current position of the entity.</param>
        /// <param name="targetPosition">The target position (not used in this strategy).</param>
        /// <param name="movementController">The movement controller responsible for handling movement actions.</param>
        public override void ExecuteMovement(Vector2 selfPosition, Vector2 targetPosition, IMovable movementController)
        {
            movementController.Stop();
        }
    }
}