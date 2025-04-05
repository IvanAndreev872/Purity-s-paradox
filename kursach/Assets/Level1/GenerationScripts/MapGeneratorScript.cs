using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO.IsolatedStorage;
using System.Xml.Linq;

public class CorridorFirstDungeonGenerator : DungeonGenerator
{
    [SerializeField]
    private int length = 70; 
    [SerializeField]
    private int count = 10; 
    [SerializeField]
    private float percent = 0.9f;

    [SerializeField] public Node nodePrefab;

    [SerializeField] public float EnemyFrequency = 8;
    [SerializeField] public int FreeSpace = 25;
    [SerializeField] public int EnemyQuantity = 15;

    [SerializeField] public List<GameObject> EnemyPrefabs;

    public static List<Vector2Int> DirectionsList = new List<Vector2Int> 
    {
        new Vector2Int(0, 1),  // up
        new Vector2Int(1, 0),  // right
        new Vector2Int(0, -1), // down
        new Vector2Int(-1, 0)  // left
    };
    public override void RunGeneration()
    {
        CorridorGeneration();
    }

    private void CorridorGeneration() 
    {
        List<Vector2Int> floors = new List<Vector2Int>();
        List<Vector2Int> hardRooms = new List<Vector2Int>();
        List<Node> NodeList = new List<Node>();
        GameObject NodeParent = new GameObject("NodeParent");
        GameObject EnemyParent = new GameObject("EnemyParent");

        List<List<Vector2Int>> corridors = CreateCorridors(floors, hardRooms);

        List<Vector2Int> roomPositions = CreateRooms(hardRooms);

        List<Vector2Int> badCorridors = SearchBadCorridors(floors);

        BadCorridorsCreate(badCorridors, roomPositions);

        floors.AddRange(roomPositions.Except(floors));

        for (int i = 0; i < count; i++) 
        {
            List<Vector2Int> current = corridors[i];
            current = CorridorExpansion(current);
            floors.AddRange(current.Except(floors));
        }

        CreateNodes(floors, NodeList, NodeParent);
        CreateConnections(NodeList);

        SpawnEnemies(NodeList, EnemyParent);

        tileRenderer.PaintFloorTiles(floors);
        WallGenerator.CreateWalls(floors, tileRenderer);
    }

    private void BadCorridorsCreate(List<Vector2Int> badCorridors, List<Vector2Int> roomFloors) 
    {
        foreach (Vector2Int position in badCorridors) {
            if (!roomFloors.Contains(position)) 
            {
                List<Vector2Int> room = SearchRoad(settings, position);
                roomFloors.AddRange(room.Except(roomFloors));
            }
        }
    }

    private List<Vector2Int> SearchBadCorridors(List<Vector2Int> floors) 
    {
        List<Vector2Int> badCorridors = new List<Vector2Int>();

        foreach (Vector2Int position in floors) 
        {
            int count = 0;
            foreach (Vector2Int direction in DirectionsList) 
            {
                if (floors.Contains(position + direction)) 
                    count += 1;
            }
            if (count == 1)
                badCorridors.Add(position);
        }
        return badCorridors;
    }
    private List<Vector2Int> CreateRooms(List<Vector2Int> hardRooms) 
    {
        List<Vector2Int> roomPositions = new List<Vector2Int>();

        int readyRoomsCount = Mathf.RoundToInt(hardRooms.Count * percent);

        List<Vector2Int> readyRooms = hardRooms.OrderBy(x => Guid.NewGuid()).Take(readyRoomsCount).ToList();
        
        foreach (Vector2Int pos in readyRooms)  
        {
            List<Vector2Int> roomFloor = SearchRoad(settings, pos);
            roomPositions.AddRange(roomFloor.Except(roomPositions));
        }

        return roomPositions;
    }

    public static List<Vector2Int> BuildCorridor(Vector2Int start, int length) 
    {
        List<Vector2Int> corridorSystem = new List<Vector2Int>();

        Vector2Int direction = directions[UnityEngine.Random.Range(0, 4)];
        corridorSystem.Add(start);
        Vector2Int current = start;

        for (int i = 0; i < length; i++) 
        {
            current += direction;
            corridorSystem.Add(current);
        }

        return corridorSystem;
    }

    private List<List<Vector2Int>> CreateCorridors(List<Vector2Int> floorPositions, List<Vector2Int> potentialRoomPositions) 
    {
        Vector2Int current = start;
        potentialRoomPositions.Add(current);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < count; i++) 
        {
            List<Vector2Int> corridor = BuildCorridor(current, length);
            corridors.Add(corridor);
            current = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(current);
            floorPositions.AddRange(corridor.Except(floorPositions));
        }
        return corridors;
    }
    
    public List<Vector2Int> CorridorExpansion(List<Vector2Int> corridor) 
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 1; i < corridor.Count; i++) 
        {
            for (int x = -1; x < 2; x++) 
            {
                for (int y = -1; y < 2; y++) 
                {
                    newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                }
            }
        }
        return newCorridor;
    }

    void CreateNodes(List<Vector2Int> floorPositions, List<Node> NodeList, GameObject NodeParent)
    {
        foreach (Vector2Int floor in floorPositions)
        {
            Node node = Instantiate(nodePrefab, new Vector2(floor.x + 0.5f, floor.y + 0.5f), Quaternion.identity);
            node.transform.SetParent(NodeParent.transform);
            NodeList.Add(node);
        }
    }

    void CreateConnections(List<Node> NodeList)
    {
        foreach (Node node in NodeList)
        {
            Vector2 position = node.transform.position;
            for (int i = -1; i < 2; ++i)
            {
                for (int j = -1; j < 2; ++j)
                {
                    position.x += i;
                    position.y += j;
                    Collider2D collider = Physics2D.OverlapCircle(position, 0.1f);
                    if (collider != null)
                    {
                        GameObject nodeInPos = collider.gameObject;
                        Node connectedNode = nodeInPos.GetComponent<Node>();
                        if (i * j == 0)
                        {
                            ConnectNodes(node, connectedNode);
                        } else
                        {
                            if (!CheckIfWallBetween(node, i, j))
                            {
                                ConnectNodes(node, connectedNode);
                            }
                        }
                    }
                    position.x -= i;
                    position.y -= j;
                }
            }
        }
    }

    void ConnectNodes(Node From, Node To)
    {
        if (From == To)
        {
            return;
        }
        From.connections.Add(To);
    }

    bool CheckIfWallBetween(Node First, int i, int j)
    {
        Vector2 FirstPosition = First.transform.position;
        Vector2 TestPosition1 = FirstPosition + new Vector2(i, 0);
        Vector2 TestPosition2 = FirstPosition + new Vector2(0, j);
        Collider2D collider1 = Physics2D.OverlapCircle(TestPosition1, 0.1f);
        Collider2D collider2 = Physics2D.OverlapCircle(TestPosition2, 0.1f);
        if (collider1 != null && collider2 != null)
        {
            return false;
        }
        return true;
    }

    void SpawnEnemies(List<Node> NodeList, GameObject EnemyParent)
    {
        List<Node> EnemySpawns = NodeList.OrderBy(x => Guid.NewGuid()).ToList();
        int combinedLayerMask = LayerMask.GetMask("Character", "Enemy");
        int index = 0;
        System.Random random = new System.Random();
        for (int i = 0; i < EnemyQuantity; ++i)
        {
            Node potentialSpawn = EnemySpawns[index + i];
            Collider2D[] colliders = Physics2D.OverlapCircleAll(potentialSpawn.transform.position, EnemyFrequency, combinedLayerMask);
            if (colliders.Length != 0)
            {
                index++;
                i--;
                continue;
            }
            GameObject EnemyPrefab = EnemyPrefabs.ElementAt(random.Next(0, EnemyPrefabs.Count));
            GameObject Enemy = Instantiate(EnemyPrefab, potentialSpawn.transform.position, Quaternion.identity);
            Enemy.transform.SetParent(EnemyParent.transform);
        }
    }
}
