using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD
using UnityEngine.UI;
=======
>>>>>>> OIF
using UnityEngine.Tilemaps;

public class GameManager : MonoSingleTon<GameManager>
{
<<<<<<< HEAD
    private GravityState gameGravityState;

    public Vector3Int playerTilePos;

    [SerializeField] public Tilemap tileMap;

    void Start()
    {
        gameGravityState = GravityState.Down;
    }


    public void SetGravityState(GravityState state)
    {
        gameGravityState = state;
        EventManager.TriggerEvent("CHANGEGRAVITYSTATE");
    }

    public float GetZRotate()
    {
        switch (gameGravityState)
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

    public Vector2 GetGravityDirection()
    {
        switch (gameGravityState)
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
=======
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
>>>>>>> OIF
    }
}
