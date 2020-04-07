using Unity.Entities;

public static class GameInitializer 
{
    public static void InitializeSystemWorkflow()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        var initialization = world.GetOrCreateSystem<InitializationSystemGroup>();
        var simulation = world.GetOrCreateSystem<SimulationSystemGroup>();
        var presentation = world.GetOrCreateSystem<PresentationSystemGroup>();

        var initializeManager = world.GetOrCreateSystem<InitializeManager>();

        var beforeTransformManager = world.GetOrCreateSystem<BeforeTransformManager>();
        var transformSimulationManager = world.GetOrCreateSystem<TransformSimulationManager>();
        var lateSimulationManager = world.GetOrCreateSystem<LateSimulationManager>();

        var presentationManager = world.GetOrCreateSystem<PresentationManager>();
        
        initialization.AddSystemToUpdateList(initializeManager);
        
        simulation.AddSystemToUpdateList(beforeTransformManager);
        simulation.AddSystemToUpdateList(transformSimulationManager);
        simulation.AddSystemToUpdateList(lateSimulationManager);
        
        presentation.AddSystemToUpdateList(presentationManager);
        
        initialization.SortSystemUpdateList();
        simulation.SortSystemUpdateList();
        presentation.SortSystemUpdateList();
    }
}
