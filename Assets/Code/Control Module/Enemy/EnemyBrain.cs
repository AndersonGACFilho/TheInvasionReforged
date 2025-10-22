using Control_Module.HSTM;
using Control_Module.HSTM.States.Enemy.Combat;
using Control_Module.HSTM.States.Enemy.Combat.SubState;
using Control_Module.HSTM.States.Enemy.OutOfCombat;
using Control_Module.HSTM.States.Enemy.OutOfCombat.SubState;
using UnityEngine;

namespace Control_Module.Enemy
{
    /// <summary>
    /// Runs the Hierarchical State Machine for an enemy (SRP).
    /// Its single responsibility is to initialize and update the AI's states.
    /// </summary>
    [RequireComponent(typeof(EnemyAIContext))]
    public class EnemyBrain : MonoBehaviour
    {
        private EnemyAIContext _context;

        /// <summary>
        /// Initializes the Hierarchical State Machine and its states.
        /// </summary>
        private void Start()
        {
            _context = GetComponent<EnemyAIContext>();

            // 1. Create the HSTM
            _context.StateMachine = new EntityHierarchicalStateMachine<EnemyAIContext>(_context);

            // 2. Create leaf-states
            _context.IdleState = new EnemyIdleState(_context, _context.StateMachine);
            _context.PatrolState = new EnemyPatrolState(_context, _context.StateMachine);
            _context.ChaseState = new EnemyChaseState(_context, _context.StateMachine);
            _context.AttackState = new EnemyAttackState(_context, _context.StateMachine);

            // 3. Create super-states (passing in children)
            _context.OutOfCombatState = new EnemyOutOfCombatState(_context, _context.StateMachine, _context.IdleState,
                _context.PatrolState);
            _context.CombatState = new EnemyCombatState(_context, _context.StateMachine, _context.ChaseState,
                _context.AttackState);

            // 4. Initialize HSTM with the root state
            _context.StateMachine.Initialize(_context.OutOfCombatState);
        }

        /// <summary>
        /// Updates the state machine logic each frame.
        /// </summary>
        private void Update()
        {
            _context.StateMachine?.LogicUpdate();
            UpdateDebugText();
        }

        /// <summary>
        /// Updates the state machine physics each fixed frame.
        /// </summary>
        private void FixedUpdate()
        {
            _context.StateMachine?.PhysicsUpdate();
        }

        /// <summary>
        /// Updates the debug text UI element with the current state information.
        /// </summary>
        private void UpdateDebugText()
        {
            if (!_context.debugText || _context.StateMachine?.currentState == null)
                return;

            // Get current main state and sub-state
            var mainState = _context.StateMachine.currentState;
            var subState = mainState.currentSubState;

            // Format state names by removing common prefixes/suffixes
            string mainStateName = mainState.GetType().Name.Replace("Enemy", "").Replace("State", "");
            
            string fullStateText = $"State: {mainStateName}";

            if (subState != null)
            {
                string subStateName = subState.GetType().Name.Replace("Enemy", "").Replace("State", "");
                fullStateText += $" ({subStateName})";
            }
            
            _context.debugText.text = fullStateText;
        }
    }
}