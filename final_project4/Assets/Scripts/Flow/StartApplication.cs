using System;
using UnityEngine;

public class StartApplication : MonoBehaviour
{
    public Camera CurrentCamera;

    void Start()
    {
        GameInitializer.SetMainCamera(CurrentCamera);
        GameInitializer.InitializeSystemWorkflow();
    }

    private void OnDestroy()
    {
        GameInitializer.OnDestroy();
    }
}