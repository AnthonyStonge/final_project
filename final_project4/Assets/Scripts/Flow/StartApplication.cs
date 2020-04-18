using System;
using UnityEngine;

public class StartApplication : MonoBehaviour
{
    public Camera CurrentCamera;

    private int PlayerObjectsToLoad;
    private int EnemyObjectsToLoad;
    private int ProjectileObjectsToLoad;

    void Start()
    {
       // GameInitializer.SetMainCamera(CurrentCamera);
       // GameInitializer.InitializeSystemWorkflow();
       
      // PlayerObjectsToLoad = PlayerHolder.
       
    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        GameInitializer.OnDestroy();
    }
}