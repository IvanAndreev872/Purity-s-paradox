using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, DamageInterface
{
    public float max_healthpoint;

    private float current_health;
    // Start is called before the first frame update
    void Start()
    {
        current_health = max_healthpoint;
    }

    public void Hit(float damage)
    {
        if (current_health <= damage)
        {
            Die();
        }
        current_health -= damage;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
