using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AngerStatusController : MonoBehaviour, DamageInterface
{
    public enum AngerLevel { Calm, Raged, Enraged }
    public AngerLevel currentEnragement { get; private set; }

    public delegate void OnAngerChanged(AngerLevel newLevel);
    public event OnAngerChanged EnragementChanged;

    public float maxHealthpoint;
    public float ragedAfterHealthpoint;
    public float enragedAfterHealthpoint;

    private float currentHealth;

    public Slider slider;

    private bool damagable = true, isDead = false;
    // Start is called before the first frame update�
    void Awake()
    {
        currentHealth = maxHealthpoint;
        currentEnragement = AngerLevel.Calm;
        slider.value = 1;
    }

    public void Hit(float damage)
    {
        if (damagable)
        {
            Debug.Log(damage);
            currentHealth -= damage;
            UpdateHealthBar();
            CheckHealth();
        }
    }

    private void CheckHealth()
    {
        if (currentHealth < 0) 
        {
            slider.gameObject.SetActive(false);
            Die();
        } 
        else
        {
            AngerLevel newEnragementLevel = currentEnragement;
            if (currentHealth <= enragedAfterHealthpoint)
            {
                Debug.Log(1);
                newEnragementLevel = AngerLevel.Enraged;
            }
            else if (currentHealth <= ragedAfterHealthpoint)
            {
                Debug.Log(0);
                newEnragementLevel = AngerLevel.Raged;
            }

            if (currentEnragement != newEnragementLevel)
            {
                Debug.Log(newEnragementLevel);
                currentEnragement = newEnragementLevel;
                EnragementChanged?.Invoke(newEnragementLevel);
            }
        }
    }

    public void CanBeDamaged(bool isDamagable)
    {
        damagable = isDamagable;
    }

    private void Die()
    {
        if (!isDead)
        {
            GetComponent<RewardAfterDeath>().GetReward(transform.position);
        }
        isDead = true;
        Destroy(gameObject);
    }

    private void UpdateHealthBar()
    {
        slider.value = Math.Max(0, (float)currentHealth / maxHealthpoint);
    }
}
