using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class dodge : MonoBehaviour
{
    public MovementInterface movement_interface;
    public float dodge_speed;

    private bool is_dodging = false;
    private Vector3 dodge_target;
    private Rigidbody2D rb;
    private Collider2D enemy_collider;

    // Start is called before the first frame update
    void Start()
    {
        movement_interface = transform.parent.GetComponent<MovementInterface>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        enemy_collider = rb.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (is_dodging) 
        {
            rb.MovePosition(Vector3.Lerp(transform.position, dodge_target, dodge_speed * Time.fixedDeltaTime));
            if (Vector2.Distance(transform.position, dodge_target) <= 0.1)
            {
                is_dodging = false;
                movement_interface.able_to_move = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (movement_interface.able_to_move && other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            if (movement_interface != null) 
            {
                Vector2 bullet_velocity = other.attachedRigidbody.velocity;
                Vector2 bullet_to_enemy = (Vector2)transform.position - other.attachedRigidbody.position;

                if (Vector2.Dot(bullet_to_enemy, bullet_velocity) > 0)
                {
                    if (Vector3.Cross(bullet_to_enemy, bullet_velocity.normalized).magnitude < (enemy_collider.bounds.extents.magnitude + other.bounds.extents.magnitude) * 1.1f)
                    {
                        is_dodging = true;
                        movement_interface.able_to_move = false;
                        float dodge_distance = other.bounds.size.magnitude + enemy_collider.bounds.extents.magnitude;
                        Vector3 direction = Vector2.Perpendicular(other.attachedRigidbody.velocity).normalized;
                        if (Vector2.Dot(direction, bullet_to_enemy) < 0)
                        {
                            direction *= -1;
                        }
                        dodge_target = transform.position + direction.normalized * dodge_distance;
                    }
                }
            }
        }
    }
}   
