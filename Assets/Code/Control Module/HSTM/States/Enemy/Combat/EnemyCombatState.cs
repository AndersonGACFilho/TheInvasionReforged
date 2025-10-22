
using Control_Module.Enemy;
using Control_Module.HSTM.States.Enemy.Combat.SubState;
using Locomotion.MovementStrategies;
using UnityEngine;

namespace Control_Module.HSTM.States.Enemy.Combat
{
    /// <summary>
    /// State representing the enemy being in combat.
    /// </summary>
    /// <remarks>
    /// This state manages transitions between chasing and attacking sub-states
    /// based on the target's position.
    /// </remarks>
    public class EnemyCombatState : EntityStateBase<EnemyAIContext>
    {
        private EnemyChaseState _chaseState;
        private EnemyAttackState _attackState;
        
        private readonly MovementStrategy _combatMovementStrategy;

        /// <summary>
        /// Initializes the Combat state with its context, state machine, and child states.
        /// </summary>
        /// <param name="context">The AI context.</param>
        /// <param name="stateMachine">The hierarchical state machine.</param>
        /// <param name="chaseState">The chase sub-state.</param>
        /// <param name="attackState">The attack sub-state.</param>
        public EnemyCombatState(
            EnemyAIContext context,
            EntityHierarchicalStateMachine<EnemyAIContext> stateMachine,
            EnemyChaseState chaseState,
            EnemyAttackState attackState
        )
            : base(context, stateMachine)
        {
            _chaseState = chaseState;
            _attackState = attackState;

            if (!Context.MovementStrategies.TryGetValue(MovementStrategyTypeEnum.Chase, out var strategy))
            {
                Debug.LogError($"CombatState: No 'Chase' movement strategy found in context for {Context.gameObject.name}!");
                return;
            }
            _combatMovementStrategy = strategy;
        }

        /// <summary>
        /// Called when entering the Combat state.
        /// </summary>
        protected override void DoEnter()
        {
            // Subscribe to events from children
            Context.OnTargetInAttackRange += HandleTargetInAttackRange;
            Context.OnTargetOutOfAttackRange += HandleTargetOutOfAttackRange;
            Context.OnTargetLost += HandleTargetLost;
            // Default to Chasing
            SetSubState(_chaseState);
        }

        /// <summary>
        /// Called when exiting the Combat state.
        /// </summary>
        protected override void DoExit()
        {
            // Unsubscribe
            Context.OnTargetInAttackRange -= HandleTargetInAttackRange;
            Context.OnTargetOutOfAttackRange -= HandleTargetOutOfAttackRange;
            Context.OnTargetLost -= HandleTargetLost;
            
            Context.EntityMovement.Stop();
        }

        protected override void DoLogic()
        {
            if (!Context.target)
            {
                Context.InvokeTargetLost();
                return;
            }

            float targetLostDistance = Context.detectionRange * 1.2f;
            float distance = Vector2.Distance(Context.transform.position, Context.target.transform.position);

            if (distance > targetLostDistance)
            {
                Context.InvokeTargetLost();
            }
        }

        protected override void DoPhysics()
        {
            if (!_combatMovementStrategy || !Context.target)
                return;

            Vector2 selfPos = Context.transform.position;
            Vector2 targetPos = Context.target.transform.position;

            _combatMovementStrategy.ExecuteMovement(
                selfPos,
                targetPos,
                Context.EntityMovement
            );

            Context.HandleRotation((targetPos - selfPos).normalized);
        }

        /// <summary>
        /// Handles the event when the target is within attack range.
        /// </summary>
        private void HandleTargetInAttackRange()
        {
            SetSubState(_attackState);
        }


        /// <summary>
        /// Handles the event when the target is out of attack range.
        /// </summary>
        private void HandleTargetOutOfAttackRange()
        {
            SetSubState(_chaseState);
        }

        /// <summary>
        /// Handles the event when the target is lost.
        /// </summary>
        private void HandleTargetLost()
        {
            // Target is gone, go back to OutOfCombat
            StateMachine.TransitionTo(Context.OutOfCombatState);
        }
    }
}