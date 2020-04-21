using System;
using System.Collections;
using UnityEngine;

public class StartApplication : MonoBehaviour
{
    private bool test;
    void Start()
    {
        GameInitializer.LoadAssets();    
    }

    private void Update()
    {
        //This is not working
        if (GameInitializer.IsLoadingFinished() && !test)
        {
            test = true;
            GameInitializer.InitializeSystemWorkflow();
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameInitializer.OnDestroy();
    }
}