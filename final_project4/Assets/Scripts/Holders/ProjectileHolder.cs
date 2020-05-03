using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Physics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static ECSUtility;

//TODO DO WE REALLY NEED THIS BECAUSE WHEN CONVERTING PREFABS WEAPON, IT CONVERT ITS BULLET WITH IT...
//TODO UNLESS WE MAKE BULLET THAT DONT HAVE WEAPON, WE SHOULD REMOVE THIS CLASS
public static class ProjectileHolder
{
    public static ConcurrentDictionary<ProjectileType, Entity> ProjectilePrefabDict = new ConcurrentDictionary<ProjectileType, Entity>();
    public static ConcurrentDictionary<ProjectileType, CollisionFilter> ProjectileFilters = new ConcurrentDictionary<ProjectileType, CollisionFilter>();
    
    private static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();
    private static int currentNumberOfLoadedAssets = 0;
    private static int numberOfAssetsToLoad = 1;

    public static void Initialize()
    {
    }

    public static void LoadAssets()
    {
        Addressables.LoadAssetAsync<BulletPrefabsContainer>("BulletPrefabsContainer").Completed += handle =>
        {
            // ExtractPrefabs(handle.Result);
            currentNumberOfLoadedAssets++;
        };
    }

    private static void ExtractPrefabs(BulletPrefabsContainer container)
    {
        foreach (BulletPrefabsContainer.BulletLink bullet in container.Prefabs)
        {
            //Convert
            Entity e = ConvertGameObjectPrefab(bullet.Prefab, out BlobAssetStore blob);

            if (e != Entity.Null)
                if(!ProjectilePrefabDict.TryAdd(bullet.Type, e))
                    Debug.Log($"Couldn't add bullet type {bullet.Type}");
            
            //Create filter
            CollisionFilter filter = new CollisionFilter
            {
                BelongsTo = bullet.Filter.BelongsTo.Value,
                CollidesWith = bullet.Filter.CollidesWith.Value
            };
            if(!ProjectileFilters.TryAdd(bullet.Type, filter))
                Debug.Log($"Couldnt add filter for type {bullet.Type}");
            
            if(blob != null)
                blobAssetStores.Add(blob);
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