using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{   
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int> 
    {
        new Vector2Int(0, 1),  // UP
        new Vector2Int(1, 0),  // RIGHT 
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, 0)  // LEFT
    };

    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int> 
    {
        new Vector2Int(1, 1),  // UP-RIGHT
        new Vector2Int(1, -1),  // RIGHT-DOWN 
        new Vector2Int(-1, -1), // DOWN-LEFT
        new Vector2Int(-1, 1)  // LEFT-UP
    };

    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1),  // UP
        new Vector2Int(1, 1),  // UP-RIGHT
        new Vector2Int(1, 0),  // RIGHT 
        new Vector2Int(1, -1),  // RIGHT-DOWN 
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, -1), // DOWN-LEFT
        new Vector2Int(-1, 0),  // LEFT
        new Vector2Int(-1, 1)  // LEFT-UP
    };
    public static void CreateWalls(List<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer) 
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, cardinalDirectionsList);
        var cornerWallPositions = FindWallsInDirections(floorPositions, diagonalDirectionsList);
        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, List<Vector2Int> cornerWallPositions, List<Vector2Int>
     floorPositions) 
     {
        foreach (var position in cornerWallPositions) 
        {
            string neighboursBinaryType = "";
            foreach (var direction in eightDirectionsList) 
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition)) 
                {
                    neighboursBinaryType += "1";
                }
                else 
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);
        }
     }

    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, List<Vector2Int> basicWallPositions, List<Vector2Int>
     floorPositions) 
    {
        foreach (var position in basicWallPositions) 
        {
            string neighboursBinaryType = "";
            foreach (var direction in cardinalDirectionsList) 
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition)) 
                {
                    neighboursBinaryType += "1";
                }
                else 
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryType);
        }
    }

    private static List<Vector2Int> FindWallsInDirections(List<Vector2Int> floorPositions, List<Vector2Int> directionList) 
    {
        List<Vector2Int> wallPositions = new List<Vector2Int>();
        foreach (var position in floorPositions) 
        {
            foreach (var direction in directionList) 
            {
                var neighbourPosition =  position + direction;
                if (floorPositions.Contains(neighbourPosition) == false) 
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }
        return wallPositions;
    }
}
