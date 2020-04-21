using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class LateSimulationManager : ComponentSystemGroup
{

    private CameraFollowSystem cameraFollowSystem;
    private ProjectileHitDetectionSystem projectileHitDetectionSystem;

    protected override void OnCreate()
    {        
        var world = World.DefaultGameObjectInjectionWorld;

        cameraFollowSystem = world.GetOrCreateSystem<CameraFollowSystem>();
        projectileHitDetectionSystem = world.GetOrCreateSystem<ProjectileHitDetectionSystem>();
        
        var lateSimulation = world.GetOrCreateSystem<LateSimulationManager>();

        lateSimulation.AddSystemToUpdateList(cameraFollowSystem);
        lateSimulation.AddSystemToUpdateList(projectileHitDetectionSystem);
    }

    protected override void OnUpdate()
    {
        cameraFollowSystem.Update();
        projectileHitDetectionSystem.Update();
    }
}