using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : MonoBehaviour
{
    public Transform player;
    public MovementInterface movement_interface;

    public GameObject shadow_prefab;
    public float attack_distance;
    public float ability_duration;
    public float damage;
    public float jump_delay;
    public float fly_speed;
    public float stop_time;

    private GameObject shadow;
    private bool is_jumping = false;
    private float jump_time;

    private Rigidbody2D rb;
    private SpriteRenderer boss_renderer;
    private Collider2D boss_collider;
    // Start is called before the first frame update
    void Start()
    {
        movement_interface = GetComponent<MovementInterface>();
        rb = GetComponent<Rigidbody2D>();
        boss_collider = GetComponent<Collider2D>();
        boss_renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movement_interface.able_to_move && !is_jumping)
        {
            CheckJump();
        }
    }

    private void FixedUpdate()
    {
        if (is_jumping)
        {
            ShadowTransparence();
            if (Time.time < jump_time + stop_time)
            {
                MoveTowards(player.transform.position);
            }
            else if (Time.time >= jump_time + ability_duration)
            {
                FinishAttack();
            }
        }
    }

    void CheckJump()
    {
        if (Time.time > jump_delay + jump_time)
        {
            movement_interface.able_to_move = false;
            jump_time = Time.time;
            is_jumping = true;

            boss_renderer.enabled = false;
            boss_collider.enabled = false;

            shadow = Instantiate(shadow_prefab, transform.position, transform.rotation, transform);
        }
    }

    void ShadowTransparence()
    {
        if (shadow != null)
        {
            SpriteRenderer shadow_renderer = shadow.GetComponent<SpriteRenderer>();
            if (shadow_renderer != null)
            {
                float attack_progress = Mathf.Clamp01((Time.time - jump_time) / ability_duration);
                Color shadow_color = shadow_renderer.color;
                shadow_color.a = attack_progress;
                shadow_renderer.color = shadow_color;
            }
        }
    }

    void FinishAttack()
    {
        boss_collider.enabled = true;
        boss_renderer.enabled = true;

        movement_interface.able_to_move = true;
        is_jumping = false;
        Destroy(shadow);

        if (Vector2.Distance(player.transform.position, transform.position) <= attack_distance)
        {
            DamageInterface enemy = player.gameObject.GetComponent<DamageInterface>();
            if (enemy != null)
            {
                enemy.Hit(damage);
            }
        }
    }

    void MoveTowards(Vector3 target)
    {
        if (Vector2.Distance(transform.position, target) >= 0.1)
        {
            Vector3 direction = (target - transform.position).normalized;
            rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction, fly_speed * Time.fixedDeltaTime));
        }
    }
}
