using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerFirePath : MonoBehaviour
{
    public float attack_delay;
    public float damage;
    public float existance_time;
    public float radius;

    private float attack_time;
    private float creation_time;

    private LayerMask player_layer;

    void Start()
    {
        player_layer = LayerMask.GetMask("Character");
        creation_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > attack_time + attack_delay)
        {
            attack_time = Time.time;
            Attack();
        }

        if (Time.time > creation_time + existance_time)
        {
            Destroy(gameObject);
        }
    }

    void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, player_layer);
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
