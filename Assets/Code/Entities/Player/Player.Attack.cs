using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player Attack Settings")]
    public GameObject projectilePrefab;
    private PlayerInput playerInput;
    private PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();

        // Every 0.1 seconds, call ShootProjectile
        InvokeRepeating("ShootProjectile", 0.1f, 0.1f);
    }

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Attack"].performed += OnAttack;
    }

    void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ShootProjectile();
        }
    }

    void ShootProjectile()
    {
        GameObject projectile = Instantiate(
            projectilePrefab, 
            transform.position, 
            Quaternion.identity
        );

        projectile.GetComponent<ProjectileBehavior>().Init(
            ProjectileOwnerType.Player, 
            playerStats.damage
        );

        // Find the closest enemy to aim at
        GameObject closestEnemy = null;
        closestEnemy = GameObject.FindWithTag("Enemy");

        // If there's an enemy, aim at it
        if(closestEnemy != null)
        {
            Vector2 direction = (closestEnemy.transform.position - transform.position).normalized;
            projectile.GetComponent<Rigidbody2D>().linearVelocity = direction * 10f;
            return;
        }
 
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(
            Mouse.current.position.ReadValue()
        );

        projectile.GetComponent<Rigidbody2D>().linearVelocity = 
            (mousePosition - (Vector2)transform.position).normalized * 10f;
    }
}
