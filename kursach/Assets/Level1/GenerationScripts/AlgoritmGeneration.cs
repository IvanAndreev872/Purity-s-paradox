using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine;

public class DungeonGenerator : Generation
{
    [SerializeField]
    protected SimpleRandomWalkSO settings; 

    public static List<Vector2Int> directions = new List<Vector2Int> {new Vector2Int(0, 1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};

    public override void RunGeneration() 
    {
        List<Vector2Int> floorPositions = SearchRoad(settings, start);

        tileRenderer.ClearSpace();
        
        tileRenderer.PaintFloorTiles(floorPositions);
      
        WallGenerator.CreateWalls(floorPositions, tileRenderer); 
    }

    public static List<Vector2Int> RandomWalk(Vector2Int start, int length) // Алгоритм блуждания по клеткам пространства
    {
        List<Vector2Int> road = new List<Vector2Int>();

        road.Add(start);

        Vector2Int previous = start;

        for (int i = 0; i < length; i++) 
        {
            Vector2Int newPos = previous + directions[Random.Range(0, 4)];
            previous = newPos;
            road.Add(newPos);
        }
        
        return road;
    }

    // Метод, реализующий алгоритм случайного блуждания с заданными параметрами
    protected List<Vector2Int> SearchRoad(SimpleRandomWalkSO parameters, Vector2Int position) 
    {
        Vector2Int current = position; 
        List<Vector2Int> floorPositions = new List<Vector2Int>();
        
        for (int i = 0; i < parameters.iterations; i++) {
            List<Vector2Int> road = RandomWalk(current, parameters.walkLength);
            
            floorPositions.AddRange(road.Except(floorPositions));
        }
        return floorPositions;
    }

    public override void RespawnEnemies()
    {
        return;   
    }
}
