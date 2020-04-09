using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class PresentationManager : ComponentSystemGroup
{

    private UpdateGunRenderSystem updateGunRenderSystem;
    private CameraFollowSystem cameraFollowSystem;
    
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        updateGunRenderSystem = world.GetOrCreateSystem<UpdateGunRenderSystem>();
        cameraFollowSystem = world.GetOrCreateSystem<CameraFollowSystem>();

        var presentation = world.GetOrCreateSystem<PresentationManager>();
        
        presentation.AddSystemToUpdateList(updateGunRenderSystem);
        presentation.AddSystemToUpdateList(cameraFollowSystem);
    }
    
    protected override void OnStartRunning()
    {
       
    }

    protected override void OnUpdate()
    {
        //Debug.Log("Presentation Manager Update");
        updateGunRenderSystem.Update();
        cameraFollowSystem.Update();
    }
}
