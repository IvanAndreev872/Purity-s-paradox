using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonProjectile : MonoBehaviour
{
    public float fire_distance;
    public float damage;

    public int effect_attacks_count;
    public float effect_attacks_delay;
    public float poison_effect_damage;

    public GameObject poison_effect_prefab;

    private Vector3 spawn_point;
    // Start is called before the first frame update
    void Start()
    {
        spawn_point = transform.position;
    }


    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, spawn_point) >= fire_distance)
        { 
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();

        if (enemy != null) 
        {
            enemy.Hit(damage);

            GameObject poison_effect_object = Instantiate(poison_effect_prefab, transform.position, transform.rotation);
            ProjectileEffect poison_effect = poison_effect_object.GetComponent<ProjectileEffect>();
            poison_effect.ApplyEffect(effect_attacks_count, poison_effect_damage, effect_attacks_delay, enemy);
        }

        Destroy(gameObject);
    }


}
