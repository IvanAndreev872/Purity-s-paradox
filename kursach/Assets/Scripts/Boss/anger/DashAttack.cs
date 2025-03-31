using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    public Transform player;
    public MovementInterface movement_interface;

    public float dash_speed;
    public float critical_distance;
    public float dash_delay;
    public float braking_distance;
    public float damage;

    private float dash_time;
    private bool is_dashing = false;
    private Vector2 dash_target;
    

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
        if (movement_interface.able_to_move && !is_dashing
            && Vector2.Distance(transform.position, player.position) > critical_distance)
        {
            CheckDash();
        }
    }

    private void FixedUpdate()
    {
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
            Vector3 direction = (player.position - transform.position).normalized;
            dash_time = Time.time;
            dash_target = player.position + direction * braking_distance;
            is_dashing = true;
        }
    }

    void Dash()
    {
        if (Vector2.Distance(transform.position, dash_target) <= 0.1)
        {
            is_dashing = false;
            movement_interface.able_to_move = true;
        }

        rb.MovePosition(Vector3.Lerp(transform.position, dash_target, dash_speed * Time.fixedDeltaTime));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (is_dashing)
        {
            is_dashing = false;
            movement_interface.able_to_move = true;
            DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();
            if (enemy != null)
            {
                enemy.Hit(damage);
            }
        }
    }
}