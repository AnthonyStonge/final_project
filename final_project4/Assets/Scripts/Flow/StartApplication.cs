using System;
using System.Collections;
using UnityEngine;

public class StartApplication : MonoBehaviour
{
    private bool test = false;
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
            //ok
            Debug.Log("Wtf bro");
            GameInitializer.InitializeSystemWorkflow();
            gameObject.SetActive(false);
            
        }
    }

    private void OnDestroy()
    {
        GameInitializer.OnDestroy();
    }
}