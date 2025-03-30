using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.IO.IsolatedStorage;
using System.Xml.Linq;

public class CorridorFirstDungeonGenerator : NewBehaviourScript
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;

    [SerializeField] public Node nodePrefab;

    [SerializeField] public float EnemyFrequency = 8;
    [SerializeField] public int FreeSpace = 25;
    [SerializeField] public int EnemyQuantity = 15;

    [SerializeField] public GameObject EnemyPrefab;
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration() 
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();
        HashSet<Node> NodeList = new HashSet<Node>();
        GameObject NodeParent = new GameObject("NodeParent");
        GameObject EnemyParent = new GameObject("EnemyParent");

        
        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        for (int i = 0; i < corridorCount; i++) 
        {
            corridors[i] = IncreaseCorridorBrush3by3(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }

        CreateNodes(floorPositions, NodeList, NodeParent);
        CreateConnections(NodeList);

        SpawnEnemies(NodeList, EnemyParent);

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors) 
    {
        foreach (var position in deadEnds) {
            if (roomFloors.Contains(position) == false) 
            {
                var room = RunRandomWalk(randomWalkParameters, position);
                roomFloors.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions) 
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();

        foreach (var position in floorPositions) 
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList) 
            {
                if (floorPositions.Contains(position + direction)) 
                    neighboursCount++;
            }
            if (neighboursCount == 1)
                deadEnds.Add(position);
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions) 
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        foreach (var roomPosition in roomsToCreate) 
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions) 
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        for (int i = 0; i < corridorCount; i++) 
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            corridors.Add(corridor);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
            floorPositions.UnionWith(corridor);

        }
        return corridors;
    }

    public List<Vector2Int> IncreaseCorridorSizeByOne(List<Vector2Int> corridor) 
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        Vector2Int previousDirection = Vector2Int.zero;

        for (int i = 1; i < corridor.Count; i++) 
        {
            Vector2Int directionFromCell = corridor[i] - corridor[i - 1];
            if (previousDirection != Vector2Int.zero &&
                  directionFromCell != previousDirection) 
            {
                for (int x = - 1; x < 2; x++) 
                {
                    for (int y = -1; y < 2; y++) 
                    {
                        newCorridor.Add(corridor[i - 1] + new Vector2Int(x, y));
                    }
                }
                previousDirection = directionFromCell;
            }
            else 
            {
                Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
            }
        }
        return newCorridor;
    }

    public List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridor) 
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

    private Vector2Int GetDirection90From(Vector2Int direction) 
    {
        if (direction == Vector2Int.up)
            return Vector2Int.right;
        if (direction == Vector2Int.right)
            return Vector2Int.down;
        if (direction == Vector2Int.down)
            return Vector2Int.left;
        if (direction == Vector2Int.left)
            return Vector2Int.up;
        return Vector2Int.zero;        
    }

    void CreateNodes(HashSet<Vector2Int> floorPositions, HashSet<Node> NodeList, GameObject NodeParent)
    {
        foreach (var floor in floorPositions)
        {
            Node node = Instantiate(nodePrefab, new Vector2(floor.x + 0.5f, floor.y + 0.5f), Quaternion.identity);
            node.transform.SetParent(NodeParent.transform);
            NodeList.Add(node);
        }
    }

    void CreateConnections(HashSet<Node> NodeList)
    {
        foreach (var node in NodeList)
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
                        ConnectNodes(node, connectedNode);
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

    void SpawnEnemies(HashSet<Node> NodeList, GameObject EnemyParent)
    {
        List<Node> EnemySpawns = NodeList.OrderBy(x => Guid.NewGuid()).ToList();
        int combinedLayerMask = LayerMask.GetMask("Character", "Enemy");
        int index = 0;
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
            GameObject Enemy = Instantiate(EnemyPrefab, potentialSpawn.transform.position, Quaternion.identity);
            Enemy.transform.SetParent(EnemyParent.transform);
        }
    }
}
