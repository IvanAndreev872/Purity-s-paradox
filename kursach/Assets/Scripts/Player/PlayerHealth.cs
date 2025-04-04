using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, DamageInterface
{
    public float maxHealthpoint;

    private float currentHealth;
    private bool damagable = true;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateMaxHealth(GetComponent<PlayerStats>());
        currentHealth = maxHealthpoint;
    }

    public void UpdateMaxHealth(PlayerStats playerStats)
    {
        maxHealthpoint = playerStats.maxHealth;
    }

    void UpdateHealth()
    {
        PlayerStats playerStats = GetComponent<PlayerStats>();
        playerStats.health = Math.Max(0, currentHealth);
        playerStats.UpdateUI();
    }

    public void PartHeal(float part)
    {
        currentHealth = Math.Max(currentHealth, maxHealthpoint * part);
        UpdateHealth();
    }

    public void Hit(float damage)
    {
        if (damagable)
        {
            currentHealth -= damage;
            UpdateHealth();
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }

    public void CanBeDamaged(bool isDamagable)
    {
        damagable = isDamagable;
    }

    private void Die()
    {
        ItemsLoader.Instance.SaveProgress(true);
        SceneManager.LoadScene("Hub");
    }
}
