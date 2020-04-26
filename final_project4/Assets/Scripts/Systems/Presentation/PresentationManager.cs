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
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;

        soundEventSystem = world.GetOrCreateSystem<SoundEventSystem>();
        lootSystem = world.GetOrCreateSystem<LootSystem>();
        visualEventSystem = world.GetOrCreateSystem<VisualEventSystem>();
        cleanupSystem = world.GetOrCreateSystem<CleanupSystem>();
        dropSystem = world.GetOrCreateSystem<DropSystem>();

        var presentation = world.GetOrCreateSystem<PresentationManager>();
        presentation.AddSystemToUpdateList(lootSystem);
        presentation.AddSystemToUpdateList(visualEventSystem);
        presentation.AddSystemToUpdateList(cleanupSystem);
        presentation.AddSystemToUpdateList(dropSystem);
    }

    protected override void OnUpdate()
    {
        dropSystem.Update();
        lootSystem.Update();
        soundEventSystem.Update();
        visualEventSystem.Update();
        cleanupSystem.Update();
    }
}
