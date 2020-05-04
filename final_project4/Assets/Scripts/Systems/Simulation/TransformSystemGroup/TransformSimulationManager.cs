using Unity.Entities;

[DisableAutoCreation]
public class TransformSimulationManager : ComponentSystemGroup
{
    private TranslateSystem translateSystem;
    private RotateSystem rotateSystem;
    private ProjectileHitDetectionSystem projectileHitDetectionSystem;
    //private PathFinding pathFinding;
    private PathFollowSystem pathFollowSystem;
    private EnemyFollowSystem enemyFollowSystem;
   // private TestToRenameIfWork testToRenameIfWork;
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        translateSystem = world.GetOrCreateSystem<TranslateSystem>();
        rotateSystem = world.GetOrCreateSystem<RotateSystem>();
        projectileHitDetectionSystem = world.GetOrCreateSystem<ProjectileHitDetectionSystem>();
        //pathFinding = world.GetOrCreateSystem<PathFinding>();
        pathFollowSystem = world.GetOrCreateSystem<PathFollowSystem>();
       // testToRenameIfWork = world.GetOrCreateSystem<TestToRenameIfWork>();
        enemyFollowSystem = world.GetOrCreateSystem<EnemyFollowSystem>();
        
        var transform = world.GetOrCreateSystem<TransformSimulationManager>();
        
        transform.AddSystemToUpdateList(translateSystem);
        transform.AddSystemToUpdateList(rotateSystem);
        transform.AddSystemToUpdateList(projectileHitDetectionSystem);
        //transform.AddSystemToUpdateList(pathFinding);
        transform.AddSystemToUpdateList(pathFollowSystem);
        //transform.AddSystemToUpdateList(testToRenameIfWork);
        transform.AddSystemToUpdateList(enemyFollowSystem);
    }

    protected override void OnUpdate()
    {
        //Debug.Log("TransformSimulation Manager Update");
        //Dependency : None
        rotateSystem.Update();
        //Dependency : RotatePlayerEnemySystem
        translateSystem.Update();
        projectileHitDetectionSystem.Update();
       // testToRenameIfWork.Update();
        //pathFinding.Update();
        pathFollowSystem.Update();
        enemyFollowSystem.Update();
    }
    
    public void OnSwapLevel()
    {
       // pathFinding.InitializeGrid(EventsHolder.LevelEvents.CurrentLevel);
        TemporaryEnemySpawnerSystem.InitializeDefaultEnemySpawn(EventsHolder.LevelEvents.CurrentLevel);
    }
}