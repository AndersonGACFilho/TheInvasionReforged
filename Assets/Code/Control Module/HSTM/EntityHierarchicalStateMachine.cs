using TMPro;
using UnityEngine;
namespace Control_Module.HSTM
{
    /// <summary>
    /// Manages the state transitions and updates for a hierarchical state machine.
    /// </summary>
    /// <typeparam name="T">The type of the entity context.</typeparam>
    /// <remarks>
    /// This class handles the current state and facilitates transitions between states.
    /// It also provides methods for updating the logic and physics of the current state.
    /// </remarks>
    public class EntityHierarchicalStateMachine<T> where T : EntityController
    {
        [Header("State Machine")]
        
        public EntityStateBase<T> currentState { get; private set; }
        private readonly T _context;

        public EntityHierarchicalStateMachine(T context)
        {
            _context = context;
        }

        public void Initialize(EntityStateBase<T> startingState)
        {
            currentState = startingState;
            currentState.Enter();
        }

        public void TransitionTo(EntityStateBase<T> newState)
        {
            if (currentState == newState) return;
            
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void LogicUpdate()
        {
            currentState?.LogicUpdate();
        }

        public void PhysicsUpdate()
        {
            currentState?.PhysicsUpdate();
        }
    }
}