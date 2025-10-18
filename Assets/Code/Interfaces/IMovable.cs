using UnityEngine;
/// <summary>
/// Interface for movable entities
/// </summary>
/// <remarks>
/// Defines a contract for movement behavior in entities, allowing for consistent movement handling across
/// different types of game objects. Also, it is an implementation of the Interface Segregation Principle by
/// ensuring that classes that implement this interface are not forced to depend on methods they do not use.
/// </remarks>
public interface IMovable
{
    /// <summary>
    /// Moves the entity in the specified direction, with an optional speed override.
    /// </summary>
    /// <param name="direction"> The direction to move in. </param>
    /// <param name="speedOverride"> Optional speed override. If null, the entity's default speed is used. </param>
    public void Move(Vector2 direction, float? speedOverride = null);
}