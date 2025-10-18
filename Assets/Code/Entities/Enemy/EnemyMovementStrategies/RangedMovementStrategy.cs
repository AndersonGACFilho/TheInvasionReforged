using UnityEngine;

namespace Code.Entities.Enemy.EnemyMovementStrategies
{
    /// <summary>
    /// Movement strategy for ranged enemies that maintain a specific distance from their target.
    /// </summary>
    /// <remarks>
    /// This strategy causes the enemy to move closer to the target if it is too far away,
    /// and to move away if it is too close, maintaining a desired distance with a specified tolerance.
    /// The enemy will stop moving when within the acceptable range.
    /// </remarks>
    [CreateAssetMenu(fileName = "RangedMovementStrategy", menuName = "Enemy Movement Strategies/Ranged Movement Strategy")]
    public class RangedMovementStrategy : MovementStrategy
    {
        [Tooltip("The distance at which the enemy will stop moving towards the target.")]
        public float desiredDistance = 5.0f;
        [Tooltip("The tolerance around the desired distance.")]
        public float tolerance = 2.0f;

        public override void ExecuteMovement(Transform self, Transform target, EntityMovement movementController)
        {
            if (!target)
            {
                movementController.Stop();
                return;
            }

            Vector2 vectorToTarget = target.position - self.position;
            var distance = vectorToTarget.magnitude;

            if (distance < desiredDistance - tolerance)
            {
                movementController.Move(-vectorToTarget.normalized);
                return;
            }
            if (distance > desiredDistance + tolerance)
            {
                movementController.Move(vectorToTarget.normalized);
                return;
            }
            
            movementController.Stop();
        }

    }
}