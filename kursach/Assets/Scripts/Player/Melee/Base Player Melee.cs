using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerMelee : PlayerMelee
{
    public void UpdateBaseSword(PlayerStats playerStats)
    {
        radius = playerStats.swordRadius;
        damage = playerStats.swordDamage;
        slashDuration = playerStats.swordSlashDuration;
        attackDelay = playerStats.swordAttackDelay;
    }

    protected override void MakeEffect(Collider2D hitCollider)
    {
        DamageInterface enemy = hitCollider.gameObject.GetComponent<DamageInterface>();
        if (enemy != null)
        {
            enemy.Hit(damage);
        }
    }
}
