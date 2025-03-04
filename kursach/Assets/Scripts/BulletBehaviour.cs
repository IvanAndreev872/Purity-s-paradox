using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float fire_distance;
    public float damage;

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
        Health health = collision.gameObject.GetComponent<Health>();
        if (health != null) 
        {
            health.Hit(damage);
        }
        Destroy(gameObject);
    }
}
