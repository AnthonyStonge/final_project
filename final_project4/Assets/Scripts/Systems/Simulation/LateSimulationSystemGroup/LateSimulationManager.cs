using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class LateSimulationManager : ComponentSystemGroup
{

    private CameraFollowSystem cameraFollowSystem;
    private ProjectileHitDetectionSystem projectileHitDetectionSystem;
    private UiSystem uiSystem;
    protected override void OnCreate()
    {        
        var world = World.DefaultGameObjectInjectionWorld;

        cameraFollowSystem = world.GetOrCreateSystem<CameraFollowSystem>();
        projectileHitDetectionSystem = world.GetOrCreateSystem<ProjectileHitDetectionSystem>();
        uiSystem = world.GetOrCreateSystem<UiSystem>();
        
        var lateSimulation = world.GetOrCreateSystem<LateSimulationManager>();

        lateSimulation.AddSystemToUpdateList(cameraFollowSystem);
        lateSimulation.AddSystemToUpdateList(projectileHitDetectionSystem);
        lateSimulation.AddSystemToUpdateList(uiSystem);
    }

    protected override void OnUpdate()
    {
        cameraFollowSystem.Update();
        projectileHitDetectionSystem.Update();
        uiSystem.Update();
    }
}