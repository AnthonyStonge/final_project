using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


[UpdateInGroup(typeof(TransformSystemGroup))]
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
    }

    protected override void OnStartRunning()
    {
        
    }
    
    protected override void OnUpdate()
    {
        moveSystem.Update();
        rotateEnemySystem.Update();
        rotatePlayerSystem.Update();
    }
}
