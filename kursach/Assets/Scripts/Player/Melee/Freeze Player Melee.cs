using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezePlayerMelee : MonoBehaviour
{
    public float radius;
    public float damage;

    public float slowdown_coeffitient;
    public float slowdow_time;

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
            MovementInterface enemy_movement = hitCollider.gameObject.GetComponent<MovementInterface>();
            if (enemy != null)
            {
                enemy.Hit(damage);
                enemy_movement.ChangeSpeed(slowdown_coeffitient, slowdow_time);
            }
        }
    }

    bool CheckAttack()
    {
        return Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(1);
    }
}
