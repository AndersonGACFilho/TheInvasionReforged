using Control_Module.Enemy;
using Locomotion.MovementStrategies;
using UnityEngine;
namespace Control_Module.HSTM.States.Enemy.OutOfCombat.SubState
{
    /// <summary>
    /// State representing the enemy being patrolling while out of combat.
    /// </summary>
    /// <remarks>
    /// This state checks for nearby targets and triggers a transition to combat when a target is spotted.
    /// </remarks>
    public class EnemyPatrolState : EntityStateBase<EnemyAIContext>
    {
        public Transform targetPoint { get; private set; }

        /// <summary>
        /// Initializes the Patrol state with its context and state machine.
        /// </summary>
        /// <param name="context"> The AI context.</param>
        /// <param name="stateMachine"> The hierarchical state machine.</param>
        public EnemyPatrolState(EnemyAIContext context, EntityHierarchicalStateMachine<EnemyAIContext> stateMachine)
            : base(context, stateMachine)
        {}
        /// <summary>
        /// Called when entering the Patrol state.
        /// </summary>
        protected override void DoEnter()
        {
            if (Context.patrolPoints == null || Context.patrolPoints.Length == 0)
            {
                Debug.LogWarning("PatrolState: No patrol points. Switching to Patrol.");
                return;
            }

            targetPoint = Context.patrolPoints[Context.currentPatrolPointIndex];
        }

        /// <summary>
        /// Called each frame to update the Patrol state logic.
        /// </summary>
        protected override void DoLogic()
        {
            if (!targetPoint) return;

            // Check for arrival
            float arrivalDistance = Vector2.Distance(Context.transform.position, targetPoint.position);
            if (arrivalDistance < 0.1f) 
            {
                // Update index for next time
                Context.currentPatrolPointIndex = (Context.currentPatrolPointIndex + 1) % Context.patrolPoints.Length;
                
                Context.InvokePatrolPointArrived();
            }
        }
    }
}