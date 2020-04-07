using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public class TransformSimulationManager : SystemBase
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

        var presentation = world.GetOrCreateSystem<PresentationSystemGroup>();
        
        presentation.AddSystemToUpdateList(moveSystem);
        presentation.AddSystemToUpdateList(rotateEnemySystem);
        presentation.AddSystemToUpdateList(rotatePlayerSystem);
    }

    protected override void OnStartRunning()
    {
        
    }
    
    protected override void OnUpdate()
    {
        //Dependency : None
        rotateEnemySystem.Update();
        //Dependency : None
        rotatePlayerSystem.Update();
        //Dependency : RotatePlayerEnemySystem
        moveSystem.Update();
    }
}
