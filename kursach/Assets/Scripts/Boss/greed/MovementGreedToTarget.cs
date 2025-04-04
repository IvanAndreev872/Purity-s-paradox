using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementGreedToTarget : MonoBehaviour, MovementInterface
{
    public bool ableToMove { get; set; } = true;
    public Transform player;
    public float walkSpeed;

    private float walkSpeedNow;

    private bool speedChanged = false;
    private float speedChangeDuration;
    private float speedChangeTime;

    private Rigidbody2D rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Character").transform;
        }
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        walkSpeedNow = walkSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (speedChanged && Time.time > speedChangeDuration + speedChangeTime)
        {
            speedChanged = false;
            walkSpeedNow = walkSpeed;
        }

        if (ableToMove)
        {
            if (player)
            {
                MoveTowards(player);
            }
        }
    }

    public void ChangeSpeed(float coef, float time)
    {
        walkSpeedNow = walkSpeed * coef;
        speedChanged = true;
        speedChangeTime = Time.time;
        speedChangeDuration = time;
    }

    void MoveTowards(Transform target)
    {
        Vector3 direction = target.position - transform.position;

        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction.normalized, walkSpeedNow * Time.fixedDeltaTime));

        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }
}
