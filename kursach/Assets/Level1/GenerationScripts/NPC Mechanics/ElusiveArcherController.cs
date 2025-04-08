using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class ElusiveArcherController : JumpPointSearch, MovementInterface
{
    public bool ableToMove { get; set; } = true;
    public Node currentNode;
    public List<Node> Path;

    public int index = 0;

    public GameObject shooter;
    public Transform character;
    public float basicSpeed = 3.0f;
    public GameObject trigger;
    private ArcherTrigger archerTrigger;
    private float speedNow;
    private bool speedChanged = false;
    private float speedChangeDuration;
    private float speedChangeTime;
    private float maxDistance;
    private float minDistance;
    private bool canShoot;
    private bool haveToRetreat;
    private Collider2D unitCollider;

    public bool playerSeen = false;

    private float UpdatePathInterval = 2f;
    private float timer = 2f;
    private Rigidbody2D rb;
    private Animator animator;

    public enum States
    {
        Patrol,
        Engage,
        Shoot,
        Retreat
    }

    public States currentState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        archerTrigger = trigger.GetComponent<ArcherTrigger>();
        unitCollider = GetComponent<Collider2D>();
        BaseShooter shooterScript = shooter.GetComponent<BaseShooter>();
        maxDistance = shooterScript.shooterDistanceMax;
        minDistance = shooterScript.shooterDistanceMin;
        speedNow = basicSpeed;
        GetStartNode();
        currentState = States.Patrol;
        character = GameObject.FindGameObjectWithTag("Character").transform;
        rb = GetComponent<Rigidbody2D>();
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
            case States.Shoot:
                Shoot();
                break;
            case States.Retreat:
                Retreat();
                break;
        }

        playerSeen = Vector2.Distance(transform.position, character.transform.position) < 20.0f;
        canShoot = Vector2.Distance(transform.position, character.transform.position) < maxDistance;
        haveToRetreat = Vector2.Distance(transform.position, character.transform.position) < minDistance;

        if (!playerSeen && currentState != States.Patrol)
        {
            currentState = States.Patrol;
            Path.Clear();
        }
        else if (playerSeen && currentState != States.Engage && !canShoot)
        {
            currentState = States.Engage;
            Path.Clear();
        }
        else if (playerSeen && currentState != States.Retreat && haveToRetreat)
        {
            currentState = States.Retreat;
            Path.Clear();
        }
        else if (playerSeen && currentState != States.Shoot && canShoot && !haveToRetreat)
        {
            currentState = States.Shoot;
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

    void Shoot()
    {
        Animate(Vector2.zero);
        return;
    }

    void Animate(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            Debug.Log(direction);
        }
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }

    void Retreat()
    {
        Vector3 direction = (transform.position - character.transform.position).normalized;
        Animate(direction);
        Vector2 dMove = direction.normalized * speedNow * Time.deltaTime;
        if (!archerTrigger.InTrigger)
        {
            rb.MovePosition(rb.position + dMove);
        }
        GetStartNode();
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
}
