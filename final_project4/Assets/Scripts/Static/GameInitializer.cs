using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public static class GameInitializer 
{
    public static void InitializeSystemWorkflow()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        var presentation = world.GetOrCreateSystem<PresentationSystemGroup>();
        var simulation = world.GetOrCreateSystem<SimulationSystemGroup>();
        var initialization = world.GetOrCreateSystem<InitializationSystemGroup>();

        var initializeManagerSystem = world.GetOrCreateSystem<InitializeManagerSystem>();

        var beforeTransformManager = world.GetOrCreateSystem<BeforeTransformManager>();
        var transformSimulationManager = world.GetOrCreateSystem<TransformSimulationManager>();
        var lateSimulationManager = world.GetOrCreateSystem<LateSimulationManager>();

        var presentationManager = world.GetOrCreateSystem<PresentationManager>();
        
        initialization.SortSystemUpdateList();
        
    }
}
