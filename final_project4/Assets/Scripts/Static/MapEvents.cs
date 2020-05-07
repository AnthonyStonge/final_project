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
        //entityManager.CompleteAllJobs();

        //Fade out
        GlobalEvents.CameraEvents.FadeOut();
        //Lock player inputs
        if (GameVariables.Player.Entity != Entity.Null)
        {
            //Set delay on Player Weapon (Quick fix for bullets spawning at wrong spot)
            Entity weaponEntity = EventsHolder.LevelEvents.CurrentLevel != MapType.Level_Hell
                ? GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld]
                : GameVariables.Player.PlayerHellWeaponEntities[GameVariables.Player.CurrentWeaponHeld];
            GunComponent weapon = entityManager.GetComponentData<GunComponent>(weaponEntity);
            weapon.SwapTimer = 0.02f;
            entityManager.SetComponentData(weaponEntity, weapon);
            GlobalEvents.PlayerEvents.LockUserInputs();
        }

        EventsHolder.LevelEvents.CurrentLevel = type;
     
        OnSwapLevel();
        TryUnloadMap();
        //Load new map
        CurrentTypeLoaded = type;
        CurrentMapLoaded = entityManager.Instantiate(MapHolder.MapPrefabDict[type]);

        //Set New Spawn Pos if Needed
        if (SetNewSpawnPos)
        {
            if (GameVariables.Player.Entity != Entity.Null)
            {
                GlobalEvents.PlayerEvents.SetPlayerPosition(MapHolder.MapsInfo[type].SpawnPosition);
            }
        }

        //Fade in
        GlobalEvents.CameraEvents.FadeIn();
        //Unlock player inputs
        if (GameVariables.Player.Entity != Entity.Null)
            GlobalEvents.PlayerEvents.UnlockUserInputs();
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