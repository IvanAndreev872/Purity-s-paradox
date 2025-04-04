using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, DamageInterface
{
    public float maxHealthpoint;

    private float currentHealth;
    private bool damagable = true, isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealthpoint;
    }

    public void Hit(float damage)
    {
        Debug.Log(damagable + " " + damage);
        if (damagable)
        {
            if (currentHealth <= damage)
            {
                Die();
            }
            currentHealth -= damage;
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
}
