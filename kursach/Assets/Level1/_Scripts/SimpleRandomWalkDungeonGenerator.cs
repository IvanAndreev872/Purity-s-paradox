using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Random = UnityEngine.Random;
using UnityEngine;

// Класс генератора подземелий, использующий алгоритм случайного блуждания
public class DungeonGenerator : DungeonGeneration
{
    [SerializeField]
    protected SimpleRandomWalkSO randomWalkParameters; // ScriptableObject с параметрами генерации
    
    // Основной метод генерации, переопределяющий абстрактный метод из родительского класса
    protected override void RunProceduralGeneration() 
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

    // Метод, реализующий алгоритм случайного блуждания с заданными параметрами
    protected List<Vector2Int> RunRandomWalk(SimpleRandomWalkSO parameters, Vector2Int position) 
    {
        var currentPosition = position; // Текущая стартовая позиция
        List<Vector2Int> floorPositions = new List<Vector2Int>(); // Коллекция для хранения всех позиций пола
        
        // Выполняем заданное количество итераций
        for (int i = 0; i < parameters.iterations; i++) {
            // Генерируем путь случайного блуждания
            var path = GenerationAlgorithm.RandomWalk(currentPosition, parameters.walkLength);
            
            // Добавляем все позиции из пути в общую коллекцию
            floorPositions.AddRange(path.Except(floorPositions));
            
            // Если нужно начинать каждую итерацию со случайной позиции
            if (parameters.startRandomlyEachIteration) 
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        return floorPositions;
    }

    protected int GetRoomLength(List<Vector2Int> floorPositions)
    {
        floorPositions.OrderBy(x => x.x);
        Vector2Int LeftFloor = floorPositions.First();
        Vector2Int RightFloor = floorPositions.Last();

        return RightFloor.x - LeftFloor.x + 1;
    }

    protected int GetRoomHeight(List<Vector2Int> floorPositions)
    {
        floorPositions.OrderBy(x => x.y);
        Vector2Int LowFloor = floorPositions.First();
        Vector2Int HighFloor = floorPositions.Last();

        return HighFloor.y - LowFloor.y + 1;
    }

    protected int GetLeft(List<Vector2Int> floorPositions)
    {
        Vector2Int Left = floorPositions.OrderBy(x => x.x).First();

        return Left.x;
    }

    protected int GetLowest(List<Vector2Int> floorPositions)
    {
        Vector2Int Bottom = floorPositions.OrderBy(x => x.y).First();

        return Bottom.y;
    }
}
