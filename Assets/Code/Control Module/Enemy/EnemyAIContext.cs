using System;
using System.Collections.Generic;
using Control_Module.HSTM;
using Control_Module.HSTM.States.Enemy.Combat;
using Control_Module.HSTM.States.Enemy.Combat.SubState;
using Control_Module.HSTM.States.Enemy.OutOfCombat;
using Control_Module.HSTM.States.Enemy.OutOfCombat.SubState;
using Locomotion;
using Locomotion.MovementStrategies;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Control_Module.Enemy
{
    /// <summary>
    /// Types of movement strategies available for the enemy AI.
    /// </summary>
    /// <remarks>
    /// This enum is used to identify and select different movement strategies
    /// for the enemy AI context.
    /// </remarks>
    [Serializable]
    public enum MovementStrategyTypeEnum
    {
        Idle,
        Patrol,
        Chase
    }

    /// <summary>
    /// A helper class that Unity can serialize to create 
    /// a key-value pair for the Inspector.
    /// </summary>
    [Serializable]
    public class MovementStrategyPair
    {
        public MovementStrategyTypeEnum type;
        public MovementStrategy strategy;
    }

    /// <summary>
    /// Acts as the "Context" for the enemy AI (SRP).
    /// Holds all data, component references, and events that AI states need.
    /// This is a passive data container; the EnemyBrain runs the logic.
    /// </summary>
    public class EnemyAIContext : EntityController
    {
        [Header("Enemy AI Context")]
        [Tooltip("The target the enemy will move towards.")]
        public GameObject target;

        [Tooltip("Tag used to identify the target object in the scene.")]
        public string targetTag = "Player";

        [Space]
        [Tooltip("Assign each movement strategy to its type here. This list will be used to build the runtime dictionary.")]
        public List<MovementStrategyPair> movementStrategyConfig;
        [Tooltip("Drag and drop the movement behaviors from the project.")]
        public Dictionary<MovementStrategyTypeEnum, MovementStrategy> MovementStrategies { get; private set; } = new();

        [Header("AI Properties")] 
        [Tooltip("The range at which the enemy can detect the target.")]
        public float detectionRange = 10f;
        [Tooltip("The range at which the enemy can attack the target.")]
        public float attackRange = 2f;

        [Header("Patrol Settings")] 
        [Tooltip("The points the enemy will move between when in the Patrol state.")]
        public Transform[] patrolPoints;
        [Tooltip("The time the enemy will idle at each patrol point.")]
        public float patrolIdleTime = 3.0f;
        [Tooltip("The index of the current patrol point the enemy is moving towards.")]
        public int currentPatrolPointIndex = 0;
        
        [Tooltip("Debug text to display AI state information.")]
        public TMP_Text debugText;

        // --- MULTICAST DELEGATES (Events) ---
        // This is the core of decoupling states (Observer Pattern)
        public event Action OnTargetSpotted;
        public event Action OnTargetLost;
        public event Action OnTargetInAttackRange;
        public event Action OnTargetOutOfAttackRange;
        public event Action OnAttackPerformed;
        public event Action OnPatrolPointArrived;
        public event Action OnIdleTimeFinished;
        

        // --- State Machine References (Populated by EnemyBrain) ---
        [HideInInspector] public EntityHierarchicalStateMachine<EnemyAIContext> StateMachine;
        [HideInInspector] public EnemyOutOfCombatState OutOfCombatState;
        [HideInInspector] public EnemyCombatState CombatState;
        [HideInInspector] public EnemyIdleState IdleState;
        [HideInInspector] public EnemyPatrolState PatrolState;
        [HideInInspector] public EnemyChaseState ChaseState;
        [HideInInspector] public EnemyAttackState AttackState;

        /// <summary>
        /// Initializes the enemy movement handler, target reference, and movement strategies.
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            target = GameObject.FindGameObjectWithTag(targetTag);

            if (movementStrategyConfig != null)
            {
                foreach (var pair in movementStrategyConfig)
                {
                    if (pair.strategy != null && !MovementStrategies.ContainsKey(pair.type))
                    {
                        MovementStrategies.Add(pair.type, pair.strategy);
                    }
                }
            }

            if (!debugText)
            {
                debugText = GetComponentInChildren<TMP_Text>();
            }
        }

        /// <summary>
        /// Utility method for states to call for rotation.
        /// </summary>
        public void HandleRotation(Vector2 direction)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // --- Event Invokers (Called by States) ---
        public void InvokeTargetSpotted() => OnTargetSpotted?.Invoke();
        public void InvokeTargetLost() => OnTargetLost?.Invoke();
        public void InvokeTargetInAttackRange() => OnTargetInAttackRange?.Invoke();
        public void InvokeTargetOutOfAttackRange() => OnTargetOutOfAttackRange?.Invoke();
        public void InvokeAttackPerformed() => OnAttackPerformed?.Invoke();
        public void InvokePatrolPointArrived() => OnPatrolPointArrived?.Invoke();
        public void InvokeIdleTimeFinished() => OnIdleTimeFinished?.Invoke();

    }
}

