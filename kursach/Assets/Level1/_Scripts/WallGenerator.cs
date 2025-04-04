using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(List<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer) 
    {
        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionsList);
        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPositions);
        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);
    }

    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, List<Vector2Int> cornerWallPositions, List<Vector2Int>
     floorPositions) 
     {
        foreach (var position in cornerWallPositions) 
        {
            string neighboursBinaryType = "";
            foreach (var direction in Direction2D.eightDirectionsList) 
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
            foreach (var direction in Direction2D.cardinalDirectionsList) 
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
