using Control_Module.Enemy;
using Locomotion.MovementStrategies;
using UnityEngine;
using UnityEngine.UI;

namespace Control_Module.HSTM.States.Enemy.Combat.SubState
{
    public class EnemyAttackState : EntityStateBase<EnemyAIContext>
    {
        private float _attackCooldown = 1.5f;
        private float _attackTimer;
        private MovementStrategy _attackStrategy;

        public EnemyAttackState(EnemyAIContext context, EntityHierarchicalStateMachine<EnemyAIContext> stateMachine) 
            : base(context, stateMachine) { }

        protected override void DoEnter()
        {
            _attackTimer = 0; // Attack immediately
        }

        protected override void DoLogic()
        {
            if (Context.target == null)
            {
                Context.InvokeTargetLost();
                return;
            }

            // Check for transition
            float distance = Vector2.Distance(Context.transform.position, Context.target.transform.position);
            if (distance > Context.attackRange)
            {
                Context.InvokeTargetOutOfAttackRange();
                return;
            }

            // Attack logic
            _attackTimer -= Time.deltaTime;
            if (_attackTimer <= 0)
            {
                PerformAttack();
                _attackTimer = _attackCooldown;
            }
        }

        private void PerformAttack()
        {
            Debug.Log($"{Context.gameObject.name} attacks!");
            // Fire event. Animation/Audio systems could listen.
            Context.InvokeAttackPerformed(); 
        }
    }
}