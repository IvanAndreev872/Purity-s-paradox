using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEffect : MonoBehaviour
{
    private int attackCount;
    private float attackDelay;
    private float damage;
    private DamageInterface enemy;
    private MonoBehaviour enemyMonoBehaviour;
    public void ApplyEffect(int effectAttacksCount, float effectDamage, float effectAttacksDelay, DamageInterface damagableEnemy)
    {
        attackCount = effectAttacksCount;
        damage = effectDamage;
        attackDelay = effectAttacksDelay;
        enemy = damagableEnemy;
        enemyMonoBehaviour = damagableEnemy as MonoBehaviour;
        StartCoroutine("Effect");
    }

    private IEnumerator Effect()
    {
        for (int i = 0; i < attackCount; i++)
        {
            if (enemy != null && enemyMonoBehaviour != null)
            {
                enemy.Hit(damage);
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(attackDelay);
        }

        Destroy(gameObject);
    }
}
