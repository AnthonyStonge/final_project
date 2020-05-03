using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class PresentationManager : ComponentSystemGroup
{
    private StateEventSystem stateEventSystem;
    private AnimationEventSystem animationEventSystem;

    private InteractableEventSystem interactableEventSystem;
    
    private SoundEventSystem soundEventSystem;
    private VisualEventSystem visualEventSystem;
    
    private DropSystem dropSystem;
    private LootSystem lootSystem;

    private UISystem uiSystem;

    private GlobalEventListenerSystem globalEventListenerSystem;
    private CleanupSystem cleanupSystem;
    protected override void OnCreate()
    {
        var world = World.DefaultGameObjectInjectionWorld;


        stateEventSystem = world.GetOrCreateSystem<StateEventSystem>();
        animationEventSystem = world.GetOrCreateSystem<AnimationEventSystem>();
        
        interactableEventSystem = world.GetOrCreateSystem<InteractableEventSystem>();
        
        soundEventSystem = world.GetOrCreateSystem<SoundEventSystem>();
        lootSystem = world.GetOrCreateSystem<LootSystem>();
        visualEventSystem = world.GetOrCreateSystem<VisualEventSystem>();
        cleanupSystem = world.GetOrCreateSystem<CleanupSystem>();
        dropSystem = world.GetOrCreateSystem<DropSystem>();
        uiSystem = world.GetOrCreateSystem<UISystem>();

        globalEventListenerSystem = world.GetOrCreateSystem<GlobalEventListenerSystem>();

        var presentation = world.GetOrCreateSystem<PresentationManager>();
        
        presentation.AddSystemToUpdateList(stateEventSystem);
        presentation.AddSystemToUpdateList(animationEventSystem);
        presentation.AddSystemToUpdateList(interactableEventSystem);
        presentation.AddSystemToUpdateList(lootSystem);
        presentation.AddSystemToUpdateList(visualEventSystem);
        presentation.AddSystemToUpdateList(soundEventSystem);
        presentation.AddSystemToUpdateList(cleanupSystem);
        presentation.AddSystemToUpdateList(dropSystem);
        presentation.AddSystemToUpdateList(uiSystem);
        presentation.AddSystemToUpdateList(globalEventListenerSystem);
        
        presentation.SortSystemUpdateList();
    }

    protected override void OnUpdate()
    {
        stateEventSystem.Update();
        animationEventSystem.Update();
        
        interactableEventSystem.Update();
        
        uiSystem.Update();
        dropSystem.Update();
        lootSystem.Update();
        soundEventSystem.Update();
        visualEventSystem.Update();
        
        globalEventListenerSystem.Update();
        cleanupSystem.Update();
    }
    
    public void OnSwapLevel()
    {
        
    }
}
