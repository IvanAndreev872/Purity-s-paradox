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

    void UpdateHealth(float damage)
    {
        PlayerStats playerStats = GetComponent<PlayerStats>();
        playerStats.health = Math.Max(0, playerStats.health - damage);
        playerStats.UpdateUI();
    }

    public void Hit(float damage)
    {
        if (damagable)
        {
            UpdateHealth(damage);
            if (current_health <= damage)
            {
                Die();
            }
            current_health -= damage;
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
