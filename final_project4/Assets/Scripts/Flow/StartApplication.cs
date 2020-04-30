using System;
using System.Collections;
using Unity.Entities;
using UnityEngine;

public class StartApplication : MonoBehaviour
{
    private bool HasInit;
    void Start()
    {
        GameInitializer.LoadAssets();    
    }

    private void Update()
    {
        //This is not working
        if (GameInitializer.IsLoadingFinished() && !HasInit)
        {
            HasInit = true;
            GameInitializer.InitializeSystemWorkflow();
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        GameInitializer.OnDestroy();
    }
}