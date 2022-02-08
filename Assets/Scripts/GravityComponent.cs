using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityComponent : MonoBehaviour
{
    public GravityState detectType;

    public Vector2Int targetDistance;

    public bool isDetected = false;

    private void Update()
    {
        if(!isDetected)
        {
            return;
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("°¨Áö");
        if (collision.gameObject.CompareTag("Gravity"))
        {
            GravityBlock();
            isDetected = true;
        }
        else
        {
            isDetected = false;
        }
    }

    void OnTriggerExit2D()
    {
        if(isDetected) 
        { 
            isDetected = false;
        }
    }

    private void GravityBlock()
    {
        GameManager.Inst.SetGravityState(detectType);
    }


}
