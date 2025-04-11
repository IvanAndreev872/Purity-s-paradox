using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour, MovementInterface
{
    public DamageInterface damageInterface;
    public bool ableToMove { get; set; } = true;

    public float walkSpeed;
    public float dashSpeed;
    private float dashSpeedNow;
    private float walkSpeedNow;

    private bool speedChanged = false;
    private float speedChangeDuration;
    private float speedChangeTime;

    public float dashDuration;
    public float dashDelay = 0.8f;
    private float dashTime;

    public float rotationSpeed;
    public GameObject shooter;

    public bool isDashInvulnerable;

    public bool isDashPoisoned;
    public GameObject poisonPathPrefab;
    public float poisonPathDuration;

    public LineRenderer lineRenderer;

    private float pathMagnitude;

    private bool pathPartCreated = false;
    private Vector2 previousPartPosition;
    private float pathPartDistance;
    private Rigidbody2D rb;
    private bool isDashing = false;
    private float dashStart;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        UpdateSpeed(GetComponent<PlayerStats>());
        damageInterface = GetComponent<DamageInterface>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        walkSpeedNow = walkSpeed;
        dashSpeedNow = dashSpeed;

        if (poisonPathPrefab != null)
        {
            pathMagnitude = poisonPathPrefab.transform.localScale.magnitude / 2;
            pathPartDistance = pathMagnitude;
        }

        if (shooter == null)
        {
            shooter = transform.Find("Shooter").gameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDashing)
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
        if (speedChanged && Time.time > speedChangeDuration + speedChangeTime)
        {
            speedChanged = false;
            walkSpeedNow = walkSpeed;
            dashSpeedNow = dashSpeed;
        }

        if (!isDashing)
        {
            CheckDash();
        }
    }

    public void UpdateSpeed(PlayerStats playerStats)
    {
        walkSpeed = playerStats.walkSpeed;
        dashSpeed = playerStats.dashSpeed;
        dashDuration = playerStats.dashDuration;
        walkSpeedNow = walkSpeed;
        dashSpeedNow = dashSpeed;
        isDashInvulnerable = playerStats.isDashInvulnerable >= 1;
        isDashPoisoned = playerStats.isDashPoisoned >= 1;
    }

    private void Walk()
    {
        Move(walkSpeedNow);
    }

    private void Dash()
    {
        if (Time.time > dashStart + dashDuration)
        {
            dashTime = Time.time;
            isDashing = false;
            pathPartCreated = false;

            if (isDashInvulnerable && damageInterface != null)
            {
                damageInterface.CanBeDamaged(true);
            }
        }
        else
        {
            Move(dashSpeedNow);
            if (isDashPoisoned)
            {
                if (pathPartCreated && Vector2.Distance(transform.position, previousPartPosition) >= pathPartDistance)
                {
                    MakePath();
                }
                else if (!pathPartCreated)
                {
                    MakePath();
                }
            }
        }
    }

    private void CheckDash()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) && Time.time > dashDelay + dashTime)
        {
            dashTime = Time.time;
            isDashing = true;
            dashStart = Time.time;

            if (isDashInvulnerable)
            {
                damageInterface.CanBeDamaged(false);
            }
            if (isDashPoisoned)
            {
                CreateNewLine();
            }
        }
    }

    void CreateNewLine()
    {
        GameObject newLineObject = new GameObject("PoisonPath");
        LineRenderer newLine = newLineObject.AddComponent<LineRenderer>();

        newLine.startWidth = pathMagnitude;
        newLine.endWidth = pathMagnitude;
        newLine.positionCount = 0;
        newLine.startColor = Color.green;
        newLine.endColor = Color.green;
        newLine.material = new Material(Shader.Find("Sprites/Default"));
        newLine.sortingOrder = 1;

        lineRenderer = newLine;
    }

    public void ChangeSpeed(float coef, float time)
    {
        dashSpeedNow = dashSpeed * coef;
        walkSpeedNow = walkSpeed * coef;
        speedChanged = true;
        speedChangeTime = Time.time;
        speedChangeDuration = time;
    }

    private void Move(float speed)
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 direction = new(moveX * speed, moveY * speed);

        rb.MovePosition(Vector3.Lerp(transform.position, transform.position + direction.normalized, speed * Time.fixedDeltaTime));

        RotateCharacter(moveX, moveY);
    }

    void MakePath()
    {
        GameObject pathPart = Instantiate(poisonPathPrefab, transform.position, transform.rotation);
        pathPartCreated = true;
        previousPartPosition = transform.position;

        Vector2 currentPoint = transform.position;
        ++lineRenderer.positionCount;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, currentPoint);

        Destroy(pathPart, poisonPathDuration);
        StartCoroutine(EraseLast(lineRenderer));
    }

    IEnumerator EraseLast(LineRenderer line)
    {
        yield return new WaitForSeconds(poisonPathDuration);

        if (line.positionCount > 0)
        {
            for (int i = 1; i < line.positionCount; i++)
            {
                line.SetPosition(i - 1, line.GetPosition(i));
            }

            line.positionCount--;
        }
        if (line.positionCount == 0)
        {
            Destroy(line.gameObject);
        }
    }

    private void RotateCharacter(float moveX, float moveY)
    {
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);

        Debug.Log(moveX + " " + moveY);

        if (moveX != 0 || moveY != 0)
        {
            float angleDegrees = Mathf.Atan2(moveY, moveX) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angleDegrees));
            if (shooter)
            {
                shooter.transform.rotation = Quaternion.Lerp(shooter.transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }

            animator.SetFloat("LastX", moveX);
            animator.SetFloat("LastY", moveY);
        }
    }
}
