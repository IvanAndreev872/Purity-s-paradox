using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : MonoBehaviour
{
    public float fire_distance;
    public float damage;
    public float fire_enemy_multiplier;

    private Vector3 spawn_point;
    // Start is called before the first frame update
    void Start()
    {
        spawn_point = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, spawn_point) >= fire_distance)
        { 
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();

        if (enemy != null) 
        {
            if (collision.gameObject.CompareTag("Fire enemy"))
            {
                enemy.Hit(damage * fire_enemy_multiplier);
            }
            else
            {
                enemy.Hit(damage);
            }
        }

        Destroy(gameObject);
    }
}
