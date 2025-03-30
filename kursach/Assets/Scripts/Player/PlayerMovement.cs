using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, MovementInterface
{
    public DamageInterface damage_interface;
    public bool able_to_move { get; set; } = true;

    public float walk_speed;
    public float dash_speed;
    private float dash_speed_now;
    private float walk_speed_now;

    private bool speed_changed = false;
    private float speed_change_duration;
    private float speed_change_time;

    public float dash_duration;

    public float rotation_speed;
    public GameObject shooter;

    public bool is_dash_invulnerable;

    public bool is_dash_poisoned;
    public GameObject poison_path_prefab;
    public float poison_path_duration;

    private Rigidbody2D rb;
    private bool is_dashing = false;
    private float dash_start;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        damage_interface = GetComponent<DamageInterface>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        walk_speed_now = walk_speed;
        dash_speed_now = dash_speed;
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
        if (speed_changed && Time.time > speed_change_duration + speed_change_time)
        {
            speed_changed = false;
            walk_speed_now = walk_speed;
            dash_speed_now = dash_speed;
        }

        if (!is_dashing)
        {
            CheckDash();
        }
    }

    private void Walk()
    {
        Move(walk_speed_now);
    }

    private void Dash()
    {
        if (Time.time > dash_start + dash_duration) 
        { 
            is_dashing = false;

            if (is_dash_invulnerable && damage_interface != null)
            {
                damage_interface.CanBeDamaged(true);
            }
        }
        else
        {
            Move(dash_speed_now);
            if (is_dash_poisoned)
            {
                MakePath();
            }
        }
    }

    private void CheckDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            is_dashing = true;
            dash_start = Time.time;

            if (is_dash_invulnerable)
            {
                damage_interface.CanBeDamaged(false);
            }
        }
    }

    public void ChangeSpeed(float coef, float time)
    {
        dash_speed_now = dash_speed * coef;
        walk_speed_now = walk_speed * coef;
        speed_changed = true;
        speed_change_time = Time.time;
        speed_change_duration = time;
    }

    private void Move(float speed)
    {
        float move_x = Input.GetAxis("Horizontal");
        float move_y = Input.GetAxis("Vertical");

        Vector3 direction = new(move_x * speed, move_y * speed);

        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction.normalized, speed * Time.fixedDeltaTime));

        RotateCharacter(move_x, move_y);
    }

    void MakePath()
    {
        GameObject path_part = Instantiate(poison_path_prefab, transform.position, transform.rotation);

        Destroy(path_part, poison_path_duration);
    }

    private void RotateCharacter(float move_x, float move_y)
    {
        if (move_x != 0 || move_y != 0)
        {
            float angle_degrees = Mathf.Atan2(move_y, move_x) * Mathf.Rad2Deg;
            Quaternion target_rotation = Quaternion.Euler(new Vector3(0, 0, angle_degrees));
            if (shooter)
            {
                shooter.transform.rotation = Quaternion.Lerp(shooter.transform.rotation, target_rotation, rotation_speed * Time.fixedDeltaTime);
            }
        }

        animator.SetFloat("MoveX", move_x);
        animator.SetFloat("MoveY", move_y);
    }
}
    