using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
[AlwaysUpdateSystem]
public class InitializeManager : ComponentSystemGroup
{
    private PathFinding pathFinding;
    private InputSystem inputSystem;
    private SwapWeaponSystem swapWeaponSystem;
    private PlayerTargetSystem playerTargetSystem;

    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        inputSystem = world.GetOrCreateSystem<InputSystem>();
        swapWeaponSystem = world.GetOrCreateSystem<SwapWeaponSystem>();
        playerTargetSystem = world.GetOrCreateSystem<PlayerTargetSystem>();
        pathFinding= world.GetOrCreateSystem<PathFinding>();
        
        var initialize = world.GetOrCreateSystem<InitializeManager>();
        
        initialize.AddSystemToUpdateList(pathFinding);
        initialize.AddSystemToUpdateList(inputSystem);
        initialize.AddSystemToUpdateList(swapWeaponSystem);
        initialize.AddSystemToUpdateList(playerTargetSystem);
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
            
            pathFinding.Update();
            
            swapWeaponSystem.Update();
            
            //Dependency: InputSystem
            playerTargetSystem.Update();
        }
    }


    protected override void OnDestroy()
    {
    }
}
