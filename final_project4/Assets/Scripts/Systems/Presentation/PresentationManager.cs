using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class PresentationManager : ComponentSystemGroup
{

    private UpdateGunRenderSystem updateGunRenderSystem;
    
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        updateGunRenderSystem = world.GetOrCreateSystem<UpdateGunRenderSystem>();

        var presentation = world.GetOrCreateSystem<PresentationManager>();
        
        presentation.AddSystemToUpdateList(updateGunRenderSystem);
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Presentation Manager Update");
        updateGunRenderSystem.Update();
    }
}
