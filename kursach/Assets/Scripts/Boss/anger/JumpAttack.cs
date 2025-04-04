using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AngerStatusController;

public class JumpAttack : MonoBehaviour
{
    private AngerStatusController angerStatusController;

    public Transform player;
    public MovementInterface movementInterface;

    public GameObject jumpEffect;
    public GameObject shadowPrefab;
    public GameObject explosionPrefab;
    public float attackDistance;
    public float abilityDuration;
    public float damage;
    public float jumpDelay;
    public float flySpeed;
    public float stopTime;

    private GameObject shadow;
    private bool isJumping = false;
    private float jumpTime;

    private EnemyMelee melee;
    private Rigidbody2D rb;
    private SpriteRenderer bossRenderer;
    private Collider2D bossCollider;

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
        bossCollider = GetComponent<Collider2D>();
        bossRenderer = GetComponent<SpriteRenderer>();
        Debug.Log(1);
        melee = GetComponent<EnemyMelee>();
        Debug.Log(melee);
    }

    // Update is called once per frame
    void Update()
    {
        if (movementInterface.ableToMove && !isJumping && canBeEnabled)
        {
            CheckJump();
        }
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            ShadowTransparence();
            if (Time.time < jumpTime + stopTime)
            {
                MoveTowards(player.transform.position);
            }
            else if (Time.time >= jumpTime + abilityDuration)
            {
                FinishAttack();
            }
        }
    }

    void CheckJump()
    {
        if (Time.time > jumpDelay + jumpTime)
        {
            melee.enabled = false;
            movementInterface.ableToMove = false;
            jumpTime = Time.time;
            isJumping = true;

            bossRenderer.enabled = false;
            bossCollider.enabled = false;

            Instantiate(jumpEffect, transform.position, transform.rotation);
            shadow = Instantiate(shadowPrefab, transform.position, transform.rotation, transform);
        }
    }

    void ShadowTransparence()
    {
        if (shadow != null)
        {
            SpriteRenderer shadowRenderer = shadow.GetComponent<SpriteRenderer>();
            if (shadowRenderer != null)
            {
                float attackProgress = Mathf.Clamp01((Time.time - jumpTime) / abilityDuration);
                Color shadowColor = shadowRenderer.color;
                shadowColor.a = attackProgress;
                shadowRenderer.color = shadowColor;
            }
        }
    }

    void FinishAttack()
    {
        bossCollider.enabled = true;
        bossRenderer.enabled = true;
        melee.enabled = true;

        movementInterface.ableToMove = true;
        isJumping = false;
        Destroy(shadow);

        Instantiate(explosionPrefab, transform.position, transform.rotation);
    }

    void MoveTowards(Vector3 target)
    {
        if (Vector2.Distance(transform.position, target) >= 0.1)
        {
            Vector3 direction = (target - transform.position).normalized;
            rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction, flySpeed * Time.fixedDeltaTime));
        }
    }

    private void OnAngerChanged(AngerStatusController.AngerLevel newLevel)
    {
        if (newLevel == AngerLevel.Enraged)
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
