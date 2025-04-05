using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class FirstEnemyController : AStarAlgoritm, MovementInterface
{
    public bool ableToMove { get; set; } = true;
    public Node currentNode;
    public List<Node> Path;

    public Transform character;
    public float basicSpeed = 3.0f;
    private float speedNow;
    private bool speedChanged = false;
    private float speedChangeDuration;
    private float speedChangeTime;

    public bool playerSeen = false;

    private float UpdatePathInterval = 1f;
    private float timer = 1f;


    public enum States
    {
        Patrol,
        Engage
    }

    public States currentState;

    private void Awake()
    {
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
        }

        playerSeen = Vector2.Distance(transform.position, character.transform.position) < 7.0f;

        if (playerSeen == false && currentState != States.Patrol)
        {
            currentState = States.Patrol;
            Path.Clear();
        }
        else if (playerSeen == true && currentState != States.Engage)
        {
            currentState = States.Engage; 
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
        return;
    }

    void Engage()
    {
        timer += Time.deltaTime;
        if (timer > UpdatePathInterval)
        {
            Path.Clear();
            Path = GeneratePath(currentNode, character.GetComponent<NodeFinder>().currentNode);
            timer -= UpdatePathInterval;
        }
    }

    void generatePath()
    {
        if (Path.Count > 0)
        {
            int x = 0;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(Path[x].transform.position.x, Path[x].transform.position.y),
                speedNow * Time.deltaTime);

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

    private void OnDrawGizmos()
    {
        if (Path.Count > 0)
        {
            Gizmos.color = Color.red;
            for (int i = 1; i < Path.Count; i++)
            {
                Gizmos.DrawLine(Path[i].transform.position, Path[i - 1].transform.position);  
            }
        }
    }
}
