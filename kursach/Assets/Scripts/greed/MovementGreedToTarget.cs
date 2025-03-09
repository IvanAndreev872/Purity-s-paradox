using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MovementGreedToTarget : MonoBehaviour, MovementInterface
{
    public bool able_to_move { get; set; } = true;
    public Transform player;
    public float speed;

    private Rigidbody2D rb;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (able_to_move)
        {
            if (player)
            {
                MoveTowards(player);
            }
        }
    }

    void MoveTowards(Transform target)
    {
        Vector3 direction = target.position - transform.position;

        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction.normalized, speed * Time.fixedDeltaTime));

        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }
}
