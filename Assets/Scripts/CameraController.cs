using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    private void Start()
    {
        EventManager.StartListening("CHANGEGRAVITYSTATE", RotateCamera);
    }

    private void RotateCamera()
    {
        Vector3 rot = Vector3.zero;

       // rot.z = GameManager.Inst.GetZRotate();

        transform.DORotate(rot, .5f);
    }
}
