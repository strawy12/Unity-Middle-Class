using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleTon<GameManager>
{
    private GravityState gameGravityState;

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
}
