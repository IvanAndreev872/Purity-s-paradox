using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreezeProjectile : PlayerProjectile
{

    public float slowdownCoeffitient;
    public float slowdownTime;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();
        MovementInterface enemyMovement = collision.gameObject.GetComponent<MovementInterface>();

        if (enemy != null) 
        {
            enemy.Hit(damage);
            enemyMovement.ChangeSpeed(slowdownCoeffitient, slowdownTime);
        }

        Destroy(gameObject);
    }

    public void UpdateFreezeBullet(PlayerStats playerStats)
    {
        fireDistance = playerStats.staffRange;
        damage = playerStats.staffDamage;
        slowdownCoeffitient = playerStats.staffSlowdownCoeffitient;
        slowdownTime = playerStats.staffSlowdownTime;
    }
}
