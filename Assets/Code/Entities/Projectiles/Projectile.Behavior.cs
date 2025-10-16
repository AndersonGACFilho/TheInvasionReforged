using UnityEngine;

public enum ProjectileOwnerType
{
    Player,
    Enemy
}
public class ProjectileBehavior : MonoBehaviour
{
    private ProjectileOwnerType ownerType;
    private int damage;

    [Header("Projectile Settings")]
    public float lifespan = 0.1f; // seconds

    public void Init(ProjectileOwnerType owner, int damageAmount)
    {
        ownerType = owner;
        damage = damageAmount;
        Destroy(gameObject, lifespan);
    }

   void Start()
   {
        
   }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(ownerType == ProjectileOwnerType.Player && other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyStats>().TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if(ownerType == ProjectileOwnerType.Enemy && other.CompareTag("Player"))
        {
            other.GetComponent<PlayerStats>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
