using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class GameManager : MonoSingleTon<GameManager>
{
    public GameState gameState { get; private set; }
    public Vector3Int playerTilePos;

    public Tilemap tileMap;
    private TileMapData[,] tilemapInfoArray;

    void Start()
    {
        SetGameState(GameState.Start);
        int x = tileMap.cellBounds.size.x;
        int y = tileMap.cellBounds.size.y;
        tilemapInfoArray = new TileMapData[x, y];
        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                tilemapInfoArray[i, j] = new TileMapData();
            }
        }
    }

    public void SetGameState(GameState state)
    {
        gameState = state;  
    }
    public float GetZRotate(GravityState state)
    {
        switch (state)
        {
            case GravityState.Left:
                return -90f;
            case GravityState.Right:
                return 90f;
            case GravityState.Down:
                return 0f;
            case GravityState.Up:
                return 180f;

            default:
                return 0f;
        }
    }

    public Vector2 GetGravityDirection(GravityState state)
    {
        switch (state)
        {
            case GravityState.Left:
                return Vector2.left;
            case GravityState.Right:
                return Vector2.right;
            case GravityState.Down:
                return Vector2.down;
            case GravityState.Up:
                return Vector2.up;

            default:
                return Vector2.down;
        }
    }

    public bool PaintBlockCheck(int x, int y, GravityState state)
    {
        x = ConversionToTilemapGridPos(x, true);
        y = ConversionToTilemapGridPos(y, false);

        return tilemapInfoArray[x, y].isPaint && state == tilemapInfoArray[x,y].gravityState;
    }

    public void SetPaintBlock(int x, int y, bool isPainted, GravityState state)
    {
        x = ConversionToTilemapGridPos(x, true);
        y = ConversionToTilemapGridPos(y, false);
        tilemapInfoArray[x, y].gravityState = state;
        tilemapInfoArray[x, y].isPaint = isPainted;
    }

    public int ConversionToTilemapGridPos(int pos, bool isPosX)
    {
        return pos + Mathf.Abs(isPosX ? tileMap.cellBounds.xMin : tileMap.cellBounds.yMin);
    }

}
