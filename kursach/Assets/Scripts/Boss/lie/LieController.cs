using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LieController : MonoBehaviour, MovementInterface
{
    public bool able_to_move { get; set; } = true;
    public Transform player;
    public float walk_speed;
    public float minimum_distance;
    public Transform room_center;
    public GameObject clone_prefab;
    public int max_clones;

    private bool too_close = false;
    private bool is_teleporting = false;

    private float walk_speed_now;

    private bool speed_changed = false;
    private float speed_change_duration;
    private float speed_change_time;

    private Rigidbody2D rb;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        walk_speed_now = walk_speed;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (speed_changed && Time.time > speed_change_duration + speed_change_time)
        {
            speed_changed = false;
            walk_speed_now = walk_speed;
        }

        CheckDistanceToPlayer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (too_close && !is_teleporting && able_to_move)
        {
            MoveAway();
        }
    }

    void CheckDistanceToPlayer()
    {
        if (Vector2.Distance(rb.position, player.position) < minimum_distance)
        {
            too_close = true;
        } else
        {
            too_close = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall")) {
            if (!is_teleporting)
            {
                is_teleporting = true;
                Invoke("TeleportToCenter", 1);
            }
        }
    }

    void TeleportToCenter()
    {
        ActivateCopyAbility();
        is_teleporting = false;
        too_close = false;
    }

    void ActivateCopyAbility()
    {
        Vector3[] positions_deflection =
        {
            Vector3.right,
            Vector3.up,
            Vector3.down,
            Vector3.left,
        };

        int real_position_index = Random.Range(0, positions_deflection.Length);

        int clone_number = GameObject.FindGameObjectsWithTag("Clone").Length;

        for (int i = 0; i < positions_deflection.Length; i++)
        {
            Vector3 position = positions_deflection[i] + room_center.position;
            if (i != real_position_index && clone_number < max_clones)
            {
                GameObject clone = Instantiate(clone_prefab, position, transform.rotation);
                LieController clone_controller = clone.GetComponent<LieController>();
                if (clone_controller != null)
                {
                    clone_controller.player = player;
                    clone_controller.walk_speed = walk_speed;
                    clone_controller.minimum_distance = minimum_distance;
                    clone_controller.room_center = room_center;
                    clone_controller.max_clones = max_clones;
                    clone_controller.clone_prefab = clone_prefab;
                }
                clone_number++;
            }
            else 
            {
                rb.position = position;
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

    void MoveAway()
    {
        Vector3 direction = (transform.position - player.position).normalized;

        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction.normalized, walk_speed_now * Time.fixedDeltaTime));

        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }
}
