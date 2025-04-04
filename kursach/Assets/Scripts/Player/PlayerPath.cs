using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPath : MonoBehaviour
{
    public float attackDelay;
    public float damage;
    public float existanceTime;
    public float radius;

    private float attackTime;
    private float creationTime;

    private LayerMask enemyLayer;

    void Start()
    {
        enemyLayer = LayerMask.GetMask("Enemy");
        creationTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > attackTime + attackDelay)
        {
            attackTime = Time.time;
            Attack();
        }

        if (Time.time > creationTime + existanceTime)
        {
            Destroy(gameObject);
        }
    }

    void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            DamageInterface enemy = hitCollider.gameObject.GetComponent<DamageInterface>();
            if (enemy != null)
            {
                enemy.Hit(damage);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
