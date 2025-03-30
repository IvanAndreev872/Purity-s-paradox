using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEffect : MonoBehaviour
{
    private int attack_count;
    private float attack_delay;
    private float damage;
    private DamageInterface enemy;
    private MonoBehaviour enemy_mono_behaviour;
    public void ApplyEffect(int effect_attacks_count, float effect_damage, float effect_attacks_delay, DamageInterface damagable_enemy)
    {
        attack_count = effect_attacks_count;
        damage = effect_damage;
        attack_delay = effect_attacks_delay;
        enemy = damagable_enemy;
        enemy_mono_behaviour = damagable_enemy as MonoBehaviour;
        StartCoroutine("Effect");
    }

    private IEnumerator Effect()
    {
        for (int i = 0; i < attack_count; i++)
        {
            if (enemy != null && enemy_mono_behaviour != null)
            {
                enemy.Hit(damage);
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(attack_delay);
        }

        Destroy(gameObject);
    }
}
