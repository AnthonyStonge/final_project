using Unity.Entities;

[DisableAutoCreation]
public class TransformSimulationManager : ComponentSystemGroup
{
    private MoveSystem moveSystem;
    private RotateEnemySystem rotateEnemySystem;
    private RotatePlayerSystem rotatePlayerSystem;
    private ProjectileHitDetectionSystem projectileHitDetectionSystem;
    private PathFollowSystem pathFollowSystem;
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        moveSystem = world.GetOrCreateSystem<MoveSystem>();
        rotateEnemySystem = world.GetOrCreateSystem<RotateEnemySystem>();
        rotatePlayerSystem = world.GetOrCreateSystem<RotatePlayerSystem>();
        projectileHitDetectionSystem = world.GetOrCreateSystem<ProjectileHitDetectionSystem>();
        pathFollowSystem = world.GetOrCreateSystem<PathFollowSystem>();
        
        var transform = world.GetOrCreateSystem<TransformSimulationManager>();
        
        transform.AddSystemToUpdateList(moveSystem);
        transform.AddSystemToUpdateList(rotateEnemySystem);
        transform.AddSystemToUpdateList(rotatePlayerSystem);
        transform.AddSystemToUpdateList(projectileHitDetectionSystem);
        transform.AddSystemToUpdateList(pathFollowSystem);
    }

    protected override void OnUpdate()
    {
        //Debug.Log("TransformSimulation Manager Update");
        //Dependency : None
        rotateEnemySystem.Update();
        //Dependency : None
        rotatePlayerSystem.Update();
        //Dependency : RotatePlayerEnemySystem
        moveSystem.Update();
        projectileHitDetectionSystem.Update();
        pathFollowSystem.Update();
    }
}