using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class PresentationManager : ComponentSystemGroup
{
    private AnimationSystem animationSystem;
    private SoundEventSystem soundEventSystem;
    private VisualEventSystem visualEventSystem;
    private CleanupSystem cleanupSystem;
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;

        animationSystem = world.GetOrCreateSystem<AnimationSystem>();
        soundEventSystem = world.GetOrCreateSystem<SoundEventSystem>();
        visualEventSystem = world.GetOrCreateSystem<VisualEventSystem>();
        cleanupSystem = world.GetOrCreateSystem<CleanupSystem>();

        var presentation = world.GetOrCreateSystem<PresentationManager>();
        
        presentation.AddSystemToUpdateList(animationSystem);
        presentation.AddSystemToUpdateList(visualEventSystem);
        presentation.AddSystemToUpdateList(soundEventSystem);
        presentation.AddSystemToUpdateList(cleanupSystem);
    }

    protected override void OnUpdate()
    {
        animationSystem.Update();
        soundEventSystem.Update();
        visualEventSystem.Update();
        cleanupSystem.Update();
    }
}
