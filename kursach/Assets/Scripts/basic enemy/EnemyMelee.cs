using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public float radius;
    public float damage;
    public float attack_delay;
    public Transform player;

    private float attack_time;
    private DamageInterface attack_player;
    // Start is called before the first frame update
    void Start()
    {
        attack_time = Time.time;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Character").transform;

        }
        attack_player = player.gameObject.GetComponent<DamageInterface>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > attack_time + attack_delay)
        {
            Attack();
        }
    }

    void Attack()
    {
        if (Vector2.Distance(transform.position, player.position) < radius)
        {
            attack_player.Hit(damage);
        }
        attack_time = Time.time;
    }
}
