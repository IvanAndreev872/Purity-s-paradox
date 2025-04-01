using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayerMelee : PlayerMelee
{
    public float slowdownCoeffitient;
    public float slowdownTime;

    protected override void MakeEffect(Collider2D hitCollider)
    {
        DamageInterface enemy = hitCollider.gameObject.GetComponent<DamageInterface>();
        MovementInterface enemyMovement = hitCollider.gameObject.GetComponent<MovementInterface>();
        if (enemy != null && enemyMovement != null)
        {
            enemy.Hit(damage);
            enemyMovement.ChangeSpeed(slowdownCoeffitient, slowdownTime);
        }
    }
}
