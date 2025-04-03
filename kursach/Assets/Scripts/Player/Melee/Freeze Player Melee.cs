using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayerMelee : PlayerMelee
{
    public float slowdownCoeffitient;
    public float slowdownTime;

    public void UpdateFreezeSword(PlayerStats playerStats)
    {
        isSwordEquipped = playerStats.isSwordEquipped > 0;
        radius = playerStats.swordRadius;
        damage = playerStats.swordDamage;
        slashDuration = playerStats.swordSlashDuration;
        attackDelay = playerStats.swordAttackDelay;
        slowdownCoeffitient = playerStats.swordSlowdownCoeffitient;
        slowdownTime = playerStats.swordSlowdownTime;
    }

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
