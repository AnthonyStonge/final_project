﻿using System;
using System.Data;
using Unity.Entities;
using UnityEngine;

public static class MapEvents
{
    private static EntityManager entityManager;

    public static MapType CurrentTypeLoaded;
    private static Entity CurrentMapLoaded;


    public static void Initialize()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        LoadMap(MapType.LevelMenu);
    }

    public static void LoadMap(MapType type)
    {
        //Make sure Dictionary contains Type desired (before unloading current map)
        if (!MapHolder.MapPrefabDict.ContainsKey(type))
        {
            Debug.LogError($"Map {type} doesn't exist... Staying on current map...");
            return;
        }
        
        //Complete all jobs
        entityManager.CompleteAllJobs();
        //Clear previous collision
        World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<RetrieveInteractableCollisionsSystem>()
            .PreviousFrameCollisions.Clear();

        //Fade out
        GlobalEvents.CameraEvents.FadeOut();
        //Lock player inputs
        if (GameVariables.Player.Entity != Entity.Null)
            GlobalEvents.PlayerEvents.LockUserInputs();

        TryUnloadMap();

        //Load new map
        CurrentTypeLoaded = type;
        CurrentMapLoaded = entityManager.Instantiate(MapHolder.MapPrefabDict[type]);

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

        LoadMap((MapType) idMapToLoad);
    }

    public static void LoadNextMap()
    {
        int idMapToLoad = (int) CurrentTypeLoaded + 1;

        //If on last level -> return to first one
        if (idMapToLoad >= Enum.GetNames(typeof(MapType)).Length)
            idMapToLoad = 0;

        LoadMap((MapType) idMapToLoad);
    }
}