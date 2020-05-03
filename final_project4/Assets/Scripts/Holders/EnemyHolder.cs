using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Enums;
using UnityEngine.AddressableAssets;
using static ECSUtility;
using Type = Enums.Type;

public static class EnemyHolder
{
    public static ConcurrentDictionary<Type, Entity> EnemyPrefabDict = new ConcurrentDictionary<Type, Entity>();

    public static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();

    private static int currentNumberOfLoadedAssets = 0;
    private static int numberOfAssetsToLoad = 1;

    public static void Initialize()
    {
    }

    public static void OnDestroy()
    {
        blobAssetStores.ForEach(i => { i.Dispose(); });
    }

    public static void LoadAssets()
    {
        Addressables.LoadAssetAsync<EnemyPrefabContainer>("EnemyPrefabContainer").Completed += handle =>
        {
            ExtractPrefabs(handle.Result);
            currentNumberOfLoadedAssets++;
        };
    }

    private static void ExtractPrefabs(EnemyPrefabContainer container)
    {
        foreach (EnemyPrefabContainer.EnemyPrefab enemy in container.Prefabs)
        {
            //Convert GameObject to Entity
            Entity prefabEntity = ConvertGameObjectPrefab(enemy.Prefab, out BlobAssetStore blob);

            //Add to dictionary
            if (!EnemyPrefabDict.TryAdd(enemy.Type, prefabEntity))
                Debug.Log($"Couldnt add enemy of type {enemy.Type} to the prefab dictionary...");

            //Add blob asset if needed
            if (blob != null)
                blobAssetStores.Add(blob);
        }
    }

    public static float CurrentLoadingPercentage()
    {
        return (float) currentNumberOfLoadedAssets / numberOfAssetsToLoad;
    }
}