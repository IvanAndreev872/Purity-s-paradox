using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerProjectile : MonoBehaviour
{
    public float fireDistance;
    public float damage;

    private Vector3 spawnPoint;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        spawnPoint = transform.position;
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        if (Vector3.Distance(transform.position, spawnPoint) >= fireDistance)
        {
            Destroy(gameObject);
        }
    }

    protected abstract void OnCollisionEnter2D(Collision2D collision);
}
