using UnityEngine;
public class EntityMovement : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    
    protected void Move(Vector2 direction, float? speedOverride = null)
    {
        float speedValue = speedOverride ?? this.speed;
        
        // If both directions of the vector are different than 0, normalize the vector
        if(direction.x != 0 && direction.y != 0)
            speedValue = speedValue*0.66f;
        
        gameObject.GetComponent<Rigidbody2D>().AddForce(
            direction.normalized * speedValue, 
            ForceMode2D.Force
        );
    }
}
