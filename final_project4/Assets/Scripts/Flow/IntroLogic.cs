using UnityEngine;

public class IntroLogic : IStateLogic
{
    public IntroLogic()
    {
       // Debug.Log("On Create IntroLogic");
    }

    public void Enable()
    {
      //  Debug.Log("Enable IntroLogic Systems");
    }

    public void Disable()
    {
       // Debug.Log("Disable IntroLogic Systems");
    }

    public void Initialize()
    {
       // Debug.Log("Initialize IntroLogic Systems");
    }

    public void Destroy()
    {
      //  Debug.Log("Destroy IntroLogic Systems");
    }
}