using UnityEngine;

public class StartApplication : MonoBehaviour
{

    public Camera CurrentCamera;
    
    void Start()
    {
        GameInitializer.InitializeSystemWorkflow();

        GameVariables.MainCamera = CurrentCamera;
    }
}
