using Unity.Entities;
using UnityEngine;

public class GameLogic : IStateLogic
{
    public GameLogic()
    {
        Debug.Log("OnCreate GameLogic");
    }

    public void Enable()
    {
        Debug.Log("Enable GameLogic Systems");

        var world = World.DefaultGameObjectInjectionWorld;
        world.GetExistingSystem<LateInitializeManager>().Enabled = true;
        world.GetExistingSystem<LateSimulationManager>().Enabled = true;
        world.GetExistingSystem<TransformSimulationManager>().Enabled = true;
    }

    public void Disable()
    {
        Debug.Log("Disable GameLogic Systems");

        var world = World.DefaultGameObjectInjectionWorld;

        world.GetExistingSystem<LateInitializeManager>().Enabled = false;
        world.GetExistingSystem<LateSimulationManager>().Enabled = false;
        world.GetExistingSystem<TransformSimulationManager>().Enabled = false;
    }

    public void Initialize()
    {
        PlayerInitializer.Initialize();
        WeaponInitializer.Initialize();
        
        Debug.Log("Initialize GameLogic Systems");
    }

    public void Destroy()
    {
        //Delete or reset everything tied to GameLogic
        Debug.Log("Destroy GameLogic Systems");
    }
}