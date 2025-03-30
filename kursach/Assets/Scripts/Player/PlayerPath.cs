using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPath : MonoBehaviour
{
    public float attack_delay;
    public float damage;
    public float existance_time;
    public float radius;

    private float attack_time;

    private LayerMask enemy_layer;

    void Start()
    {
        enemy_layer = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > attack_time + attack_delay)
        {
            attack_time = Time.time;
            Attack();
        }
    }

    void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, enemy_layer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            DamageInterface enemy = hitCollider.gameObject.GetComponent<DamageInterface>();
            if (enemy != null)
            {
                enemy.Hit(damage);
            }
        }
    }
}
