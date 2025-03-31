using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

// Генератор подземелий, который сначала создает коридоры, а затем комнаты
public class CorridorFirstDungeonGenerator : NewBehaviourScript
{
    [SerializeField]
    private int corridorLength = 14; // Длина каждого коридора
    [SerializeField]
    private int corridorCount = 5;   // Количество коридоров
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f; // Процент потенциальных позиций комнат, которые будут использованы

    // Основной метод генерации
    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }

    // Основная логика генерации (сначала коридоры, потом комнаты)
    private void CorridorFirstGeneration() 
    {
        // Все позиции пола
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        // Потенциальные позиции для комнат (концы коридоров)
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();
        
        // 1. Создаем коридоры
        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        // 2. Создаем комнаты в случайных точках
        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        // 3. Находим тупики (концы коридоров без комнат)
        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        // 4. Создаем комнаты в тупиках
        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        // Объединяем все позиции пола
        floorPositions.UnionWith(roomPositions);

        // 5. Расширяем коридоры (делаем их шире)
        for (int i = 0; i < corridorCount; i++) 
        {
            corridors[i] = IncreaseCorridorBrush3by3(corridors[i]);
            floorPositions.UnionWith(corridors[i]);
        }

        // Визуализируем пол и стены
        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    // Создание комнат в тупиковых точках
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

    // Поиск всех тупиков (позиций с только одним соседом)
    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions) 
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();

        foreach (var position in floorPositions) 
        {
            int neighboursCount = 0;
            // Проверяем 4 направления (верх, право, низ, лево)
            foreach (var direction in Direction2D.cardinalDirectionsList) 
            {
                if (floorPositions.Contains(position + direction)) 
                    neighboursCount++;
            }
            // Если только 1 сосед - это тупик
            if (neighboursCount == 1)
                deadEnds.Add(position);
        }
        return deadEnds;
    }

    // Создание комнат в случайных точках
    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions) 
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        // Вычисляем количество комнат на основе процента
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        // Выбираем случайные позиции для комнат
        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
        
        // Генерируем комнаты алгоритмом случайного блуждания
        foreach (var roomPosition in roomsToCreate) 
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            roomPositions.UnionWith(roomFloor);
        }

        return roomPositions;
    }

    // Создание коридоров
    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions) 
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);
        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

        // Создаем указанное количество коридоров
        for (int i = 0; i < corridorCount; i++) 
        {
            // Генерируем коридор алгоритмом случайного блуждания
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            corridors.Add(corridor);
            // Перемещаем текущую позицию в конец коридора
            currentPosition = corridor[corridor.Count - 1];
            // Добавляем конечную точку как потенциальную позицию для комнаты
            potentialRoomPositions.Add(currentPosition);
            // Добавляем коридор в общий набор позиций
            floorPositions.UnionWith(corridor);
        }
        return corridors;
    }

    // Увеличение коридора на 1 клетку в каждую сторону (устаревший метод)
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
                // Добавляем 3x3 блок на поворотах
                for (int x = -1; x < 2; x++) 
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
                // Добавляем клетки по бокам прямых участков
                Vector2Int newCorridorTileOffset = GetDirection90From(directionFromCell);
                newCorridor.Add(corridor[i - 1]);
                newCorridor.Add(corridor[i - 1] + newCorridorTileOffset);
            }
        }
        return newCorridor;
    }

    // Увеличение коридора до размера 3x3 (текущий метод)
    public List<Vector2Int> IncreaseCorridorBrush3by3(List<Vector2Int> corridor) 
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();
        for (int i = 1; i < corridor.Count; i++) 
        {
            // Добавляем 3x3 блок для каждой точки коридора
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

    // Получение перпендикулярного направления
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
}