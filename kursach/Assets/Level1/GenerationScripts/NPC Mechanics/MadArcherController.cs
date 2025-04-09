using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadArcherController : JumpPointSearch, MovementInterface
{
    public bool ableToMove { get; set; } = true;
    public Node currentNode;
    public List<Node> Path;
    public Node nextNode;

    public int index = 1;

    public GameObject shooter;
    public Transform character;
    public float basicSpeed = 3.0f;
    private float speedNow;
    private bool speedChanged = false;
    private float speedChangeDuration;
    private float speedChangeTime;
    private float maxDistance;
    private bool canShoot;

    public bool playerSeen = false;

    private float UpdatePathInterval = 1f;
    private float timer = 1f;
    private Animator animator;

    public enum States
    {
        Patrol,
        Engage,
        Shoot
    }

    public States currentState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        InaccurateShooter shooterScript = shooter.GetComponent<InaccurateShooter>();
        maxDistance = shooterScript.shooterDistanceMax;
        speedNow = basicSpeed;
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
            case States.Shoot:
                Shoot();
                break;
        }

        playerSeen = Vector2.Distance(transform.position, character.transform.position) < 10.0f;
        canShoot = Vector2.Distance(transform.position, character.transform.position) < maxDistance;

        if (!playerSeen && currentState != States.Patrol)
        {
            currentState = States.Patrol;
            Path.Clear();
            nextNode = null;
        }
        else if (playerSeen && currentState != States.Engage && !canShoot)
        {
            currentState = States.Engage;
            Path.Clear();
            nextNode = null;
        }
        else if (playerSeen && currentState != States.Shoot && canShoot)
        {
            currentState = States.Shoot;
            Path.Clear();
            nextNode = null;
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

    void Animate(Vector2 direction)
    {
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }

    void Shoot()
    {
        return;
    }

    void generatePath()
    {
        if (Path.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(currentNode.transform.position.x, currentNode.transform.position.y),
                speedNow * Time.deltaTime);
            Animate(Path[0].transform.position - transform.position);

            if (Vector2.Distance(transform.position, currentNode.transform.position) < 0.1f)
            {
                nextNode = null;
                Path.RemoveAt(0);
                if (Path.Count > 0)
                {
                    nextNode = Path[0];
                    currentNode = nextNode;
                }
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
