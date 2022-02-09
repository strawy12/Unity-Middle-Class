using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class GameManager : MonoSingleTon<GameManager>
{
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
    }
}
