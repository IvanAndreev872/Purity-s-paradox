using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

// Класс для визуализации тайлов на Tilemap в Unity
public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap, wallTilemap; // Ссылки на Tilemap для пола и стен
    [SerializeField]
    private TileBase floorTile, wallTop; // Тайлы для пола и верхней части стен

    // Метод для отрисовки тайлов пола
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions) 
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    // Общий метод для отрисовки множества тайлов
    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile) 
    {
        foreach (var position in positions) 
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    // Метод для отрисовки одной базовой стены (верхней части)
    internal void PaintSingleBasicWall(Vector2Int position) 
    {
        PaintSingleTile(wallTilemap, wallTop, position);
    }

    // Основной метод для отрисовки одного тайла
    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position) 
    {
        // Конвертируем координаты Vector2Int в позицию тайла
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        // Устанавливаем тайл на указанную позицию
        tilemap.SetTile(tilePosition, tile);
    }

    // Метод для очистки всех тайлов
    public void Clear() {
        wallTilemap.ClearAllTiles(); // Очищаем тайлы стен
        floorTilemap.ClearAllTiles(); // Очищаем тайлы пола
    }
}