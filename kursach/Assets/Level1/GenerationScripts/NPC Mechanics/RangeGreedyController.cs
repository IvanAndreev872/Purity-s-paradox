using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class RangeGreedyController : JumpPointSearch, MovementInterface
{
    public bool ableToMove { get; set; } = true;
    public Node currentNode;
    public List<Node> Path;

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
        BaseShooter shooterScript = shooter.GetComponent<BaseShooter>();
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
        }
        else if (playerSeen && currentState != States.Engage && !canShoot)
        {
            currentState = States.Engage;
            Path.Clear();
        }
        else if (playerSeen && currentState != States.Shoot && canShoot)
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

    void Animate(Vector2 direction)
    {
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
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
