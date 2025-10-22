using UnityEngine;

namespace Control_Module.HSTM
{
    /// <summary>
    /// Base class for hierarchical entity states.
    /// </summary>
    /// <typeparam name="T">The type of the entity context.</typeparam>
    public abstract class EntityStateBase<T> where T : EntityController
    {
        protected readonly T Context;
        protected readonly EntityHierarchicalStateMachine<T> StateMachine;

        public EntityStateBase<T> parentState { get; protected set; }
        public EntityStateBase<T> currentSubState { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityStateBase{T}"/> class.
        /// </summary>
        /// <param name="context"> The entity context.</param>
        /// <param name="stateMachine"> The hierarchical state machine.</param>
        public EntityStateBase(T context, EntityHierarchicalStateMachine<T> stateMachine)
        {
            this.Context = context;
            this.StateMachine = stateMachine;
        }

        /// <summary>
        /// Called when entering the state.
        /// </summary>
        public void Enter() 
        {
            DoEnter();
        }

        /// <summary>
        /// Called when exiting the state.
        /// </summary>
        public void Exit() 
        {
            DoExit();
            currentSubState?.Exit(); // Cascade exit
        }

        /// <summary>
        /// Called each frame to update the state logic.
        /// </summary>
        public void LogicUpdate() 
        {
            DoLogic();
            currentSubState?.LogicUpdate();
        }

        /// <summary>
        /// Called each physics frame to update the state physics.
        /// </summary>
        public void PhysicsUpdate()
        {
            DoPhysics();
            currentSubState?.PhysicsUpdate(); // Cascade update
        }

        // --- Subclasses override these ---
        protected virtual void DoEnter() { }
        protected virtual void DoExit() { }
        protected virtual void DoLogic() { }
        protected virtual void DoPhysics() { }

        /// <summary>
        /// Sets the new active sub-state for this state.
        /// </summary>
        protected void SetSubState(EntityStateBase<T> newState)
        {
            currentSubState?.Exit();
            currentSubState = newState;
            newState.parentState = this;
            newState.Enter();
        }
    }
}