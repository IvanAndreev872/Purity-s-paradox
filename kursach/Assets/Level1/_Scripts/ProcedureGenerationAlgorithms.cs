using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerationAlgorithm // Алгоритмы случайного блуждания
{
    public static List<Vector2Int> directions = new List<Vector2Int> {new Vector2Int(0, 1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};
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

    // Алгоритм создания прямых коридоров
    public static List<Vector2Int> buildCorridor(Vector2Int start, int length) 
    {
        List<Vector2Int> corridorSystem = new List<Vector2Int>();

        Vector2Int direction = directions[Random.Range(0, 4)];
        corridorSystem.Add(start);
        Vector2Int current = start;

        for (int i = 0; i < length; i++) 
        {
            current += direction;
            corridorSystem.Add(current);
        }

        return corridorSystem;
    }
}

// Статический класс для работы с направлениями в 2D пространстве
public static class Direction2D 
{
    // Список основных направлений (по сторонам света)
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

    // Метод для получения случайного основного направления
    public static Vector2Int GetRandomCardinalDirection() 
    {
        // Возвращаем случайное направление из списка
        return cardinalDirectionsList[Random.Range(0, cardinalDirectionsList.Count)];
    }
}