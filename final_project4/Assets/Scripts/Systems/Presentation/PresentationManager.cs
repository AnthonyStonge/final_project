using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class PresentationManager : ComponentSystemGroup
{
    private StateEventSystem stateEventSystem;
    private AnimationEventSystem animationEventSystem;
    
    private SoundEventSystem soundEventSystem;
    private VisualEventSystem visualEventSystem;
    
    private DropSystem dropSystem;
    private LootSystem lootSystem;
    
    private PlayerCollisionSystem playerCollisionSystem;
    
    private UISystem uiSystem;
    
    private CleanupSystem cleanupSystem;
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;


        stateEventSystem = world.GetOrCreateSystem<StateEventSystem>();
        animationEventSystem = world.GetOrCreateSystem<AnimationEventSystem>();
        
        soundEventSystem = world.GetOrCreateSystem<SoundEventSystem>();
        lootSystem = world.GetOrCreateSystem<LootSystem>();
        visualEventSystem = world.GetOrCreateSystem<VisualEventSystem>();
        cleanupSystem = world.GetOrCreateSystem<CleanupSystem>();
        dropSystem = world.GetOrCreateSystem<DropSystem>();
        playerCollisionSystem = world.GetOrCreateSystem<PlayerCollisionSystem>();
        uiSystem = world.GetOrCreateSystem<UISystem>();

        var presentation = world.GetOrCreateSystem<PresentationManager>();
        presentation.AddSystemToUpdateList(stateEventSystem);
        presentation.AddSystemToUpdateList(animationEventSystem);
        
        presentation.AddSystemToUpdateList(playerCollisionSystem);
        presentation.AddSystemToUpdateList(lootSystem);
        presentation.AddSystemToUpdateList(visualEventSystem);
        presentation.AddSystemToUpdateList(soundEventSystem);
        presentation.AddSystemToUpdateList(cleanupSystem);
        presentation.AddSystemToUpdateList(dropSystem);
        presentation.AddSystemToUpdateList(uiSystem);
    }

    protected override void OnUpdate()
    {
        stateEventSystem.Update();
        animationEventSystem.Update();
        
        uiSystem.Update();
        playerCollisionSystem.Update();
        dropSystem.Update();
        lootSystem.Update();
        soundEventSystem.Update();
        visualEventSystem.Update();
        cleanupSystem.Update();
    }
}
