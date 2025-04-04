using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public static class WallGenerator
{   
    public static List<Vector2Int> cardinalDirectionsList = new List<Vector2Int> 
    {
        new Vector2Int(0, 1),  // up
        new Vector2Int(1, 0),  // right
        new Vector2Int(0, -1), // down
        new Vector2Int(-1, 0)  // left
    };

    public static List<Vector2Int> diagonalDirectionsList = new List<Vector2Int> 
    {
        new Vector2Int(1, 1),  // up right
        new Vector2Int(1, -1),  // right down
        new Vector2Int(-1, -1), // down left
        new Vector2Int(-1, 1)  // left up
    };

    public static List<Vector2Int> eightDirectionsList = new List<Vector2Int>
    {
        new Vector2Int(0, 1),  // up
        new Vector2Int(1, 1),  // up right
        new Vector2Int(1, 0),  // right
        new Vector2Int(1, -1),  // right down
        new Vector2Int(0, -1), // down
        new Vector2Int(-1, -1), // down left
        new Vector2Int(-1, 0), // left
        new Vector2Int(-1, 1)  // left up
    };
    public static void CreateWalls(List<Vector2Int> floorPositions, TilemapRenderer tilemapVisualizer) 
    {
        List<Vector2Int> basicWallPositions = FindWallsInDirections(floorPositions, cardinalDirectionsList);
        List<Vector2Int> cornerWallPositions = FindWallsInDirections(floorPositions, diagonalDirectionsList);
        
        foreach (Vector2Int position in basicWallPositions) 
        {
            string neighboursBinaryType = "";
            foreach (Vector2Int direction in cardinalDirectionsList) 
            {
                Vector2Int neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition)) 
                {
                    neighboursBinaryType += "1";
                }
                else 
                {
                    neighboursBinaryType += "0";
                }
            }
            tilemapVisualizer.TileBaseWall(position, neighboursBinaryType);
        }

        foreach (Vector2Int position in cornerWallPositions) 
        {
            string neighboursBinaryType = "";
            foreach (Vector2Int direction in eightDirectionsList) 
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
            tilemapVisualizer.TileCornerWall(position, neighboursBinaryType);
        }
    }
    private static List<Vector2Int> FindWallsInDirections(List<Vector2Int> floor, List<Vector2Int> directions) 
    {
        List<Vector2Int> wallPositions = new List<Vector2Int>();
        foreach (Vector2Int pos in floor) 
        {
            foreach (Vector2Int direction in directions) 
            {
                Vector2Int neighbour =  pos + direction;
                if (!floor.Contains(neighbour)) 
                {
                    wallPositions.Add(neighbour);
                }
            }
        }
        return wallPositions;
    }
}
