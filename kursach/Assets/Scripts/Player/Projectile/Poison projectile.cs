using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonProjectile : PlayerProjectile
{
    public int effectAttacksCount;
    public float effectAttacksDelay;
    public float poisonEffectDamage;

    public GameObject poisonEffectPrefab;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();

        if (enemy != null) 
        {
            enemy.Hit(damage);

            GameObject poisonEffectObject = Instantiate(poisonEffectPrefab, transform.position, transform.rotation);
            ProjectileEffect poisonEffect = poisonEffectObject.GetComponent<ProjectileEffect>();
            poisonEffect.ApplyEffect(effectAttacksCount, poisonEffectDamage, effectAttacksDelay, enemy);
        }

        Destroy(gameObject);
    }

    public void UpdatePoisonBullet(PlayerStats playerStats)
    {
        fireDistance = playerStats.staffRange;
        damage = playerStats.staffDamage;
        effectAttacksCount = playerStats.staffEffectAttacksCount;
        effectAttacksDelay = playerStats.staffEffectAttacksDelay;
        poisonEffectDamage = playerStats.staffEffectDamage;
    }
}
