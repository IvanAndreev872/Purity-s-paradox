using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walk_speed;
    public float dash_speed;
    public float dash_duration;
    public float rotation_speed;
    public GameObject shooter;

    public PlayerStats playerStats;
    private Rigidbody2D rb;
    private bool is_dashing;
    private float dash_start;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();   
        playerStats = GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (is_dashing)
        {
            Dash();
        }
        else
        {
            Walk();
        }
    }

    void Update()
    {
        if (!is_dashing)
        {
            CheckDash();
        }
    }

    private void Walk()
    {
        Move(walk_speed * playerStats.walkAccelerateCoefficient);
    }

    private void Dash()
    {
        if (Time.time > dash_start + dash_duration) 
        { 
            is_dashing = false;
        }
        else
        {
            Move(dash_speed * playerStats.dashAccelerateCoefficient);
        }
    }

    private void CheckDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            is_dashing = true;
            dash_start = Time.time;
        }
    }

    private void Move(float speed)
    {
        float move_x = Input.GetAxis("Horizontal");
        float move_y = Input.GetAxis("Vertical");

        Vector3 direction = new(move_x * speed, move_y * speed);

        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction.normalized, speed * Time.fixedDeltaTime));

        RotateCharacter(move_x, move_y);
    }

    private void RotateCharacter(float move_x, float move_y)
    {
        if (move_x != 0 || move_y != 0)
        {
            float angle_degrees = Mathf.Atan2(move_y, move_x) * Mathf.Rad2Deg;
            Quaternion target_rotation = Quaternion.Euler(new Vector3(0, 0, angle_degrees));
            shooter.transform.rotation = Quaternion.Lerp(shooter.transform.rotation, target_rotation, rotation_speed * Time.fixedDeltaTime);
        }

        animator.SetFloat("MoveX", move_x);
        animator.SetFloat("MoveY", move_y);
    }
}
