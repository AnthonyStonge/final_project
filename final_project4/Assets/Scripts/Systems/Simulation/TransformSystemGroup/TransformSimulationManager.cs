using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public class TransformSimulationManager : ComponentSystemGroup
{

    private MoveSystem moveSystem;
    private RotateEnemySystem rotateEnemySystem;
    private RotatePlayerSystem rotatePlayerSystem;
    
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;

        moveSystem = world.GetOrCreateSystem<MoveSystem>();
        rotateEnemySystem = world.GetOrCreateSystem<RotateEnemySystem>();
        rotatePlayerSystem = world.GetOrCreateSystem<RotatePlayerSystem>();

        var transform = world.GetOrCreateSystem<TransformSimulationManager>();
        
        transform.AddSystemToUpdateList(moveSystem);
        transform.AddSystemToUpdateList(rotateEnemySystem);
        transform.AddSystemToUpdateList(rotatePlayerSystem);
    }

    protected override void OnStartRunning()
    {
        
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
    }
}
