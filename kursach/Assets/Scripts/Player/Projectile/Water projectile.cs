using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : PlayerProjectile
{
    public float fireEnemyMultiplier;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();

        if (enemy != null) 
        {
            if (collision.gameObject.CompareTag("Fire enemy"))
            {
                enemy.Hit(damage * fireEnemyMultiplier);
            }
            else
            {
                enemy.Hit(damage);
            }
        }

        Destroy(gameObject);
    }
}
