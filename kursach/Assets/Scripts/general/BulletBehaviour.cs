using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float fireDistance;
    public float damage;

    private Vector3 spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, spawnPoint) >= fireDistance)
        { 
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log("BULLET");
        DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();
        if (enemy != null) 
        {
            enemy.Hit(damage);
        }
        Destroy(gameObject);
    }
}
