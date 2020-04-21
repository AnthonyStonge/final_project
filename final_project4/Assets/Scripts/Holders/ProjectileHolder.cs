using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static ECSUtility;

//TODO DO WE REALLY NEED THIS BECAUSE WHEN CONVERTING PREFABS WEAPON, IT CONVERT ITS BULLET WITH IT...
//TODO UNLESS WE MAKE BULLET THAT DONT HAVE WEAPON, WE SHOULD REMOVE THIS CLASS
public static class ProjectileHolder
{
    public static ConcurrentDictionary<ProjectileType, Entity> ProjectilePrefabDict;
    
    private static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;

    public static void Initialize()
    {
        // ConvertPrefabs();
        ProjectilePrefabDict = new ConcurrentDictionary<ProjectileType, Entity>();
       
        currentNumberOfLoadedAssets = 0;
        numberOfAssetsToLoad = Enum.GetNames(typeof(ProjectileType)).Length;
    }
    
    public static void LoadAssets()
    {
        foreach (var i in Enum.GetNames(typeof(ProjectileType)))
        {
            Addressables.LoadAssetAsync<GameObject>(i).Completed += obj =>
            {
                ProjectilePrefabDict.TryAdd((ProjectileType) Enum.Parse(typeof(ProjectileType), i),
                    ConvertGameObjectPrefab(obj.Result, out BlobAssetStore blob));
                currentNumberOfLoadedAssets++;
                if (blob != null)
                {
                    blobAssetStores.Add(blob);
                }
            };
        }
    }
    
    public static float CurrentLoadingPercentage()
    {
        return (float) currentNumberOfLoadedAssets / numberOfAssetsToLoad;
    }
    
    public static void OnDestroy()
    {
        blobAssetStores.ForEach(i=> { i.Dispose(); });
    }    
}