using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    EntityMovement movement;
    EnemyStats entity_stats;
    GameObject player;
    
    void Awake()
    {
        entity_stats = GetComponent<EnemyStats>();
        movement = GetComponent<EntityMovement>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        FollowPlayer();
    }
    
    void FollowPlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position, 
            player.transform.position,
            movement.speed * Time.deltaTime
        );
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().TakeDamage(entity_stats.damage);
        }
    }
}
