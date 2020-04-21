using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class LateSimulationManager : ComponentSystemGroup
{

    private CameraFollowSystem cameraFollowSystem;

    protected override void OnCreate()
    {        
        var world = World.DefaultGameObjectInjectionWorld;

        cameraFollowSystem = world.GetOrCreateSystem<CameraFollowSystem>();
        var lateSimulation = world.GetOrCreateSystem<LateSimulationManager>();

        lateSimulation.AddSystemToUpdateList(cameraFollowSystem);

    }

    protected override void OnUpdate()
    {
        cameraFollowSystem.Update();

    }

}