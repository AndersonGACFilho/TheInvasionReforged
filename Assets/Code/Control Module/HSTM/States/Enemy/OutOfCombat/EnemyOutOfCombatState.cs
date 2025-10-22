using Control_Module.Enemy;
using Control_Module.HSTM.States.Enemy.OutOfCombat.SubState;
using Locomotion.MovementStrategies;
using UnityEngine;

namespace Control_Module.HSTM.States.Enemy.OutOfCombat
{
    /// <summary>
    /// State representing the enemy being out of combat.
    /// </summary>
    /// <remarks>
    /// This state manages transitions to combat when a target is spotted.
    /// </remarks>
    public class EnemyOutOfCombatState : EntityStateBase<EnemyAIContext>
    {
        private EnemyIdleState _idleState;
        private EnemyPatrolState _patrolState;

        private readonly MovementStrategy _idleStrategy;
        private readonly MovementStrategy _patrolStrategy;

        /// <summary>
        /// Initializes the OutOfCombat state with its context, state machine, and child states.
        /// </summary>
        /// <param name="context">The AI context.</param>
        /// <param name="stateMachine">The hierarchical state machine.</param>
        /// <param name="idleState">The idle sub-state.</param>
        /// <param name="patrolState">The patrol sub-state (optional).</param>
        public EnemyOutOfCombatState(
            EnemyAIContext context,
            EntityHierarchicalStateMachine<EnemyAIContext> stateMachine, 
            EnemyIdleState idleState,
            EnemyPatrolState patrolState = null
            )
            : base(context, stateMachine)
        {
            _idleState = idleState;
            _patrolState = patrolState;

            Context.MovementStrategies.TryGetValue(MovementStrategyTypeEnum.Idle, out _idleStrategy);
            Context.MovementStrategies.TryGetValue(MovementStrategyTypeEnum.Patrol, out _patrolStrategy);
        }

        /// <summary>
        /// Called when entering the OutOfCombat state.
        /// </summary>
        protected override void DoEnter()
        {
            Context.OnTargetSpotted += HandleTargetSpotted;
            Context.OnPatrolPointArrived += HandlePatrolPointArrived;
            Context.OnIdleTimeFinished += HandleIdleTimeFinished;

            if (_patrolState != null && Context.patrolPoints != null && Context.patrolPoints.Length > 0)
                SetSubState(_patrolState);
            else
                SetSubState(_idleState);
        }

        /// <summary>
        /// Called each frame to update the OutOfCombat state logic.
        /// </summary>
        protected override void DoLogic()
        {
            if (!Context.target) return;

            var distance = Vector2.Distance(Context.transform.position, Context.target.transform.position);
            if (distance < Context.detectionRange)
            {
                Context.InvokeTargetSpotted(); 
            }
        }

        /// <summary>
        /// Called each physics update to handle movement in the OutOfCombat state.
        /// </summary>
        /// <remarks>
        /// This method uses the appropriate movement strategy based on the current sub-state (Idle or Patrol).
        /// </remarks>
        protected override void DoPhysics()
        {
            // Opção 1: O estado é Idle?
            if (currentSubState == _idleState)
            {
                if (_idleStrategy)
                {
                    _idleStrategy.ExecuteMovement(
                        Context.transform.position,
                        Context.transform.position,
                        Context.EntityMovement
                    );
                }
                else
                {
                    Context.EntityMovement.Stop();
                }
            }
            else if (currentSubState == _patrolState && _patrolStrategy != null)
            {
                Transform targetPoint = _patrolState.targetPoint;
                if (targetPoint)
                {
                    Vector2 selfPos = Context.transform.position;
                    Vector2 targetPos = targetPoint.position;

                    _patrolStrategy.ExecuteMovement(
                        selfPos,
                        targetPos,
                        Context.EntityMovement
                    );
                    Context.HandleRotation((targetPos - selfPos).normalized);
                }
            }
        }

        /// <summary>
        /// Called when exiting the OutOfCombat state.
        /// </summary>
        protected override void DoExit()
        {
            // Unsubscribe to prevent memory leaks
            Context.OnTargetSpotted -= HandleTargetSpotted;
            Context.OnPatrolPointArrived -= HandlePatrolPointArrived;
            Context.OnIdleTimeFinished -= HandleIdleTimeFinished;
        }

        /// <summary>
        /// Handles the event when a target is spotted.
        /// </summary>
        private void HandleTargetSpotted()
        {
            // Transition the entire machine to the Combat state
            StateMachine.TransitionTo(Context.CombatState);
        }

        private void HandlePatrolPointArrived()
        {
            if (currentSubState == _patrolState)
            {
                SetSubState(_idleState);
            }
        }

        private void HandleIdleTimeFinished()
        {
            if (currentSubState == _idleState && _patrolState != null)
            {
                SetSubState(_patrolState);
            }
        }
    }
}