using System;
using Code.Entities.Enemy.EnemyMovementStrategies;
using UnityEngine;

/// <summary>
/// Handles the movement of enemy entities using a specified movement strategy.
/// </summary>
/// <remarks>
/// This class utilizes a movement strategy pattern to define how enemies move
/// towards their targets. The movement strategy can be easily swapped out to change
/// the enemy's behavior without modifying the movement handler itself.
/// </remarks>
public class EnemyMovementHandler : EntityMovementHandler
{
    [Header("Enemy Movement Handler")]
    [Tooltip("Drag and drop the movement behavior strategy for this enemy.")]
    [SerializeField]
    private MovementStrategy movementStrategy;
    
    [Tooltip("The target the enemy will move towards.")]
    private GameObject target;

    /// <summary>
    /// Initializes the enemy movement handler and finds the player target.
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    /// <summary>
    /// Handles enemy movement towards the target using the specified movement strategy.
    /// </summary>
    private void FixedUpdate()
    {
        if (!movementStrategy || !target)
            return;

        movementStrategy.ExecuteMovement(
            transform,
            target.transform, 
            entityMovement);

        Vector2 targetDirection = (target.transform.position - transform.position).normalized;
        HandleRotation(targetDirection);
    }

    /// <summary>
    /// Handles the rotation of the enemy to face the given direction.
    /// </summary>
    /// <param name="direction"> The direction to face. </param>
    private void HandleRotation(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
