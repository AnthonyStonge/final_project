using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class PresentationManager : SystemBase
{

    private UpdateGunRenderSystem updateGunRenderSystem;
    
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        updateGunRenderSystem = world.GetOrCreateSystem<UpdateGunRenderSystem>();

        var presentation = world.GetOrCreateSystem<PresentationSystemGroup>();
        
        presentation.AddSystemToUpdateList(updateGunRenderSystem);
    }
    
    protected override void OnStartRunning()
    {
       
    }

    protected override void OnUpdate()
    {
        updateGunRenderSystem.Update();
    }
}
