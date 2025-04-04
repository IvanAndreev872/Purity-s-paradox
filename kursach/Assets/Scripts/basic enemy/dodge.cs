using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class dodge : MonoBehaviour
{
    public MovementInterface movementInterface;
    public float dodgeSpeed;

    private bool isDodging = false;
    private Vector3 dodgeTarget;
    private Rigidbody2D rb;
    private Collider2D enemyCollider;

    // Start is called before the first frame update
    void Start()
    {
        movementInterface = transform.parent.GetComponent<MovementInterface>();
        rb = transform.parent.GetComponent<Rigidbody2D>();
        enemyCollider = rb.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDodging) 
        {
            rb.MovePosition(Vector3.Lerp(transform.position, dodgeTarget, dodgeSpeed * Time.fixedDeltaTime));
            if (Vector2.Distance(transform.position, dodgeTarget) <= 0.1)
            {
                isDodging = false;
                movementInterface.ableToMove = true;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (movementInterface.ableToMove && other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            if (movementInterface != null) 
            {
                Vector2 bulletVelocity = other.attachedRigidbody.velocity;
                Vector2 bulletToEnemy = (Vector2)transform.position - other.attachedRigidbody.position;

                if (Vector2.Dot(bulletToEnemy, bulletVelocity) > 0)
                {
                    if (Vector3.Cross(bulletToEnemy, bulletVelocity.normalized).magnitude < (enemyCollider.bounds.extents.magnitude + other.bounds.extents.magnitude) * 1.1f)
                    {
                        isDodging = true;
                        movementInterface.ableToMove = false;
                        float dodgeDistance = other.bounds.size.magnitude + enemyCollider.bounds.extents.magnitude;
                        Vector3 direction = Vector2.Perpendicular(other.attachedRigidbody.velocity).normalized;
                        if (Vector2.Dot(direction, bulletToEnemy) < 0)
                        {
                            direction *= -1;
                        }
                        dodgeTarget = transform.position + direction.normalized * dodgeDistance;
                    }
                }
            }
        }
    }
}   
