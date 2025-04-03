using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AngerStatusController;

public class JumpAttack : MonoBehaviour
{
    private AngerStatusController anger_status_controller;

    public Transform player;
    public MovementInterface movement_interface;

    public GameObject jumpEffect;
    public GameObject shadow_prefab;
    public GameObject explosion_prefab;
    public float attack_distance;
    public float ability_duration;
    public float damage;
    public float jump_delay;
    public float fly_speed;
    public float stop_time;

    private GameObject shadow;
    private bool is_jumping = false;
    private float jump_time;

    private EnemyMelee melee;
    private Rigidbody2D rb;
    private SpriteRenderer boss_renderer;
    private Collider2D boss_collider;

    private bool can_be_enabled = false;

    private void Awake()
    {
        anger_status_controller = GetComponentInParent<AngerStatusController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        movement_interface = GetComponent<MovementInterface>();
        rb = GetComponent<Rigidbody2D>();
        boss_collider = GetComponent<Collider2D>();
        boss_renderer = GetComponent<SpriteRenderer>();
        Debug.Log(1);
        melee = GetComponent<EnemyMelee>();
        Debug.Log(melee);
    }

    // Update is called once per frame
    void Update()
    {
        if (movement_interface.able_to_move && !is_jumping && can_be_enabled)
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
            melee.enabled = false;
            movement_interface.able_to_move = false;
            jump_time = Time.time;
            is_jumping = true;

            boss_renderer.enabled = false;
            boss_collider.enabled = false;

            Instantiate(jumpEffect, transform.position, transform.rotation);
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
        melee.enabled = true;

        movement_interface.able_to_move = true;
        is_jumping = false;
        Destroy(shadow);

        Instantiate(explosion_prefab, transform.position, transform.rotation);
    }

    void MoveTowards(Vector3 target)
    {
        if (Vector2.Distance(transform.position, target) >= 0.1)
        {
            Vector3 direction = (target - transform.position).normalized;
            rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction, fly_speed * Time.fixedDeltaTime));
        }
    }

    private void OnAngerChanged(AngerStatusController.AngerLevel new_level)
    {
        if (new_level == AngerLevel.Enraged)
        {
            can_be_enabled = true;
        }
    }

    void OnEnable()
    {
        if (anger_status_controller)
        {
            anger_status_controller.EnragementChanged += OnAngerChanged;
        }
    }

    void OnDisable()
    {
        if (anger_status_controller)
        {
            anger_status_controller.EnragementChanged -= OnAngerChanged;
        }
    }
}
