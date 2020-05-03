using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class LateSimulationManager : ComponentSystemGroup
{

    private AnimationSystem animationSystem;
    private CameraFollowSystem cameraFollowSystem;

    private RetrieveInteractableCollisionsSystem retrieveInteractableCollisionsSystem;
    private PlayerCollisionSystem playerCollisionSystem;

    protected override void OnCreate()
    {        
        var world = World.DefaultGameObjectInjectionWorld;

        animationSystem = world.GetOrCreateSystem<AnimationSystem>();
        cameraFollowSystem = world.GetOrCreateSystem<CameraFollowSystem>();

        retrieveInteractableCollisionsSystem = world.GetOrCreateSystem<RetrieveInteractableCollisionsSystem>();
        playerCollisionSystem = world.GetOrCreateSystem<PlayerCollisionSystem>();

        var lateSimulation = world.GetOrCreateSystem<LateSimulationManager>();
    
        lateSimulation.AddSystemToUpdateList(animationSystem);
        lateSimulation.AddSystemToUpdateList(cameraFollowSystem);
        lateSimulation.AddSystemToUpdateList(retrieveInteractableCollisionsSystem);
        lateSimulation.AddSystemToUpdateList(playerCollisionSystem);
        
        lateSimulation.SortSystemUpdateList();
    }

    protected override void OnUpdate()
    {
        animationSystem.Update();
        cameraFollowSystem.Update();
        
        retrieveInteractableCollisionsSystem.Update();
        playerCollisionSystem.Update();
    }
    
    public void OnSwapLevel()
    {
        //Clear previous collision
        retrieveInteractableCollisionsSystem.PreviousFrameCollisions.Clear();
    }
}