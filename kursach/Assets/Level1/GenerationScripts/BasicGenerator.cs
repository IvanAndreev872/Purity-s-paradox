using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// базовый класс для генерации подземелья
public abstract class Generation : MonoBehaviour
{
    [SerializeField]
    protected TilemapRenderer tileRenderer = null; // Визуализатор тайлов для отрисовки подземелья
    
    [SerializeField]
    protected Vector2Int start = new Vector2Int(0, 0);

    public void GenerateDungeon() 
    {
        tileRenderer.ClearSpace(); // Очищаем предыдущую карту
        RunGeneration(); // Запускаем генерацию
    }

    public abstract void RunGeneration();
}