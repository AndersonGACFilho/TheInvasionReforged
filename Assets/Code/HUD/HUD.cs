using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    EntityStats playerStats;
    [Header("UI Bars")]
    public Slider healthBar;
    public Slider shieldBar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<EntityStats>();
        
        healthBar.maxValue = playerStats.maxHealth;
        healthBar.value = playerStats.health;
        
        shieldBar.maxValue = playerStats.maxShield;
        shieldBar.value = playerStats.shield;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = playerStats.health;
        shieldBar.value = playerStats.shield;
    }
}
