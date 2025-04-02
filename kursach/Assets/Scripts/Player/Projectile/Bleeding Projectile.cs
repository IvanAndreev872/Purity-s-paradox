using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedingProjectile : PlayerProjectile
{
    public int effectAttacksCount;
    public float effectAttacksDelay;
    public float bloodEffectDamage;

    public GameObject bloodEffectPrefab;

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();
        if (enemy != null)
        {
            enemy.Hit(damage);
            if (collision.gameObject.CompareTag("Regular enemy"))
            {
                GameObject fireEffectObject = Instantiate(bloodEffectPrefab, transform.position, transform.rotation);
                ProjectileEffect fireEffect = fireEffectObject.GetComponent<ProjectileEffect>();
                fireEffect.ApplyEffect(effectAttacksCount, bloodEffectDamage, effectAttacksDelay, enemy);
            }
        }

        Destroy(gameObject);
    }
}
