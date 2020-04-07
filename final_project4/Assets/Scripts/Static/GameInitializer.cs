using Unity.Entities;
using Unity.Transforms;

public static class GameInitializer 
{
    public static void InitializeSystemWorkflow()
    {
        var world = World.DefaultGameObjectInjectionWorld;
        
        //System Group Handles (From Unity)
        var initialization = world.GetOrCreateSystem<InitializationSystemGroup>(); 
        var transform = world.GetOrCreateSystem<TransformSystemGroup>();
        var simulation = world.GetOrCreateSystem<SimulationSystemGroup>();
        var presentation = world.GetOrCreateSystem<PresentationSystemGroup>();

        //Managers
        var initializeManager = world.GetOrCreateSystem<InitializeManager>();
        var afterInitialization = world.GetOrCreateSystem<LateInitializeManager>();
        
        var transformSimulationManager = world.GetOrCreateSystem<TransformSimulationManager>();
        var lateSimulationManager = world.GetOrCreateSystem<LateSimulationManager>();

        var presentationManager = world.GetOrCreateSystem<PresentationManager>();
        
        //Adding systems
        initialization.AddSystemToUpdateList(initializeManager);
        initialization.AddSystemToUpdateList(afterInitialization);

        transform.AddSystemToUpdateList(transformSimulationManager);
        simulation.AddSystemToUpdateList(lateSimulationManager);
        
        presentation.AddSystemToUpdateList(presentationManager);
        
        //Sorting
        initialization.SortSystemUpdateList();
        transform.SortSystemUpdateList();
        simulation.SortSystemUpdateList();
        presentation.SortSystemUpdateList();
    }
}
