using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerController : MonoBehaviour, MovementInterface
{
    public bool able_to_move { get; set; } = true;
    public Transform player;
    public float walk_speed;

    private float walk_speed_now;

    private bool speed_changed = false;
    private float speed_change_duration;
    private float speed_change_time;

    private Rigidbody2D rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        walk_speed_now = walk_speed;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (speed_changed && Time.time > speed_change_duration + speed_change_time)
        {
            speed_changed = false;
            walk_speed_now = walk_speed;
        }

        if (able_to_move)
        {
            if (player)
            {
                MoveTowards(player);
            }
        }
    }

    public void ChangeSpeed(float coef, float time)
    {
        walk_speed_now = walk_speed * coef;
        speed_changed = true;
        speed_change_time = Time.time;
        speed_change_duration = time;
    }

    void MoveTowards(Transform target)
    {
        Vector3 direction = target.position - transform.position;

        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction.normalized, walk_speed_now * Time.fixedDeltaTime));

        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }
}
