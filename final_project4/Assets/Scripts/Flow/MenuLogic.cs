using UnityEngine;

public class MenuLogic : IStateLogic
{
    public MenuLogic()
    {
        Debug.Log("On Create MenuLogic");
    }
    
    public void Enable()
    {
        Debug.Log("Enable MenuLogic Systems");
    }

    public void Disable()
    {
        Debug.Log("Disable MenuLogic Systems");
    }

    public void Initialize()
    {
        Debug.Log("Initialize MenuLogic Systems");
    }

    public void Destroy()
    {
        Debug.Log("Destroy MenuLogic Systems");
    }
}