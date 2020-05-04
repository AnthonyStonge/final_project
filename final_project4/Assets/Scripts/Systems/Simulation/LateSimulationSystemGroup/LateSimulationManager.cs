using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class LateSimulationManager : ComponentSystemGroup
{

    private AnimationSystem animationSystem;
    private CameraFollowSystem cameraFollowSystem;

    private RetrieveInteractableCollisionsSystem retrieveInteractableCollisionsSystem;
    private PlayerCollisionSystem playerCollisionSystem;

    private TemporaryEnemySpawnerSystem temporaryEnemySpawnerSystem;

    protected override void OnCreate()
    {        
        var world = World.DefaultGameObjectInjectionWorld;

        animationSystem = world.GetOrCreateSystem<AnimationSystem>();
        cameraFollowSystem = world.GetOrCreateSystem<CameraFollowSystem>();

        retrieveInteractableCollisionsSystem = world.GetOrCreateSystem<RetrieveInteractableCollisionsSystem>();
        playerCollisionSystem = world.GetOrCreateSystem<PlayerCollisionSystem>();
        temporaryEnemySpawnerSystem = world.GetOrCreateSystem<TemporaryEnemySpawnerSystem>();

        var lateSimulation = world.GetOrCreateSystem<LateSimulationManager>();
    
        lateSimulation.AddSystemToUpdateList(animationSystem);
        lateSimulation.AddSystemToUpdateList(cameraFollowSystem);
        lateSimulation.AddSystemToUpdateList(retrieveInteractableCollisionsSystem);
        lateSimulation.AddSystemToUpdateList(playerCollisionSystem);
        lateSimulation.AddSystemToUpdateList(temporaryEnemySpawnerSystem);
        
        lateSimulation.SortSystemUpdateList();
    }

    protected override void OnUpdate()
    {
        animationSystem.Update();
        cameraFollowSystem.Update();
        
        retrieveInteractableCollisionsSystem.Update();
        playerCollisionSystem.Update();
        temporaryEnemySpawnerSystem.Update();
    }
    
    public void OnSwapLevel()
    {
        //Clear previous collision
        retrieveInteractableCollisionsSystem.PreviousFrameCollisions.Clear();
    }
}