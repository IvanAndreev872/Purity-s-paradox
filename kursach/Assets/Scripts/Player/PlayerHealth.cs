using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, DamageInterface
{
    public float max_healthpoint;

    private float current_health;
    private bool damagable = true;
    
    // Start is called before the first frame update
    void Start()
    {
        current_health = max_healthpoint;
    }

    public void Hit(float damage)
    {
        if (damagable)
        {
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
        Destroy(gameObject);
    }
}
