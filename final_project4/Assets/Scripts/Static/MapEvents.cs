using System;
using System.Data;
using Unity.Entities;
using UnityEngine;

public static class MapEvents
{
    private static EntityManager entityManager;

    private static MapType CurrentTypeLoaded;
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

        TryUnloadMap();

        //Load new map
        CurrentTypeLoaded = type;
        CurrentMapLoaded = entityManager.Instantiate(MapHolder.MapPrefabDict[type]);
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

        LoadMap((MapType)idMapToLoad);
    }
    
    public static void LoadNextMap()
    {
        int idMapToLoad = (int) CurrentTypeLoaded + 1;
        
        //If on last level -> return to first one
        if ((int) CurrentTypeLoaded >= Enum.GetNames(typeof(MapType)).Length)
            idMapToLoad = 0;
        
        LoadMap((MapType)idMapToLoad);
    }

    public static void LoadNextMapSafe()
    {
        int idMapToLoad = (int) CurrentTypeLoaded + 1;
        
        //If on last level -> return to first one
        if ((int) CurrentTypeLoaded >= Enum.GetNames(typeof(MapType)).Length)
            idMapToLoad = 0;
        
        LoadMap((MapType)idMapToLoad);
    }
}