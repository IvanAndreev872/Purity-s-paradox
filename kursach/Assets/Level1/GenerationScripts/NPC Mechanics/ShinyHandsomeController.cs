using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class ShinyHandsomeController : JumpPointSearch, MovementInterface
{
    public bool ableToMove { get; set; } = true;
    public Node currentNode;
    public List<Node> Path;
    public float viewAngle = 90f;

    public int index = 0;

    public GameObject shooter;
    public Transform character;
    public float basicSpeed = 3.0f;
    private float speedNow;
    private bool speedChanged = false;
    private float speedChangeDuration;
    private float speedChangeTime;
    private float AtackRange;
    private bool canAtack;

    public Vector2 facingDirection = Vector2.down;
    public bool playerSeen = false;
    public float triggerRadius = 7;

    private float UpdatePathInterval = 2f;
    private float timer = 2f;

    private int obstackleMask;
    private Animator animator;

    public enum States
    {
        Patrol,
        Engage,
        Atack,
        Triggered
    }

    public States currentState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        speedNow = basicSpeed;
        obstackleMask = LayerMask.GetMask("Wall");
        GetStartNode();
        currentState = States.Patrol;
        character = GameObject.FindGameObjectWithTag("Character").transform;
    }


    private void Update()
    {
        if (speedChanged && Time.time > speedChangeDuration + speedChangeTime)
        {
            speedChanged = false;
            speedNow = basicSpeed;
        }

        if (!ableToMove)
        {
            return;
        }

        switch (currentState)
        {
            case States.Patrol:
                Patrol();
                break;
            case States.Engage:
                Engage();
                break;
            case States.Atack:
                Atack();
                break;
            case States.Triggered:
                Triggered();
                break;
        }

        IfPlayerSeen();
        canAtack = Vector2.Distance(transform.position, character.transform.position) < AtackRange;

        if (!playerSeen && currentState != States.Patrol)
        {
            if (currentState != States.Triggered || currentState == States.Triggered && Path.Count == 0)
            {
                currentState = States.Patrol;
                Path.Clear();
            }
        }
        else if (playerSeen && currentState != States.Engage && !canAtack)
        {
            currentState = States.Engage;
            LayerMask enemyLayer = LayerMask.GetMask("Enemy");
            Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(transform.position, triggerRadius, enemyLayer);
            foreach (Collider2D col in enemyColliders)
            {
                if (col == null || col.GetComponent<ShinyHandsomeController>() == null)
                {
                    continue;
                }
                if (col.gameObject.GetComponent<ShinyHandsomeController>().currentState != States.Engage)
                {
                    col.gameObject.GetComponent<ShinyHandsomeController>().currentState = States.Triggered;
                    if (currentNode != null && character.GetComponent<NodeFinder>().currentNode != null)
                    {
                        col.gameObject.GetComponent<ShinyHandsomeController>().Path = GeneratePath(currentNode,
                        character.GetComponent<NodeFinder>().currentNode); 
                    }
                }
            }
            Path.Clear();
        }
        else if (playerSeen && currentState != States.Atack && canAtack)
        {
            currentState = States.Atack;
            Path.Clear();
        }

        generatePath();
    }

    public void ChangeSpeed(float coef, float time)
    {
        speedNow = basicSpeed * coef;
        speedChanged = true;
        speedChangeTime = Time.time;
        speedChangeDuration = time;
    }

    void Patrol()
    {
        Animate(Vector2.zero);
        return;
    }


    void Engage()
    {
        timer += Time.deltaTime;
        if (timer > UpdatePathInterval)
        {
            if (currentNode != null && character.GetComponent<NodeFinder>().currentNode != null)
            {
                Path.Clear();
                Path = GeneratePath(currentNode, character.GetComponent<NodeFinder>().currentNode);
                timer -= UpdatePathInterval;
            }
        }
    }

    void Atack()
    {
        Animate(Vector2.zero);
        return;
    }

    void Triggered()
    {
        if (Path.Count == 0)
        {
            currentState = States.Patrol;
        }
    }

    void Animate(Vector2 direction)
    {
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }

    void generatePath()
    {
        if (Path.Count > 0)
        {
            int x = 0;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(Path[x].transform.position.x, Path[x].transform.position.y),
                speedNow * Time.deltaTime);
            Animate(Path[x].transform.position - transform.position);

            if (Vector2.Distance(transform.position, Path[x].transform.position) < 0.1f)
            {
                currentNode = Path[x];
                Path.RemoveAt(x);
            }
        }
        return;
    }

    private void IfPlayerSeen()
    {
        float dist = Vector2.Distance(transform.position, character.transform.position);
        if (dist < 10.0f)
        {
            Vector2 directionToPlayer = (character.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(facingDirection, directionToPlayer);
            Debug.Log(angle);
            if (angle <= 45.0f)
            {
                if (!Physics2D.Raycast(transform.position, directionToPlayer, dist, obstackleMask))
                {
                    playerSeen = true;
                    facingDirection = directionToPlayer;
                    return;
                }
            }
        }
        playerSeen = false;
    }


    private void GetStartNode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.71f);
        float minDist = 1.0f;
        Node NearestNode = null;
        foreach (Collider2D collider in colliders)
        {
            Node node = collider.GetComponent<Node>();
            if (node != null)
            {
                float curDist = Vector2.Distance(transform.position, node.transform.position);
                if (minDist > curDist)
                {
                    minDist = curDist;
                    NearestNode = node;
                }
            }
        }
        currentNode = NearestNode;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 10);

        Vector3 viewAngleA = DirectionFromAngle(-viewAngle / 2);
        Vector3 viewAngleB = DirectionFromAngle(viewAngle / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * 10);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * 10);

        // Показываем, видим ли игрока
        if (playerSeen)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, character.position);
        }
    }

    private Vector2 DirectionFromAngle(float angle)
    {
        // Преобразование локального угла в глобальные координаты
        angle += transform.eulerAngles.y;
        return new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    }

}
