using System;
using UnityEngine;

/// <summary>
/// Interface for movable entities.
/// </summary>
/// <remarks>
/// Defines a contract for movement behavior in entities, allowing for consistent movement handling across
/// different types of game objects. Also, it is an implementation of the Interface Segregation Principle by
/// ensuring that classes that implement this interface are not forced to depend on methods they do not use.
/// </remarks>
public class EntityMovement : MonoBehaviour, IMovable
{
    [Tooltip("Reference to the EntityStats component.")]
    public EntityStats entityStats;
    
    [Tooltip("Reference to the Rigidbody2D component.")]
    private Rigidbody2D _rigidbody2D;

    /// <summary>
    /// Initializes the EntityMovement component.
    /// </summary>
    public void Awake()
    {
        entityStats = gameObject.GetComponent<EntityStats>();
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Moves the entity in the specified direction with an optional speed override.
    /// </summary>
    /// <param name="direction"> The direction to move in. </param>
    /// <param name="speedOverride"> Optional speed override. If null, the entity's default speed is used. </param>
    public void Move(Vector2 direction, float? speedOverride = null)
    {
        float speedValue = speedOverride ?? entityStats.speed;
        _rigidbody2D.linearVelocity = direction.normalized * speedValue;
    }

    /// <summary>
    /// Stops the entity's movement.
    /// </summary>
    public void Stop()
    {
        _rigidbody2D.linearVelocity = Vector2.zero;
    }
}
