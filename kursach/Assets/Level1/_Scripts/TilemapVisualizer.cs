using System;
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
    private TileBase floor1, floor2, floor3, floor4, floor5, floor6, top, right, left, down, full,
    wallInnerCornerDownLeft, wallInnerCornerDownRight, 
    wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;

    // Метод для отрисовки тайлов пола
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions) 
    {
        TileBase[] floorTiles = {floor1, floor2, floor3, floor4, floor5, floor6};
    
        foreach (Vector2Int position in floorPositions)
        {
            // Выбираем случайный тайл из массива
            TileBase randomTile = floorTiles[UnityEngine.Random.Range(0, 6)];
            PaintSingleTile(floorTilemap, randomTile, position);
        }
    }

    // Метод для отрисовки одной базовой стены (верхней части)
    internal void PaintSingleBasicWall(Vector2Int position, string bin) 
    {
        int typeAsInt = Convert.ToInt32(bin, 2);
        TileBase tile = null;
        if (TopCheck.Contains(typeAsInt)) 
        {
            tile = top;
        } else if (RightCheck.Contains(typeAsInt)) 
        {
            tile = right;
        } else if (LeftCheck.Contains(typeAsInt)) 
        {
            tile = left;
        } else if (DownCheck.Contains(typeAsInt)) 
        {
            tile = down;
        } else if (FullCheck.Contains(typeAsInt)) 
        {
            tile = full;
        }
        if (tile != null) {
            PaintSingleTile(wallTilemap, tile, position);
        }
    }

    // Основной метод для отрисовки одного тайла
    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position) 
    {
        // Конвертируем координаты Vector2Int в позицию тайла
        Vector3Int tilePosition = tilemap.WorldToCell((Vector3Int)position);
        // Устанавливаем тайл на указанную позицию
        tilemap.SetTile(tilePosition, tile);
    }

    // Метод для очистки всех тайлов
    public void Clear() {
        wallTilemap.ClearAllTiles(); // Очищаем тайлы стен
        floorTilemap.ClearAllTiles(); // Очищаем тайлы пола
    }

    internal void PaintSingleCornerWall(Vector2Int position, string bin) 
    {
        int typeAsInt = Convert.ToInt32(bin, 2);
        TileBase tile = null;

        if (wallInnerCornerDownLeftCheck.Contains(typeAsInt)) 
        {
            tile = wallInnerCornerDownLeft;
        }
        else if (wallInnerCornerDownRightCheck.Contains(typeAsInt)) 
        {
            tile = wallInnerCornerDownRight;
        }
        else if (wallDiagonalCornerDownLeftCheck.Contains(typeAsInt)) 
        {
            tile = wallDiagonalCornerDownLeft;
        }
        else if (wallDiagonalCornerDownRightCheck.Contains(typeAsInt)) 
        {
            tile = wallDiagonalCornerDownRight;
        } 
        else if (wallDiagonalCornerUpRightCheck.Contains(typeAsInt)) 
        {
            tile = wallDiagonalCornerUpRight;
        }
        else if (wallDiagonalCornerUpLeftCheck.Contains(typeAsInt)) 
        {
            tile = wallDiagonalCornerUpLeft;
        }
        else if (wallFullEightDirectionsCheck.Contains(typeAsInt)) 
        {
            tile = full;
        }
        else if (wallDownEightDirectionsCheck.Contains(typeAsInt)) 
        {
            tile = down;
        }

        if (tile != null) 
        {
            PaintSingleTile(wallTilemap, tile, position);
        }
    }

    public static List<int> TopCheck = new List<int>
    {
        0b1111,
        0b0110,
        0b0011,
        0b0010,
        0b1010,
        0b1100,
        0b1110,
        0b1011,
        0b0111
    };

    public static List<int> LeftCheck = new List<int>
    {
        0b0100
    };

    public static List<int> RightCheck = new List<int>
    {
        0b0001
    };

    public static List<int> DownCheck = new List<int>
    {
        0b1000
    };

    public static List<int> wallInnerCornerDownLeftCheck = new List<int>
    {
        0b11110001,
        0b11100000,
        0b11110000,
        0b11100001,
        0b10100000,
        0b01010001,
        0b11010001,
        0b01100001,
        0b11010000,
        0b01110001,
        0b00010001,
        0b10110001,
        0b10100001,
        0b10010000,
        0b00110001,
        0b10110000,
        0b00100001,
        0b10010001
    };

    public static List<int> wallInnerCornerDownRightCheck = new List<int>
    {
        0b11000111,
        0b11000011,
        0b10000011,
        0b10000111,
        0b10000010,
        0b01000101,
        0b11000101,
        0b01000011,
        0b10000101,
        0b01000111,
        0b01000100,
        0b11000110,
        0b11000010,
        0b10000100,
        0b01000110,
        0b10000110,
        0b11000100,
        0b01000010

    };

    public static List<int> wallDiagonalCornerDownLeftCheck = new List<int>
    {
        0b01000000
    };

    public static List<int> wallDiagonalCornerDownRightCheck = new List<int>
    {
        0b00000001
    };

    public static List<int> wallDiagonalCornerUpLeftCheck = new List<int>
    {
        0b00010000,
        0b01010000,
    };

    public static List<int> wallDiagonalCornerUpRightCheck = new List<int>
    {
        0b00000100,
        0b00000101
    };

    public static List<int> FullCheck = new List<int>
    {
        0b1101,
        0b0101,
        0b1101,
        0b1001

    };

    public static List<int> wallFullEightDirectionsCheck = new List<int>
    {
        0b00010100,
        0b11100100,
        0b10010011,
        0b01110100,
        0b00010111,
        0b00010110,
        0b00110100,
        0b00010101,
        0b01010100,
        0b00010010,
        0b00100100,
        0b00010011,
        0b01100100,
        0b10010111,
        0b11110100,
        0b10010110,
        0b10110100,
        0b11100101,
        0b11010011,
        0b11110101,
        0b11010111,
        0b11010111,
        0b11110101,
        0b01110101,
        0b01010111,
        0b01100101,
        0b01010011,
        0b01010010,
        0b00100101,
        0b00110101,
        0b01010110,
        0b11010101,
        0b11010100,
        0b10010101

    };

    public static List<int> wallDownEightDirectionsCheck = new List<int>
    {
        0b01000001
    };

}