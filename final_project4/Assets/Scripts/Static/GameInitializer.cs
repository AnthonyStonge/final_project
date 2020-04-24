﻿using Unity.Entities;
using Unity.Transforms;

public static class GameInitializer
{
    public delegate float LoadingStatus();

    public static LoadingStatus OnLoadingStatus;
    
    public static void LoadAssets()
    {
        PlayerHolder.Initialize();
        EnemyHolder.Initialize();
        WeaponHolder.Initialize();
        ProjectileHolder.Initialize();
        SoundHolder.Initialize();
        VisualEffectHolder.Initialize();

        OnLoadingStatus += PlayerHolder.CurrentLoadingPercentage;
        OnLoadingStatus += EnemyHolder.CurrentLoadingPercentage;
        OnLoadingStatus += WeaponHolder.CurrentLoadingPercentage;
        OnLoadingStatus += ProjectileHolder.CurrentLoadingPercentage;
        OnLoadingStatus += SoundHolder.CurrentLoadingPercentage;
        OnLoadingStatus += VisualEffectHolder.CurrentLoadingPercentage;
        
        PlayerHolder.LoadAssets();
        EnemyHolder.LoadAssets();
        WeaponHolder.LoadAssets();
        ProjectileHolder.LoadAssets();
        SoundHolder.LoadAssets();
        VisualEffectHolder.LoadAssets();
    }
    
    public static void InitializeSystemWorkflow()
    {
        GameVariables.EntityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        
        //GameVariables.PlayerVars.Default.PlayerAudioSource = MonoGameVariables.instance.playerAudioSource;
        //Game Initializer?
        
        PlayerInitializer.Initialize();
        WeaponInitializer.Initialize();

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

        //var gameLogicSystem = world.GetOrCreateSystem<GameLogicSystem>();

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
        lateSimulation.AddSystemToUpdateList(lateSimulationManager);

        presentation.AddSystemToUpdateList(presentationManager);

        //Sorting
        initialization.SortSystemUpdateList();
        transform.SortSystemUpdateList();
        lateSimulation.SortSystemUpdateList();
        presentation.SortSystemUpdateList();
    }

    public static bool IsLoadingFinished()
    {
        float loadingPercentage = 0;
        foreach (var i in OnLoadingStatus.GetInvocationList())
        {
            loadingPercentage += ((LoadingStatus) i).Invoke();
        }

        if (loadingPercentage >= OnLoadingStatus.GetInvocationList().Length)
        {
            return true;
        }
        return false;
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
}