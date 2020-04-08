using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public class LateInitializeManager : SystemBase
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

        var initialize = world.GetOrCreateSystem<InitializationSystemGroup>();
        
        initialize.AddSystemToUpdateList(updatePlayerStateSystem);
        initialize.AddSystemToUpdateList(stateIdleSystem);
        initialize.AddSystemToUpdateList(stateMovingSystem);
        initialize.AddSystemToUpdateList(stateAttackingSystem);
        initialize.AddSystemToUpdateList(enemyTargetSystem);
        initialize.AddSystemToUpdateList(stateDyingSystem);
        initialize.AddSystemToUpdateList(stateDashingSystem);
    }

    protected override void OnStartRunning()
    {
        
    }

    protected override void OnUpdate()
    {
        //Debug.Log("LateInitialize Manager Update");
        #region AI&Behaviours
        //Dependency : PlayerTargetSystem, DecrementTimeSystem
        updatePlayerStateSystem.Update();
        //Dependency : PlayerTargetSystem, DecrementTimeSystem
        enemyTargetSystem.Update();
        //Dependency : PlayerTargetSystem, DecrementTimeSystem
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
}
