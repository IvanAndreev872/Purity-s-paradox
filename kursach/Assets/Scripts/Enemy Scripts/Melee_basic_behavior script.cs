using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Melee_basic_behaviorscript : MonoBehaviour
{
    [SerializeField] public float move_speed;
    [SerializeField] public float rotation_speed;
    [SerializeField] public float attack_range;
    [SerializeField] public GameObject target;

    private Rigidbody2D rb;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            Vector2 curPosition = rb.position;
            Vector2 tarPosition = target.transform.position;
            Vector2 direction = (Vector2)(tarPosition - curPosition);

            float distance = direction.magnitude;
            if (distance > attack_range)
            {
                Vector2 directionnorm = direction.normalized;
                rb.MovePosition(Vector3.Lerp(transform.position, curPosition + directionnorm, move_speed * Time.fixedDeltaTime));

                animator.SetFloat("MoveX", directionnorm.x);
                animator.SetFloat("MoveY", directionnorm.y);
            }
            else
            {
                rb.velocity = Vector2.zero;

                animator.SetFloat("MoveX", 0);
                animator.SetFloat("MoveY", 0);
            }

        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}
