using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AngerStatusController;

public class DashAttackExplosion : MonoBehaviour
{
    private AngerStatusController angerStatusController;

    public Transform player;
    public MovementInterface movementInterface;

    public float dashSpeed;
    public float makeDistance;
    public float dashDelay;
    public float brakingDistance;
    public float damage;

    public GameObject explosionPrefab;

    public float pathDuration;
    public GameObject pathPrefab;

    private float dashTime;
    private bool isDashing = false;
    private bool isPreparing = false;
    private Vector2 dashTarget;
    private Vector2 retreatTarget;

    private float pathWidth;
    private Color pathColor;
    private TrailRenderer trail;

    private Rigidbody2D rb;

    private bool canBeEnabled = false;
    private bool pathPartCreated = false;

    private void Awake()
    {
        angerStatusController = GetComponentInParent<AngerStatusController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        movementInterface = GetComponent<MovementInterface>();
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        pathWidth = pathPrefab.transform.localScale.magnitude / 1.5f;
        pathColor = Color.red;
        trail.time = pathDuration;
        trail.startWidth = pathWidth;
        trail.endWidth = pathWidth;
        trail.material = new Material(Shader.Find("Sprites/Default"));
        trail.startColor = pathColor;
        trail.endColor = pathColor;
        trail.autodestruct = false;
        trail.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(movementInterface.able_to_move + " " + isDashing + " " + isPreparing + " " + canBeEnabled);
        if (movementInterface.able_to_move && !isDashing && !isPreparing && canBeEnabled)
        {
            Debug.Log(1);
            CheckDash();
        }
    }

    private void FixedUpdate()
    {
        if (isPreparing)
        {
            Retreat();
        }
        if (isDashing)
        {
            Dash();
        }
    }

    void CheckDash()
    {
        Debug.Log(Time.time + " " + (dashDelay + dashTime));
        if (Time.time > dashDelay + dashTime)
        {
            movementInterface.able_to_move = false;
            Vector3 directionOpposite = (transform.position - player.position).normalized;
            dashTime = Time.time;
            retreatTarget = player.position + directionOpposite * makeDistance;
            isPreparing = true;
        }
    }

    void Retreat()
    {
        if (Vector2.Distance(transform.position, retreatTarget) <= 0.1)
        {
            isPreparing = false;
            isDashing = true;

            Vector3 direction = (player.position - transform.position).normalized;
            dashTarget = player.position + direction * brakingDistance;
            MakePath(true);
        }

        MoveTowards(retreatTarget);
    }

    void Dash()
    {
        if (Vector2.Distance(transform.position, dashTarget) <= 0.1)
        {
            isDashing = false;
            movementInterface.able_to_move = true;
            trail.enabled = false;
            Explosion();
        }

        MakePath(false);

        MoveTowards(dashTarget);
    }

    void MoveTowards(Vector3 target) {
        Vector3 direction = (target - transform.position).normalized;
        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction, dashSpeed * Time.fixedDeltaTime));
    }

    void MakePath(bool newTrail)
    {
        GameObject path_part = Instantiate(pathPrefab, transform.position, transform.rotation);

        Destroy(path_part, pathDuration);

        if (newTrail)
        {
            trail.Clear();
            trail.enabled = true;
        }
    }

    void Explosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isPreparing)
        {
            isPreparing = false;
            isDashing = true;

            Vector3 direction = (player.position - transform.position).normalized;
            dashTarget = player.position + direction * brakingDistance;
            MakePath(true);
        }
        else if (isDashing)
        {
            isDashing = false;
            movementInterface.able_to_move = true;
            trail.enabled = false;
            DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();
            if (enemy != null)
            {
                enemy.Hit(damage);
            }
            Explosion();
        }
    }

    private void OnAngerChanged(AngerStatusController.AngerLevel new_level)
    {
        if (new_level == AngerLevel.Raged || new_level == AngerLevel.Enraged)
        {
            canBeEnabled = true;
        }
    }

    void OnEnable()
    {
        if (angerStatusController)
        {
            angerStatusController.EnragementChanged += OnAngerChanged;
        }
    }

    void OnDisable()
    {
        if (angerStatusController)
        {
            angerStatusController.EnragementChanged -= OnAngerChanged;
        }
    }
}
    