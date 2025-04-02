using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Абстрактный базовый класс для генерации подземелий (данжей)
public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null; // Визуализатор тайлов (плиток) для отрисовки подземелья
    
    [SerializeField]
    protected Vector2Int startPosition = Vector2Int.zero; // Стартовая позиция для генерации (по умолчанию (0,0))

    // Основной публичный метод для запуска генерации подземелья
    public void GenerateDungeon() 
    {
        tilemapVisualizer.Clear(); // Очищаем предыдущую карту
        RunProceduralGeneration(); // Запускаем процедурную генерацию
    }

    public void RegenerateEnemies()
    {
        ReplaceAllEnemies();
    }
    // Абстрактный метод, который должен быть реализован в дочерних классах
    // Содержит конкретную логику генерации подземелья
    protected abstract void RunProceduralGeneration();
    protected abstract void ReplaceAllEnemies();
}