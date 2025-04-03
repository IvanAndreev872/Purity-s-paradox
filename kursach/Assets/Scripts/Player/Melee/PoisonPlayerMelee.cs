using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPlayerMelee : PlayerMelee
{
    public int effectAttacksCount;
    public float effectAttacksDelay;
    public float poisonEffectDamage;

    public void UpdatePoisonedSword(PlayerStats playerStats)
    {
        isSwordEquipped = playerStats.isSwordEquipped > 0;
        radius = playerStats.swordRadius;
        damage = playerStats.swordDamage;
        slashDuration = playerStats.swordSlashDuration;
        attackDelay = playerStats.swordAttackDelay;
        effectAttacksCount = playerStats.swordEffectAttacksCount;
        effectAttacksDelay = playerStats.swordEffectAttacksDelay;
        poisonEffectDamage = playerStats.swordEffectDamage;
    }

    protected override void MakeEffect(Collider2D hitCollider)
    {
        DamageInterface enemy = hitCollider.gameObject.GetComponent<DamageInterface>();
        if (enemy != null)
        {
            enemy.Hit(damage);
            StartCoroutine(Effect(enemy));
        }
    }

    IEnumerator Effect(DamageInterface enemy)
    {
        MonoBehaviour enemyMonoBehaviour = enemy as MonoBehaviour;
        for (int i = 0; i < effectAttacksCount; i++)
        {
            if (enemy != null && enemyMonoBehaviour != null)
            {
                enemy.Hit(poisonEffectDamage);
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(effectAttacksDelay);
        }
    }
}
