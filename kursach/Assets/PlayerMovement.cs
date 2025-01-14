using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float move_speed;
    public float dash_speed;
    public float dash_duration;
    public float rotation_speed;

    private Rigidbody2D rb;
    private bool is_dashing;
    private float dash_start;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (is_dashing)
        {
            Dash();
        }
        else
        {
            CheckDash();
            Move();
        }
    }

    private void Dash()
    {
        if (Time.time > dash_start + dash_duration) 
        { 
            is_dashing = false;
        }
        else
        {
            float move_x = Input.GetAxis("Horizontal");
            float move_y = Input.GetAxis("Vertical");

            rb.velocity = new Vector3(move_x * dash_speed, move_y * dash_speed);

            RotateCharacter(move_x, move_y);    
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

    private void Move()
    {
        float move_x = Input.GetAxis("Horizontal");
        float move_y = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(move_x * move_speed, move_y * move_speed);

        RotateCharacter(move_x, move_y);
    }

    private void RotateCharacter(float move_x, float move_y)
    {
        if (move_x != 0 || move_y != 0)
        {
            float angle_degrees = Mathf.Atan2(move_y, move_x) * Mathf.Rad2Deg;
            Quaternion target_rotation = Quaternion.Euler(new Vector3(0, 0, angle_degrees));
            transform.rotation = Quaternion.Lerp(transform.rotation, target_rotation, rotation_speed * Time.deltaTime);
        }
    }
}
