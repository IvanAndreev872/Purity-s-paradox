using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAttack : MonoBehaviour
{
    public Transform player;
    public MovementInterface movementInterface;

    public float dashSpeed;
    public float criticalDistance;
    public float dashDelay;
    public float breakingDistance;
    public float damage;

    private float dashTime;
    private bool isDashing = false;
    private Vector2 dashTarget;
    

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        movementInterface = GetComponent<MovementInterface>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementInterface.ableToMove && !isDashing
            && Vector2.Distance(transform.position, player.position) > criticalDistance)
        {
            CheckDash();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            Dash();
        }
    }

    void CheckDash()
    {
        if (Time.time > dashDelay + dashTime)
        {
            movementInterface.ableToMove = false;
            Vector3 direction = (player.position - transform.position).normalized;
            dashTime = Time.time;
            dashTarget = player.position + direction * breakingDistance;
            isDashing = true;
        }
    }

    void Dash()
    {
        if (Vector2.Distance(transform.position, dashTarget) <= 0.1)
        {
            isDashing = false;
            movementInterface.ableToMove = true;
        }

        rb.MovePosition(Vector3.Lerp(transform.position, dashTarget, dashSpeed * Time.fixedDeltaTime));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing)
        {
            isDashing = false;
            movementInterface.ableToMove = true;
            DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();
            if (enemy != null)
            {
                enemy.Hit(damage);
            }
        }
    }
}