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
    public static ConcurrentDictionary<WeaponType, Entity> WeaponPrefabDict = new ConcurrentDictionary<WeaponType, Entity>();

    private static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();
    private static int currentNumberOfLoadedAssets = 0;
    private static int numberOfAssetsToLoad = 1;

    public static void Initialize()
    {
    }

    public static void LoadAssets()
    {
        Addressables.LoadAssetAsync<WeaponPrefabContainer>("WeaponPrefabContainer").Completed += handle =>
        {
            ExtractPrefab(handle.Result);
            currentNumberOfLoadedAssets++;
        };
    }

    private static void ExtractPrefab(WeaponPrefabContainer container)
    {
        foreach (WeaponPrefabContainer.PrefabLink gun in container.Prefabs)
        {
            //Convert
            Entity weaponEntity = ConvertGameObjectPrefab(gun.Prefab, out BlobAssetStore blob);

            if(!WeaponPrefabDict.TryAdd(gun.Type, weaponEntity))
                Debug.Log($"Couldnt add weapon prefab of type {gun.Type}");
            
            if(blob != null)
                blobAssetStores.Add(blob);
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