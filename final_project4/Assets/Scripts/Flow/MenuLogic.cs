using UnityEngine;

public class MenuLogic : IStateLogic
{
    public MenuLogic()
    {
        Debug.Log("On Create MenuLogic");
    }
    
    public void Enable()
    {

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