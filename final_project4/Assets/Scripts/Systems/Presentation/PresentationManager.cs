using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class PresentationManager : ComponentSystemGroup
{

    private VisualEventSystem visualEventSystem;
    
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        visualEventSystem = world.GetOrCreateSystem<VisualEventSystem>();

        var presentation = world.GetOrCreateSystem<PresentationManager>();
        
        presentation.AddSystemToUpdateList(visualEventSystem);
    }

    protected override void OnUpdate()
    {
        visualEventSystem.Update();
    }
}
