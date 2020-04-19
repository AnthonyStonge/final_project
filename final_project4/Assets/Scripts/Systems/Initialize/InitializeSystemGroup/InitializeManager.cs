using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
[AlwaysUpdateSystem]
public class InitializeManager : ComponentSystemGroup
{
    private InputSystem inputSystem;
    private SwapWeaponSystem swapWeaponSystem;
    private PlayerTargetSystem playerTargetSystem;
    private DecrementTimeSystem decrementTimeSystem;
    private UpdatePlayerInfoSystem updatePlayerInfoSystem;
    
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        inputSystem = world.GetOrCreateSystem<InputSystem>();
        swapWeaponSystem = world.GetOrCreateSystem<SwapWeaponSystem>();
        playerTargetSystem = world.GetOrCreateSystem<PlayerTargetSystem>();
        decrementTimeSystem = world.GetOrCreateSystem<DecrementTimeSystem>();
        updatePlayerInfoSystem = world.GetOrCreateSystem<UpdatePlayerInfoSystem>();
        
        var initialize = world.GetOrCreateSystem<InitializeManager>();
        initialize.AddSystemToUpdateList(inputSystem);
        initialize.AddSystemToUpdateList(swapWeaponSystem);
        initialize.AddSystemToUpdateList(playerTargetSystem);
        initialize.AddSystemToUpdateList(decrementTimeSystem);
        initialize.AddSystemToUpdateList(updatePlayerInfoSystem);
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
            
            swapWeaponSystem.Update();
            
            //Dependency: InputSystem
            playerTargetSystem.Update();
        }
        //Dependency: None 
        decrementTimeSystem.Update();
        updatePlayerInfoSystem.Update();
    }


    protected override void OnDestroy()
    {
    }
}
