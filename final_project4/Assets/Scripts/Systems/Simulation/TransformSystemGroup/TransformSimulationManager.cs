using Unity.Entities;

[DisableAutoCreation]
public class TransformSimulationManager : ComponentSystemGroup
{
    private MoveSystem moveSystem;
    private RotateEnemySystem rotateEnemySystem;
    private RotatePlayerSystem rotatePlayerSystem;
    private ProjectileHitDetectionSystem projectileHitDetectionSystem;
    private PathFinding pathFinding;
    private PathFollowSystem pathFollowSystem;
    private EnnemieFollowSystem ennemieFollowSystem;
    private TestToRenameIfWork testToRenameIfWork;
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        moveSystem = world.GetOrCreateSystem<MoveSystem>();
        rotateEnemySystem = world.GetOrCreateSystem<RotateEnemySystem>();
        rotatePlayerSystem = world.GetOrCreateSystem<RotatePlayerSystem>();
        projectileHitDetectionSystem = world.GetOrCreateSystem<ProjectileHitDetectionSystem>();
        pathFinding = world.GetOrCreateSystem<PathFinding>();
        pathFollowSystem = world.GetOrCreateSystem<PathFollowSystem>();
        testToRenameIfWork = world.GetOrCreateSystem<TestToRenameIfWork>();
        ennemieFollowSystem = world.GetOrCreateSystem<EnnemieFollowSystem>();
        
        var transform = world.GetOrCreateSystem<TransformSimulationManager>();
        
        transform.AddSystemToUpdateList(moveSystem);
        transform.AddSystemToUpdateList(rotateEnemySystem);
        transform.AddSystemToUpdateList(rotatePlayerSystem);
        transform.AddSystemToUpdateList(projectileHitDetectionSystem);
        transform.AddSystemToUpdateList(pathFinding);
        transform.AddSystemToUpdateList(pathFollowSystem);
        transform.AddSystemToUpdateList(testToRenameIfWork);
        transform.AddSystemToUpdateList(ennemieFollowSystem);
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
        testToRenameIfWork.Update();
        pathFinding.Update();
        pathFollowSystem.Update();
        ennemieFollowSystem.Update();
    }
    
    public void OnSwapLevel()
    {
        
    }
}