using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine;

public class NewBehaviourScript : AbstractDungeonGenerator
{
    [SerializeField]
    protected SimpleRandomWalkSO randomWalkParameters; 
    
    protected override void RunProceduralGeneration() 
    {
        HashSet<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, startPosition);
        tilemapVisualizer.Clear();
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer); 
    }

    protected HashSet<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position) 
    {
        var currentPosition = position;
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        
        for (int i = 0; i < parameters.iterations; i++) {
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, parameters.walkLength);
            floorPositions.UnionWith(path);
            if (parameters.startRandomlyEachIteration) 
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }

    protected int GetRoomLength(HashSet<Vector2Int> floorPositions)
    {
        floorPositions.OrderBy(x => x.x);
        Vector2Int LeftFloor = floorPositions.First();
        Vector2Int RightFloor = floorPositions.Last();

        return RightFloor.x - LeftFloor.x + 1;
    }

    protected int GetRoomHeight(HashSet<Vector2Int> floorPositions)
    {
        floorPositions.OrderBy(x => x.y);
        Vector2Int LowFloor = floorPositions.First();
        Vector2Int HighFloor = floorPositions.Last();

        return HighFloor.y - LowFloor.y + 1;
    }

    protected int GetLeft(HashSet<Vector2Int> floorPositions)
    {
        Vector2Int Left = floorPositions.OrderBy(x => x.x).First();

        return Left.x;
    }

    protected int GetLowest(HashSet<Vector2Int> floorPositions)
    {
        Vector2Int Bottom = floorPositions.OrderBy(x => x.y).First();

        return Bottom.y;
    }
}
