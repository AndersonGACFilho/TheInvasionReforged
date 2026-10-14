using System;
using UnityEngine;

namespace Locomotion
{
    /// <summary>
    /// Interface for movable entities.
    /// </summary>
    /// <remarks>
    /// Defines a contract for movement behavior in entities, allowing for consistent movement handling across
    /// different types of game objects. Also, it is an implementation of the Interface Segregation Principle by
    /// ensuring that classes that implement this interface are not forced to depend on methods they do not use.
    /// </remarks>
    [RequireComponent(typeof(EntityStats), typeof(Rigidbody2D))]
    public class EntityMovement : MonoBehaviour, IMovable
    {
        [Tooltip("Reference to the EntityStats component.")]
        public EntityStats entityStats;

        [Tooltip("Reference to the Rigidbody2D component.")]
        private Rigidbody2D _rigidbody2D;
        
        [Tooltip("Reference to the entities animator component.")]
        private Animator _animator;
        
        [Tooltip("The flag that the entity is moving.")]
        private bool _isMoving = false;

        /// <summary>
        /// Initializes the EntityMovement component.
        /// </summary>
        public void Awake()
        {
            entityStats = gameObject.GetComponent<EntityStats>();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _animator = gameObject.GetComponent<Animator>();
        }

        /// <summary>
        /// Moves the entity in the specified direction with an optional speed override.
        /// </summary>
        /// <param name="direction"> The direction to move in. </param>
        /// <param name="speedOverride"> Optional speed override. If null, the entity's default speed is used. </param>
        public void Move(Vector2 direction, float? speedOverride = null)
        {
            if (direction.sqrMagnitude < 0.0001f)
            {
                Stop();
                return;
            }
            
            var initialSpeed = speedOverride ?? entityStats.speed;

            if (!_isMoving)
            {
                entityStats.currentMovementSpeed = initialSpeed;
                _isMoving = true;
            }

            entityStats.SetMovementSpeed(
                Mathf.MoveTowards(
                    entityStats.currentMovementSpeed,
                    entityStats.maxSpeed,
                    entityStats.acceleration * Time.fixedDeltaTime
                )
            );
            
            _rigidbody2D.linearVelocity = direction.normalized * 
                                    entityStats.currentMovementSpeed;
            
            if (_animator)
                _animator.SetFloat("Speed", entityStats.currentMovementSpeed);
        }

        /// <summary>
        /// Stops the entity's movement.
        /// </summary>
        public void Stop()
        {
            _isMoving = false;
            entityStats.currentMovementSpeed = 0;
            
            _rigidbody2D.linearVelocity = Vector2.zero;
            if (_animator)
                _animator.SetFloat("Speed", 0);
        }
    }
}
