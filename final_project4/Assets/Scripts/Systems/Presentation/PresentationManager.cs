using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public class PresentationManager : SystemBase
{

    private UpdateGunRenderSystem updateGunRenderSystem;
    
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        updateGunRenderSystem = world.GetOrCreateSystem<UpdateGunRenderSystem>();
    }
    
    protected override void OnStartRunning()
    {
       
    }

    protected override void OnUpdate()
    {
        updateGunRenderSystem.Update();
    }
}
