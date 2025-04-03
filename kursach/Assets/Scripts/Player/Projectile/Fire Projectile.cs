using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : PlayerProjectile
{
    public int effectAttacksCount;
    public float effectAttacksDelay;
    public float fireEffectDamage;

    public GameObject fireEffectPrefab;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();
        if (enemy != null)
        {
            enemy.Hit(damage);
            if (!collision.gameObject.CompareTag("Fire enemy"))
            {
                GameObject fireEffectObject = Instantiate(fireEffectPrefab, transform.position, transform.rotation);
                ProjectileEffect fireEffect = fireEffectObject.GetComponent<ProjectileEffect>();
                fireEffect.ApplyEffect(effectAttacksCount, fireEffectDamage, effectAttacksDelay, enemy);
            }
        }

        Destroy(gameObject);
    }

    public void UpdateFireBullet(PlayerStats playerStats)
    {
        fireDistance = playerStats.staffRange;
        damage = playerStats.staffDamage;
        effectAttacksCount = playerStats.staffEffectAttacksCount;
        effectAttacksDelay = playerStats.staffEffectAttacksDelay;
        fireEffectDamage = playerStats.staffEffectDamage;
    }
}
