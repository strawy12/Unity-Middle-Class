using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TileMapData 
{
    public bool isPaint = false;
    public GravityState gravityState;
    
    public TileMapData()
    {
        gravityState = GravityState.None;
        isPaint = false;
    }
    public TileMapData(GravityState state)
    {
        gravityState = state;
        isPaint = true;
    }
}
