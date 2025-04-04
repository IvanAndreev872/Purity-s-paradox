using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LieController : MonoBehaviour, MovementInterface
{
    public bool ableToMove { get; set; } = true;
    public Transform player;
    public float walkSpeed;
    public float minimumDistance;
    public Transform roomCenter;
    public GameObject clonePrefab;
    public int maxClones;

    private bool tooClose = false;
    private bool isTeleporting = false;

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
        rb = GetComponent<Rigidbody2D>();
        walkSpeedNow = walkSpeed;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (speedChanged && Time.time > speedChangeDuration + speedChangeTime)
        {
            speedChanged = false;
            walkSpeedNow = walkSpeed;
        }

        CheckDistanceToPlayer();
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        if (tooClose && !isTeleporting && ableToMove)
        {
            MoveAway();
        }
    }

    void CheckDistanceToPlayer()
    {
        if (Vector2.Distance(rb.position, player.position) < minimumDistance)
        {
            tooClose = true;
        } else
        {
            tooClose = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall")) {
            if (!isTeleporting)
            {
                isTeleporting = true;
                Invoke("TeleportToCenter", 1);
            }
        }
    }

    void TeleportToCenter()
    {
        ActivateCopyAbility();
        isTeleporting = false;
        tooClose = false;
    }

    void ActivateCopyAbility()
    {
        Vector3[] positionsDeflection =
        {
            Vector3.right,
            Vector3.up,
            Vector3.down,
            Vector3.left,
        };

        int realPositionIndex = Random.Range(0, positionsDeflection.Length);

        int cloneNumber = GameObject.FindGameObjectsWithTag("Clone").Length;

        for (int i = 0; i < positionsDeflection.Length; i++)
        {
            Vector3 position = positionsDeflection[i] + roomCenter.position;
            if (i != realPositionIndex && cloneNumber < maxClones)
            {
                GameObject clone = Instantiate(clonePrefab, position, transform.rotation);
                LieController cloneController = clone.GetComponent<LieController>();
                if (cloneController != null)
                {
                    cloneController.player = player;
                    cloneController.walkSpeed = walkSpeed;
                    cloneController.minimumDistance = minimumDistance;
                    cloneController.roomCenter = roomCenter;
                    cloneController.maxClones = maxClones;
                    cloneController.clonePrefab = clonePrefab;
                }
                cloneNumber++;
            }
            else 
            {
                rb.position = position;
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

    void MoveAway()
    {
        Vector3 direction = (transform.position - player.position).normalized;

        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction.normalized, walkSpeedNow * Time.fixedDeltaTime));

        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }
}
