using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeProjectile : MonoBehaviour
{
    public float fire_distance;
    public float damage;

    public float slowdown_coeffitient;
    public float slowdow_time;

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
        MovementInterface enemy_movement = collision.gameObject.GetComponent<MovementInterface>();

        if (enemy != null) 
        {
            enemy.Hit(damage);
            enemy_movement.ChangeSpeed(slowdown_coeffitient, slowdow_time);
        }

        Destroy(gameObject);
    }
}
