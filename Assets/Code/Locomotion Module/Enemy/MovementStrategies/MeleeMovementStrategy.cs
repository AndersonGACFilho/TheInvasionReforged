using Locomotion.MovementStrategies;
using UnityEngine;

namespace Locomotion.Enemy.MovementStrategies
{
    /// <summary>
    /// Movement strategy for melee enemies that move directly towards their target until within a specified stop
    /// distance.
    /// </summary>
    /// <remarks>
    /// This strategy causes the enemy to move directly towards the target until it is within the defined stop
    /// distance, at which point it will stop moving.
    /// </remarks>
    [CreateAssetMenu(fileName = "MeleeMovementStrategy", menuName = "Enemy Movement Strategies/Melee Movement Strategy")]
    public class MeleeMovementStrategy : MovementStrategy
    {
        [Tooltip("The distance at which the enemy will stop moving towards the target.")]
        public float stopDistance = 0.5f;

        /// <summary>
        /// Executes the movement logic for melee enemies towards the target.
        /// </summary>
        /// <param name="self"> The transform of the enemy entity. </param>
        /// <param name="target"> The transform of the target entity. </param>
        /// <param name="movementController"> The movement controller responsible for handling movement actions. </param>
        public override void ExecuteMovement(Vector2 selfPosition, Vector2 targetPosition, IMovable movementController)
        {
            Vector2 direction = targetPosition - selfPosition;
            if (direction.magnitude > stopDistance)
            {
                movementController.Move(direction.normalized);
                return;
            }
            
            movementController.Stop();
        }

    }
}