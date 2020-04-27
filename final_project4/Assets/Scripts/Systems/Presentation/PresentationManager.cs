using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class PresentationManager : ComponentSystemGroup
{
    
    private SoundEventSystem soundEventSystem;
    private VisualEventSystem visualEventSystem;
    private CleanupSystem cleanupSystem;
    private DropSystem dropSystem;
    private LootSystem lootSystem;
    private PlayerCollisionSystem playerCollisionSystem;
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;

        
        soundEventSystem = world.GetOrCreateSystem<SoundEventSystem>();
        lootSystem = world.GetOrCreateSystem<LootSystem>();
        visualEventSystem = world.GetOrCreateSystem<VisualEventSystem>();
        cleanupSystem = world.GetOrCreateSystem<CleanupSystem>();
        dropSystem = world.GetOrCreateSystem<DropSystem>();
        playerCollisionSystem = world.GetOrCreateSystem<PlayerCollisionSystem>();

        var presentation = world.GetOrCreateSystem<PresentationManager>();
        presentation.AddSystemToUpdateList(playerCollisionSystem);
        presentation.AddSystemToUpdateList(lootSystem);
        presentation.AddSystemToUpdateList(visualEventSystem);
        presentation.AddSystemToUpdateList(soundEventSystem);
        presentation.AddSystemToUpdateList(cleanupSystem);
        presentation.AddSystemToUpdateList(dropSystem);
    }

    protected override void OnUpdate()
    {
        playerCollisionSystem.Update();
        dropSystem.Update();
        lootSystem.Update();
        soundEventSystem.Update();
        visualEventSystem.Update();
        cleanupSystem.Update();
    }
}
