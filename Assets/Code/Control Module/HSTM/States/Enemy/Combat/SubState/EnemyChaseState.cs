
using Control_Module.Enemy;
using Locomotion.MovementStrategies;
using UnityEngine;

namespace Control_Module.HSTM.States.Enemy.Combat.SubState
{
    /// <summary>
    /// State representing the enemy chasing its target.
    /// </summary>
    /// <remarks>
    /// This state handles the logic for pursuing a target and transitioning to attack when in range.
    /// </remarks>
    public class EnemyChaseState : EntityStateBase<EnemyAIContext>
    {
        [ Tooltip("Movement strategy used for chasing the target.") ]
        private readonly MovementStrategy _chaseStrategy;

        /// <summary>
        /// Initializes the Chase state with its context and state machine.
        /// </summary>
        /// <param name="context"> The AI context.</param>
        /// <param name="stateMachine"> The hierarchical state machine.</param>
        public EnemyChaseState(EnemyAIContext context, EntityHierarchicalStateMachine<EnemyAIContext> stateMachine)
            : base(context, stateMachine)
        {}

        /// <summary>
        /// Called when entering the Chase state.
        /// </summary>
        protected override void DoLogic()
        {
            if (!Context.target)
            {
                Context.InvokeTargetLost();
                return;
            }

            // Move towards the target
            var distance = Vector2.Distance(Context.transform.position, Context.target.transform.position);
            if (distance <= Context.attackRange)
            {
                Context.InvokeTargetInAttackRange();
            }
        }
        
    }
}