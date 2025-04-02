using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// базовый класс для генерации подземелья
public abstract class DungeonGeneration : MonoBehaviour
{
    [SerializeField]
    protected TilemapVisualizer tilemapVisualizer = null; // Визуализатор тайлов для отрисовки подземелья
    
    [SerializeField]
    protected Vector2Int start = new Vector2Int(0, 0);

    public void GenerateDungeon() 
    {
        tilemapVisualizer.Clear(); // Очищаем предыдущую карту
        RunProceduralGeneration(); // Запускаем генерацию
    }

    protected abstract void RunProceduralGeneration();
}