using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, DamageInterface
{
    public float maxHealthpoint;

    protected float currentHealth;
    protected bool damagable = true, isDead = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHealthpoint;
    }

    public virtual void Hit(float damage)
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

    protected void Die()
    {
        if (!isDead)
        {
            GetComponent<RewardAfterDeath>().GetReward(transform.position);
        }
        isDead = true;
        Destroy(gameObject);
    }
}
