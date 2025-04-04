using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine;

// Класс генератора подземелий, использующий алгоритм случайного блуждания
public class DungeonGenerator : Generation
{
    [SerializeField]
    protected SimpleRandomWalkSO randomWalkParameters; // ScriptableObject с параметрами генерации

    public static List<Vector2Int> directions = new List<Vector2Int> {new Vector2Int(0, 1), new Vector2Int(1, 0),
        new Vector2Int(0, -1), new Vector2Int(-1, 0)};
    // Основной метод генерации, переопределяющий абстрактный метод из родительского класса
    protected override void RunGeneration() 
    {
        // 1. Генерируем позиции пола с помощью алгоритма случайного блуждания
        List<Vector2Int> floorPositions = RunRandomWalk(randomWalkParameters, start);
        
        // 2. Очищаем предыдущую карту
        tilemapVisualizer.Clear();
        
        // 3. Отрисовываем тайлы пола
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        
        // 4. Генерируем стены вокруг пола
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer); 
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
    protected List<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position) 
    {
        Vector2Int current = position; // Текущая стартовая позиция
        List<Vector2Int> floorPositions = new List<Vector2Int>(); // Коллекция для хранения всех позиций пола
        
        // Выполняем заданное количество итераций
        for (int i = 0; i < parameters.iterations; i++) {
            // Генерируем путь случайного блуждания
            List<Vector2Int> road = RandomWalk(current, parameters.walkLength);
            
            // Добавляем все позиции из пути в общую коллекцию
            floorPositions.AddRange(road.Except(floorPositions));
        }
        return floorPositions;
    }
}
