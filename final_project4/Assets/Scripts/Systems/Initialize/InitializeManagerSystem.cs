using Unity.Entities;

[DisableAutoCreation]
public class InitializeManagerSystem : SystemBase
{

    private InputSystem inputSystem;
    private PlayerTargetSystem playerTargetSystem;
    private DecrementTimeSystem decrementTimeSystem;
    
    protected override void OnCreate()
    {
    }

    protected override void OnStartRunning()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        inputSystem = world.GetOrCreateSystem<InputSystem>();
        playerTargetSystem = world.GetOrCreateSystem<PlayerTargetSystem>();
        decrementTimeSystem = world.GetOrCreateSystem<DecrementTimeSystem>();
    }

    protected override void OnUpdate()
    {
        
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
