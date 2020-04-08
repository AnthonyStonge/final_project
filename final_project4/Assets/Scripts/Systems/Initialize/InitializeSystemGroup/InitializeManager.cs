using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
[AlwaysUpdateSystem]
public class InitializeManager : SystemBase
{
    private InputSystem inputSystem;
    private PlayerTargetSystem playerTargetSystem;
    private DecrementTimeSystem decrementTimeSystem;
    
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        inputSystem = world.GetOrCreateSystem<InputSystem>();
        playerTargetSystem = world.GetOrCreateSystem<PlayerTargetSystem>();
        decrementTimeSystem = world.GetOrCreateSystem<DecrementTimeSystem>();
        
        var initialize = world.GetOrCreateSystem<InitializationSystemGroup>();

        initialize.AddSystemToUpdateList(inputSystem);
        initialize.AddSystemToUpdateList(playerTargetSystem);
        initialize.AddSystemToUpdateList(decrementTimeSystem);
    }

    protected override void OnStartRunning()
    {
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Initialize Manager Update");
        if (GameVariables.InputEnabled)
        {
            //Dependency: None   
            inputSystem.Update();
            //Dependency: InputSystem
            playerTargetSystem.Update();
        }
        //Dependency: None 
        decrementTimeSystem.Update();
    }


    protected override void OnDestroy()
    {
    }
}
