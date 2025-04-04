using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AngerStatusController;

public class BounceAttack : MonoBehaviour
{
    private AngerStatusController angerStatusController;

    public MovementInterface movementInterface;

    public float bounceSpeed;
    public float bounceDelay;
    public float bounceDuration;
    public float damage;

    private Vector3 bounceDirection;
    private bool isBouncing = false;
    private float bounceTime;

    public GameObject explosionPrefab;

    private Rigidbody2D rb;

    private bool canBeEnabled = false;

    private void Awake()
    {
        angerStatusController = GetComponentInParent<AngerStatusController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        movementInterface = GetComponent<MovementInterface>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementInterface.ableToMove && !isBouncing && canBeEnabled)
        {
            CheckBounce();
        }
    }

    private void FixedUpdate()
    {
        if (isBouncing)
        {
            Bounce();
        }
    }

    void CheckBounce()
    {
        if (Time.time > bounceDelay + bounceTime)
        {
            movementInterface.ableToMove = false;
            bounceDirection = Random.insideUnitCircle.normalized;
            bounceTime = Time.time;
            isBouncing = true;
        }
    }

    void Bounce()
    {
        if (Time.time >= bounceTime + bounceDuration)
        {
            isBouncing = false;
            movementInterface.ableToMove = true;
            Explosion();
        }
        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + bounceDirection, bounceSpeed * Time.fixedDeltaTime));
    }

    void Explosion()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, transform.rotation);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBouncing)
        {
            DamageInterface enemy = collision.gameObject.GetComponent<DamageInterface>();
            if (enemy != null)
            {
                isBouncing = false;
                movementInterface.ableToMove = true;
                enemy.Hit(damage);
                Explosion();
            } else
            {
                Vector2 normalToWall = collision.GetContact(0).normal;
                bounceDirection = Vector2.Reflect(bounceDirection, normalToWall);
            }
        }
    }

    private void OnAngerChanged(AngerStatusController.AngerLevel newLevel)
    {
        if (newLevel == AngerLevel.Raged || newLevel == AngerLevel.Enraged)
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
