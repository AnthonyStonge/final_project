using UnityEngine;

public class StartApplication : MonoBehaviour
{
    void Start()
    {
        //TestHolder.LoadAssets();
        GameInitializer.InitializeSystemWorkflow();
    }
}
