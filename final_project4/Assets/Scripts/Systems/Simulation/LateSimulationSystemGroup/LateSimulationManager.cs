using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class LateSimulationManager : ComponentSystemGroup
{

    private AnimationSystem animationSystem;
    private CameraFollowSystem cameraFollowSystem;

    private RetrieveInteractableCollisionsSystem retrieveInteractableCollisionsSystem;

    protected override void OnCreate()
    {        
        var world = World.DefaultGameObjectInjectionWorld;

        animationSystem = world.GetOrCreateSystem<AnimationSystem>();
        cameraFollowSystem = world.GetOrCreateSystem<CameraFollowSystem>();

        retrieveInteractableCollisionsSystem = world.GetOrCreateSystem<RetrieveInteractableCollisionsSystem>();

        var lateSimulation = world.GetOrCreateSystem<LateSimulationManager>();
    
        lateSimulation.AddSystemToUpdateList(animationSystem);
        lateSimulation.AddSystemToUpdateList(cameraFollowSystem);
        lateSimulation.AddSystemToUpdateList(retrieveInteractableCollisionsSystem);
    }

    protected override void OnUpdate()
    {
        animationSystem.Update();
        cameraFollowSystem.Update();
        
        retrieveInteractableCollisionsSystem.Update();
    }
    
    public void OnSwapLevel()
    {
        //Clear previous collision
        retrieveInteractableCollisionsSystem.PreviousFrameCollisions.Clear();
    }
}