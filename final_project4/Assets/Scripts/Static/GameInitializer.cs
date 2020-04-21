using Static.Events;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public static class GameInitializer
{
    public static void InitializeSystemWorkflow()
    {
        //Init archetypes (must be done before creating any entities)*
        StaticArchetypes.InitializeArchetypes();
        
        GameVariables.EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        PlayerHolder.Initialize();
        EnemyHolder.Initialize();
        WeaponHolder.Initialize();
        //ProjectileHolder.Initialize();

        //Event init
        PlayerEvents.Initialize();
        GunEvents.Initialize();

        //Init holder?
        //TODO change Singleton to holders
        GameVariables.PlayerVars.Default = MonoGameVariables.instance.playerAssets;
        GameVariables.PlayerVars.Dash = MonoGameVariables.instance.playerDashAssets;
        // GameVariables.PlayerVars.Pistol = MonoGameVariables.instance.playerPistolAssets;
        GameVariables.PlayerVars.Default.PlayerAudioSource = MonoGameVariables.instance.playerAudioSource;

        //Init map
        //TODO change this to a more appropriate name.
        MapInitializer.Initialize();

        InitializeSystems();
    }

    private static void InitializeSystems()
    {
        var world = World.DefaultGameObjectInjectionWorld;

        //System Group Handles (From Unity)
        var initialization = world.GetOrCreateSystem<InitializationSystemGroup>();
        var transform = world.GetOrCreateSystem<TransformSystemGroup>();
        var lateSimulation = world.GetOrCreateSystem<LateSimulationSystemGroup>();
        var presentation = world.GetOrCreateSystem<PresentationSystemGroup>();

        var gameLogicSystem = world.GetOrCreateSystem<GameLogicSystem>();

        //Managers
        var initializeManager = world.GetOrCreateSystem<InitializeManager>();
        var afterInitialization = world.GetOrCreateSystem<LateInitializeManager>();

        var transformSimulationManager = world.GetOrCreateSystem<TransformSimulationManager>();
        var lateSimulationManager = world.GetOrCreateSystem<LateSimulationManager>();

        var presentationManager = world.GetOrCreateSystem<PresentationManager>();

        //Adding Managers as systems
        initialization.AddSystemToUpdateList(initializeManager);
        initialization.AddSystemToUpdateList(afterInitialization);

        transform.AddSystemToUpdateList(transformSimulationManager);
        initialization.AddSystemToUpdateList(lateSimulationManager);

        presentation.AddSystemToUpdateList(presentationManager);

        //Sorting
        initialization.SortSystemUpdateList();
        transform.SortSystemUpdateList();
        lateSimulation.SortSystemUpdateList();
        presentation.SortSystemUpdateList();
    }

    public static void OnDestroy()
    {
        EventsHolder.OnDestroy();

        //Holders
        PlayerHolder.OnDestroy();
        EnemyHolder.OnDestroy();
        WeaponHolder.OnDestroy();
        ProjectileHolder.OnDestroy();
    }

    public static void SetMainCamera(Camera cam)
    {
        GameVariables.MainCamera = cam;
    }
}