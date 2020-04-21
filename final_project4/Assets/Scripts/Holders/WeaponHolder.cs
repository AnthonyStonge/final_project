using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static ECSUtility;

public static class WeaponHolder
{
    public static ConcurrentDictionary<WeaponType, Entity> WeaponDict;

    private static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;

    public static void Initialize()
    {
        WeaponDict = new ConcurrentDictionary<WeaponType, Entity>();

        currentNumberOfLoadedAssets = 0;
        numberOfAssetsToLoad = Enum.GetNames(typeof(WeaponType)).Length;
    }

    public static void LoadAssets()
    {
        foreach (var i in Enum.GetNames(typeof(WeaponType)))
        {
            Addressables.LoadAssetAsync<GameObject>(i).Completed += obj =>
            {
                WeaponDict.TryAdd((WeaponType) Enum.Parse(typeof(WeaponType), i),
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