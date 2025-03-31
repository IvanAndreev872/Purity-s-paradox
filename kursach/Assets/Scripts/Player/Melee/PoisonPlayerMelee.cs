using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonPlayerMelee : MonoBehaviour
{
    public float radius;
    public float damage;

    public int effect_attacks_count;
    public float effect_attacks_delay;
    public float poison_effect_damage;

    private LayerMask enemy_layer;
    // Start is called before the first frame update
    void Start()
    {
        enemy_layer = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckAttack())
        {
            Attack();
        }
    }

    void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, enemy_layer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            DamageInterface enemy = hitCollider.gameObject.GetComponent<DamageInterface>();
            if (enemy != null)
            {
                enemy.Hit(damage);
                StartCoroutine(Effect(enemy));
            }
        }
    }

    bool CheckAttack()
    {
        return Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(1);
    }

    private IEnumerator Effect(DamageInterface enemy)
    {
        MonoBehaviour enemy_mono_behaviour = enemy as MonoBehaviour;
        for (int i = 0; i < effect_attacks_count; i++)
        {
            if (enemy != null && enemy_mono_behaviour != null)
            {
                enemy.Hit(poison_effect_damage);
            }
            else
            {
                break;
            }

            yield return new WaitForSeconds(effect_attacks_delay);
        }
    }
}
