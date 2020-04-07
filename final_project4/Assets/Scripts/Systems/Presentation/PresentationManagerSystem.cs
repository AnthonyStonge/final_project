using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(PresentationSystemGroup))]
public class PresentationManagerSystem : SystemBase
{

    private UpdateGunRenderSystem updateGunRenderSystem;
    
    protected override void OnUpdate()
    {
        
    }

    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        updateGunRenderSystem = world.GetOrCreateSystem<UpdateGunRenderSystem>();
    }

    protected override void OnStartRunning()
    {
        updateGunRenderSystem.Update();
    }
}
