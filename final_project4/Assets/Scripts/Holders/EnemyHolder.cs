using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Enums;
using UnityEngine.AddressableAssets;
using static ECSUtility;

public static class EnemyHolder
{
    public static ConcurrentDictionary<EnemyType, Entity> EnemyPrefabDict;
    
    public static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;
    
    public static void Initialize()
    {
        EnemyPrefabDict = new ConcurrentDictionary<EnemyType, Entity>();
        
        currentNumberOfLoadedAssets = 0;
        numberOfAssetsToLoad = Enum.GetNames(typeof(EnemyType)).Length;
    }

    public static void LoadAssets()
    {
        foreach (var i in Enum.GetNames(typeof(EnemyType)))
        {
            Addressables.LoadAssetAsync<GameObject>(i).Completed += obj =>
            {
                EnemyPrefabDict.TryAdd((EnemyType) Enum.Parse(typeof(EnemyType), i), 
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
        blobAssetStores.ForEach(i=>{ i.Dispose(); });
    }
}