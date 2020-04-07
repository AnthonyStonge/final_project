using Unity.Entities;

[DisableAutoCreation]
public class InitializeManagerSystem : SystemBase
{

    private InputSystem inputSystem;
    private PlayerTargetSystem playerTargetSystem;
    private EnemyTargetSystem enemyTargetSystem;
    private DecrementTimeSystem decrementTimeSystem;
    
    protected override void OnCreate()
    {
    }

    protected override void OnStartRunning()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        inputSystem = world.GetOrCreateSystem<InputSystem>();
        playerTargetSystem = world.GetOrCreateSystem<PlayerTargetSystem>();
        enemyTargetSystem = world.GetOrCreateSystem<EnemyTargetSystem>();
        decrementTimeSystem = world.GetOrCreateSystem<DecrementTimeSystem>();
    }

    protected override void OnUpdate()
    {
        
        if (GameVariables.InputEnabled)
        {
            inputSystem.Update();
        }
        playerTargetSystem.Update();
        enemyTargetSystem.Update();
        decrementTimeSystem.Update();
    }


    protected override void OnDestroy()
    {
    }
}
