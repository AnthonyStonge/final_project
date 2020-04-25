using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class PresentationManager : ComponentSystemGroup
{

    private VisualEventSystem visualEventSystem;
    private CleanupSystem cleanupSystem;
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        visualEventSystem = world.GetOrCreateSystem<VisualEventSystem>();
        cleanupSystem = world.GetOrCreateSystem<CleanupSystem>();

        var presentation = world.GetOrCreateSystem<PresentationManager>();
        
        presentation.AddSystemToUpdateList(visualEventSystem);
        presentation.AddSystemToUpdateList(cleanupSystem);
    }

    protected override void OnUpdate()
    {
        visualEventSystem.Update();
        cleanupSystem.Update();
    }
}
