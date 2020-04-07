using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(SimulationSystemGroup))]
[UpdateBefore(typeof(TransformSystemGroup))]
public class BeforeTransformManager : SystemBase
{

    private UpdatePlayerStateSystem updatePlayerStateSystem;
    private StateIdleSystem stateIdleSystem;
    private StateMovingSystem stateMovingSystem;
    private StateAttackingSystem stateAttackingSystem;
    private EnemyTargetSystem enemyTargetSystem;
    private StateDyingSystem stateDyingSystem;
    private StateDashingSystem stateDashingSystem;
    
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;

        updatePlayerStateSystem = world.GetOrCreateSystem<UpdatePlayerStateSystem>();
        stateIdleSystem = world.GetOrCreateSystem<StateIdleSystem>();
        stateMovingSystem = world.GetOrCreateSystem<StateMovingSystem>();
        stateAttackingSystem = world.GetOrCreateSystem<StateAttackingSystem>();
        enemyTargetSystem = world.GetOrCreateSystem<EnemyTargetSystem>();
        stateDyingSystem = world.GetOrCreateSystem<StateDyingSystem>();
        stateDashingSystem = world.GetOrCreateSystem<StateDashingSystem>();
    }

    protected override void OnStartRunning()
    {
        #region AI&Behaviours
        //Dependency : None
        updatePlayerStateSystem.Update();
        //Dependency : None
        enemyTargetSystem.Update();
        //Dependency : None
        stateIdleSystem.Update();
        //Dependency : EnemyTargetSystem, StateIdleSystem
        stateMovingSystem.Update();
        //Dependency : StateMovingSystem
        stateAttackingSystem.Update();
        //Dependency : StateMovingSystem, UpdatePlayerStateSystem
        stateDyingSystem.Update();
        #endregion
        //Dependency : StateDyingSystem
        stateDashingSystem.Update();
    }

    protected override void OnUpdate()
    {
    }
}
