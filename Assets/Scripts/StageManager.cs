using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoSingleTon<StageManager>
{
    [SerializeField]
    public GameObject stagePrefab;
    public GameObject stage;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void ReStart()
    {
        Destroy(stage);
        stage = Instantiate(stagePrefab);
        Time.timeScale = 1;
    }
}
