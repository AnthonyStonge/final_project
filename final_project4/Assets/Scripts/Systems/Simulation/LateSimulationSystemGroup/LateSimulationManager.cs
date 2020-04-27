using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class LateSimulationManager : ComponentSystemGroup
{

    private AnimationSystem animationSystem;
    private CameraFollowSystem cameraFollowSystem;
    private ProjectileHitDetectionSystem projectileHitDetectionSystem;
    private UISystem uiSystem;
    protected override void OnCreate()
    {        
        var world = World.DefaultGameObjectInjectionWorld;

        animationSystem = world.GetOrCreateSystem<AnimationSystem>();
        cameraFollowSystem = world.GetOrCreateSystem<CameraFollowSystem>();
        projectileHitDetectionSystem = world.GetOrCreateSystem<ProjectileHitDetectionSystem>();
        uiSystem = world.GetOrCreateSystem<UISystem>();

        var lateSimulation = world.GetOrCreateSystem<LateSimulationManager>();
    
        lateSimulation.AddSystemToUpdateList(animationSystem);
        lateSimulation.AddSystemToUpdateList(cameraFollowSystem);
        lateSimulation.AddSystemToUpdateList(projectileHitDetectionSystem);
        lateSimulation.AddSystemToUpdateList(uiSystem);
    }

    protected override void OnUpdate()
    {
        animationSystem.Update();
        cameraFollowSystem.Update();
        projectileHitDetectionSystem.Update();
        uiSystem.Update();
    }
}