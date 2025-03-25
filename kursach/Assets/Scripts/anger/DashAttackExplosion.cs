using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttackExplosion : MonoBehaviour
{
    public Transform player;
    public MovementInterface movement_interface;

    public float dash_speed;
    public float make_distance;
    public float dash_delay;
    public float braking_distance;
    public float damage;

    public GameObject explosion_prefab;

    public float path_duration;
    public GameObject path_prefab;

    private float dash_time;
    private bool is_dashing = false;
    private bool is_preparing = false;
    private Vector2 dash_target;
    private Vector2 retreat_target;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        movement_interface = GetComponent<MovementInterface>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movement_interface.able_to_move && !is_dashing && !is_preparing)
        {
            CheckDash();
        }
    }

    private void FixedUpdate()
    {
        if (is_preparing)
        {
            Retreat();
        }
        if (is_dashing)
        {
            Dash();
        }
    }

    void CheckDash()
    {
        if (Time.time > dash_delay + dash_time)
        {
            movement_interface.able_to_move = false;
            Vector3 direction_opposite = (transform.position - player.position).normalized;
            dash_time = Time.time;
            retreat_target = player.position + direction_opposite * make_distance;
            is_preparing = true;
        }
    }

    void Retreat()
    {
        if (Vector2.Distance(transform.position, retreat_target) <= 0.1)
        {
            is_preparing = false;
            is_dashing = true;

            Vector3 direction = (player.position - transform.position).normalized;
            dash_target = player.position + direction * braking_distance;
        }

        MoveTowards(retreat_target);
    }

    void Dash()
    {
        if (Vector2.Distance(transform.position, dash_target) <= 0.1)
        {
            is_dashing = false;
            movement_interface.able_to_move = true;
            Explosion();
        }

        MakePath();

        MoveTowards(dash_target);
    }

    void MoveTowards(Vector3 target) {
        Vector3 direction = (target - transform.position).normalized;
        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction, dash_speed * Time.fixedDeltaTime));
    }

    void MakePath()
    {
        GameObject path_part = Instantiate(path_prefab, transform.position, transform.rotation);

        Destroy(path_part, path_duration);
    }

    void Explosion()
    {
        GameObject explosion = Instantiate(explosion_prefab, transform.position, transform.rotation);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (is_preparing)
        {
            is_preparing = false;
            is_dashing = true;

            Vector3 direction = (player.position - transform.position).normalized;
            dash_target = player.position + direction * braking_distance;
        }
        else if (is_dashing)
        {
            is_dashing = false;
            movement_interface.able_to_move = true;
            DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();
            if (enemy != null)
            {
                enemy.Hit(damage);
            }
            Explosion();
        }
    }
}
    