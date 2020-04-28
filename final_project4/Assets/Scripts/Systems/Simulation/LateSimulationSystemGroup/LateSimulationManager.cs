using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class LateSimulationManager : ComponentSystemGroup
{

    private AnimationSystem animationSystem;
    private CameraFollowSystem cameraFollowSystem;
    protected override void OnCreate()
    {        
        var world = World.DefaultGameObjectInjectionWorld;

        animationSystem = world.GetOrCreateSystem<AnimationSystem>();
        cameraFollowSystem = world.GetOrCreateSystem<CameraFollowSystem>();

        var lateSimulation = world.GetOrCreateSystem<LateSimulationManager>();
    
        lateSimulation.AddSystemToUpdateList(animationSystem);
        lateSimulation.AddSystemToUpdateList(cameraFollowSystem);
    }

    protected override void OnUpdate()
    {
        animationSystem.Update();
        cameraFollowSystem.Update();
    }
}