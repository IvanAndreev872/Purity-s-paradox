using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAttack : MonoBehaviour
{
    public MovementInterface movement_interface;

    public float bounce_speed;
    public float bounce_delay;
    public float bounce_duration;
    public float damage;

    private Vector3 bounce_direction;
    private bool is_bouncing = false;
    private float bounce_time;

    public GameObject explosion_prefab;

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
        if (movement_interface.able_to_move && !is_bouncing)
        {
            CheckBounce();
        }
    }

    private void FixedUpdate()
    {
        if (is_bouncing)
        {
            Bounce();
        }
    }

    void CheckBounce()
    {
        if (Time.time > bounce_delay + bounce_time)
        {
            movement_interface.able_to_move = false;
            bounce_direction = Random.insideUnitCircle.normalized;
            bounce_time = Time.time;
            is_bouncing = true;
        }
    }

    void Bounce()
    {
        if (Time.time >= bounce_time + bounce_duration)
        {
            is_bouncing = false;
            movement_interface.able_to_move = true;
            Explosion();
        }
        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + bounce_direction, bounce_speed * Time.fixedDeltaTime));
    }

    void Explosion()
    {
        GameObject explosion = Instantiate(explosion_prefab, transform.position, transform.rotation);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (is_bouncing)
        {
            DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();
            if (enemy != null)
            {
                is_bouncing = false;
                movement_interface.able_to_move = true;
                enemy.Hit(damage);
                Explosion();
            } else
            {
                Vector2 normal_to_wall = collision.GetContact(0).normal;
                bounce_direction = Vector2.Reflect(bounce_direction, normal_to_wall);
            }
        }
    }
}
