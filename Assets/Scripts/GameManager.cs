using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoSingleTon<GameManager>
{
    [SerializeField] private Tilemap tilemap;
    private bool[,] tilemapInfoArray;
    void Start()
    {
        int x = tilemap.cellBounds.size.x;
        int y = tilemap.cellBounds.size.y;
        tilemapInfoArray = new bool[x, y];
    }

    public bool PaintBlockCheck(int x, int y)
    {
        x = ConversionToTilemapGridPos(x, true);
        y = ConversionToTilemapGridPos(y, false);

        return tilemapInfoArray[x, y];
    }

    public void SetPaintBlock(int x, int y, bool isPainted)
    {
        x = ConversionToTilemapGridPos(x, true);
        y = ConversionToTilemapGridPos(y, false);

        tilemapInfoArray[x, y] = isPainted;
    }

    public int ConversionToTilemapGridPos(int pos, bool isPosX)
    {
        return pos + Mathf.Abs(isPosX ? tilemap.cellBounds.xMin : tilemap.cellBounds.yMin);
    }
}
