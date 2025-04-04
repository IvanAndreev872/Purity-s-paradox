using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public float attackDelay;
    public float damage;
    public float existanceTime;
    public float radius;

    private float attackTime;

    private LayerMask playerLayer;

    void Start()
    {
        playerLayer = LayerMask.GetMask("Character");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > attackTime + attackDelay)
        {
            attackTime = Time.time;
            Attack();
        }
    }

    void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
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
