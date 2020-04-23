using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
[AlwaysUpdateSystem]
public class InitializeManager : ComponentSystemGroup
{
    private InputSystem inputSystem;
    private SwapWeaponSystem swapWeaponSystem;
    private PlayerTargetSystem playerTargetSystem;

    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        inputSystem = world.GetOrCreateSystem<InputSystem>();
        swapWeaponSystem = world.GetOrCreateSystem<SwapWeaponSystem>();
        playerTargetSystem = world.GetOrCreateSystem<PlayerTargetSystem>();

        var initialize = world.GetOrCreateSystem<InitializeManager>();
        
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
            
            swapWeaponSystem.Update();
            
            //Dependency: InputSystem
            playerTargetSystem.Update();
        }
        
        //TODO REMOVE
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            //Debug.Log("Fade in...");
            GlobalEvents.FadeIn();
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            //Debug.Log("Fade out...");
            GlobalEvents.FadeOut();
        }
    }


    protected override void OnDestroy()
    {
    }
}
