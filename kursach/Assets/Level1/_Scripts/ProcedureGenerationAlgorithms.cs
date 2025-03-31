using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Статический класс, содержащий алгоритмы процедурной генерации
public static class ProceduralGenerationAlgorithms
{
    // Алгоритм случайного блуждания для создания комнат и других областей
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength) 
    {
        // Используем HashSet для хранения уникальных позиций
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        // Добавляем стартовую позицию
        path.Add(startPosition);
        var previousPosition = startPosition;

        // Совершаем заданное количество шагов
        for (int i = 0; i < walkLength; i++) 
        {
            // Получаем новую позицию, двигаясь в случайном направлении
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;
    }

    // Алгоритм создания прямых коридоров
    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength) 
    {
        // Используем List, так как порядок точек важен для коридоров
        List<Vector2Int> corridor = new List<Vector2Int>();
        // Выбираем случайное направление один раз для всего коридора
        var direction = Direction2D.GetRandomCardinalDirection();
        var currentPosition = startPosition;
        corridor.Add(currentPosition);

        // Строим коридор заданной длины в выбранном направлении
        for (int i = 0; i < corridorLength; i++) 
        {
            currentPosition += direction;
            corridor.Add(currentPosition);
        }
        return corridor;
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