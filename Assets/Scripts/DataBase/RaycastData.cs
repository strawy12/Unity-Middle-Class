using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RaycastData
{
    public GravityState detectType;
    public bool isdetected;
    public float detectDistance;
    public Vector3 hitPos;

    public RaycastData(GravityState type)
    {
        detectType = type;
        isdetected = false;
        detectDistance = 0f;
        hitPos = Vector3.zero;
    }
}
