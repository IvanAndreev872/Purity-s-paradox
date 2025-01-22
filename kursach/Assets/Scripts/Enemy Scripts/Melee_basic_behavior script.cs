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
    void Start()
    {
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
                rb.MovePosition(curPosition + directionnorm * move_speed * Time.fixedDeltaTime);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            rb.velocity += Vector2.zero;
        }
    }
}
