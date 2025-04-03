using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, DamageInterface
{
    public float max_healthpoint;

    private float current_health;
    private bool damagable = true;
    
    // Start is called before the first frame update
    void Start()
    {
        UpdateMaxHealth(GetComponent<PlayerStats>());
        current_health = max_healthpoint;
    }

    public void UpdateMaxHealth(PlayerStats playerStats)
    {
        max_healthpoint = playerStats.maxHealth;
    }

    void UpdateHealth()
    {
        PlayerStats playerStats = GetComponent<PlayerStats>();
        playerStats.health = Math.Max(0, current_health);
        playerStats.UpdateUI();
    }

    public void PartHeal(float part)
    {
        current_health = Math.Max(current_health, max_healthpoint * part);
        UpdateHealth();
    }

    public void Hit(float damage)
    {
        if (damagable)
        {
            current_health -= damage;
            UpdateHealth();
            if (current_health <= 0)
            {
                Die();
            }
        }
    }

    public void CanBeDamaged(bool is_damagable)
    {
        damagable = is_damagable;
    }

    private void Die()
    {
        ItemsLoader.Instance.SaveProgress(true);
        SceneManager.LoadScene("Hub");
    }
}
