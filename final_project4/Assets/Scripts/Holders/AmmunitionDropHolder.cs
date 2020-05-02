using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static ECSUtility;

public static class AmmunitionDropHolder
{
    public static ConcurrentDictionary<DropType, Entity> DropItemPrefabDict;

    private static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;

    public static void Initialize()
    {
        DropItemPrefabDict = new ConcurrentDictionary<DropType, Entity>();

        currentNumberOfLoadedAssets = 0;
        numberOfAssetsToLoad = Enum.GetNames(typeof(DropType)).Length;
    }

    public static void LoadAssets()
    {
        foreach (var i in Enum.GetNames(typeof(DropType)))
        {
            Addressables.LoadAssetAsync<GameObject>(i).Completed += obj =>
            {
                DropItemPrefabDict.TryAdd((DropType) Enum.Parse(typeof(DropType), i),
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
        return  currentNumberOfLoadedAssets / (float)numberOfAssetsToLoad;
    }

    public static void OnDestroy()
    {
        blobAssetStores.ForEach(i=>{ i.Dispose(); });
    }
}
