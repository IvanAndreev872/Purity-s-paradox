using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FirstEnemyController : CorridorFirstDungeonGenerator
{
    public Node currentNode;
    public List<Node> Path;

    public PlayerMovement character;
    public float speed = 3.0f;

    public bool playerSeen = false;
    public int roomNumber = 0;

    public enum States
    {
        Patrol,
        Engage
    }

    public States currentState;

    private void Start()
    {
        currentState = States.Patrol;
        roomNumber = AStarAlgoritm.instance.FindRoomNumber(currentNode.transform.position);
    }


    private void Update()
    {
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

    void Patrol()
    {
        if (Path.Count == 0)
        {
            Path = AStarAlgoritm.instance.GeneratePath(currentNode, RoomsNodeList[roomNumber][Random.Range(0, RoomsNodeList[roomNumber].Count)]);
        }
    }

    void Engage()
    {
        if (Path.Count == 0)
        {
            Path = AStarAlgoritm.instance.GeneratePath(currentNode, AStarAlgoritm.instance.FindNearestNodeInRoom(character.transform.position, roomNumber));
        }
    }

    void generatePath()
    {
        if (Path.Count > 0)
        {
            int x = 0;
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(Path[x].transform.position.x, Path[x].transform.position.y),
                speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, Path[x].transform.position) < 0.1f)
            {
                currentNode = Path[x];
                Path.RemoveAt(x);
            }
        }
    }
}
