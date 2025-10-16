using UnityEngine;
public class EntityStats : MonoBehaviour
{
    [Header("Stats")]
    public int health = 100;
    public int maxHealth = 100;
    public int shield = 50;
    public int maxShield = 50;
    public int damage = 20;
    
    public void TakeDamage(int amount)
    {
        int remainingDamage = amount;
        if (shield > 0)
        {
            if (shield >= remainingDamage)
            {
                shield -= remainingDamage;
                remainingDamage = 0;
            }
            else
            {
                remainingDamage -= shield;
                shield = 0;
            }
        }
        if (remainingDamage > 0)
        {
            health -= remainingDamage;
            if (health < 0) health = 0;
        }
        
        if (health == 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        Destroy(gameObject);
    }
}