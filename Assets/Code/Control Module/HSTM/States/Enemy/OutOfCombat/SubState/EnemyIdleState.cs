using Control_Module.Enemy;
using Locomotion.MovementStrategies;
using UnityEngine;
namespace Control_Module.HSTM.States.Enemy.OutOfCombat.SubState
{
    /// <summary>
    /// State representing the enemy being idle while out of combat.
    /// </summary>
    /// <remarks>
    /// This state checks for nearby targets and triggers a transition to combat when a target is spotted.
    /// </remarks>
    public class EnemyIdleState : EntityStateBase<EnemyAIContext>
    {
        private float _idleTimer;

        /// <summary>
        /// Initializes the Idle state with its context and state machine.
        /// </summary>
        /// <param name="context"> The AI context.</param>
        /// <param name="stateMachine"> The hierarchical state machine.</param>
        public EnemyIdleState(EnemyAIContext context, EntityHierarchicalStateMachine<EnemyAIContext> stateMachine)
            : base(context, stateMachine)
        {}

        /// <summary>
        /// Called when entering the Idle state.
        /// </summary>
        protected override void DoEnter()
        {
            _idleTimer = Context.patrolIdleTime;
        }

        /// <summary>
        /// Called each frame to update the Idle state logic.
        /// </summary>
        protected override void DoLogic()
        {
            // Don't tick down timer if we have no points (idle forever)
            if (Context.patrolPoints == null || Context.patrolPoints.Length == 0)
            {
                return;
            }

            _idleTimer -= Time.deltaTime;
            if (_idleTimer <= 0)
            {
                Context.InvokeIdleTimeFinished();
            }
        }
    }
}