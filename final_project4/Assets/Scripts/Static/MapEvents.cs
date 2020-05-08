using System;
using System.Data;
using System.Net;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;

public static class MapEvents
{
    private static EntityManager entityManager;

    public static MapType CurrentTypeLoaded;
    private static Entity CurrentMapLoaded;


    public static void Initialize()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        LoadMap(MapType.LevelMenu, true);
    }

    public static void LoadMap(MapType type, bool SetNewSpawnPos = false)
    {
        //Make sure Dictionary contains Type desired (before unloading current map)
        if (!MapHolder.MapPrefabDict.ContainsKey(type))
        {
#if UNITY_EDITOR
            Debug.LogError($"Map {type} doesn't exist... Staying on current map...");
#endif
            return;
        }

        //Complete all jobs
        entityManager.CompleteAllJobs();

        //Subscribe to FadeOutEvent
        FadeSystem.OnFadeEnd += TryUnloadMap;
        FadeSystem.OnFadeEnd += () => { CurrentMapLoaded = entityManager.Instantiate(MapHolder.MapPrefabDict[type]); };
        //Set Player position
        if (SetNewSpawnPos)
            FadeSystem.OnFadeEnd += () =>
            {
                GlobalEvents.PlayerEvents.SetPlayerPosition(MapHolder.MapsInfo[type].SpawnPosition);
            };

        CurrentTypeLoaded = type;
        EventsHolder.LevelEvents.CurrentLevel = type;

        //Lock User inputs
        GlobalEvents.PlayerEvents.LockUserInputs();

        //Subscribe functions to ChangeWorldEvent
        ChangeWorldDelaySystem.OnChangeWorld += OnSwapLevel;
        ChangeWorldDelaySystem.OnChangeWorld += GlobalEvents.PlayerEvents.UnlockUserInputs;
        ChangeWorldDelaySystem.ChangeWorld(1.5f);
    }

    private static void TryUnloadMap()
    {
        //If map currently loaded -> Unload
        if (CurrentMapLoaded != Entity.Null)
            entityManager.DestroyEntity(CurrentMapLoaded);
    }

    public static void LoadPreviousMap()
    {
        int idMapToLoad = (int) CurrentTypeLoaded - 1;

        //If on last level -> return to first one
        if (idMapToLoad < 0)
            idMapToLoad = Enum.GetNames(typeof(MapType)).Length - 1;

        LoadMap((MapType) idMapToLoad, true);
    }

    public static void LoadNextMap()
    {
        int idMapToLoad = (int) CurrentTypeLoaded + 1;

        //If on last level -> return to first one
        if (idMapToLoad >= Enum.GetNames(typeof(MapType)).Length)
            idMapToLoad = 0;

        LoadMap((MapType) idMapToLoad, true);
    }

    public static void OnSwapLevel()
    {
        World world = World.DefaultGameObjectInjectionWorld;

        GlobalEvents.GameEvents.OnSwapLevel();

        world.GetOrCreateSystem<InitializeManager>().OnSwapLevel();
        world.GetOrCreateSystem<LateInitializeManager>().OnSwapLevel();
        world.GetOrCreateSystem<TransformSimulationManager>().OnSwapLevel();
        world.GetOrCreateSystem<LateSimulationManager>().OnSwapLevel();
        world.GetOrCreateSystem<PresentationManager>().OnSwapLevel();
    }
}