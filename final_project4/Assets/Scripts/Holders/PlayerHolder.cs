using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static ECSUtility;

public static class PlayerHolder
{
    public static ConcurrentDictionary<PlayerType, Entity> PlayerDict;
    
    private static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;

    public static void Initialize()
    {
        PlayerDict = new ConcurrentDictionary<PlayerType, Entity>();
        
        currentNumberOfLoadedAssets = 0;
        numberOfAssetsToLoad = Enum.GetNames(typeof(PlayerType)).Length;
    }

    public static void LoadAssets()
    {
        foreach (var i in Enum.GetNames(typeof(PlayerType)))
        {
            Addressables.LoadAssetAsync<GameObject>(i).Completed +=
                obj =>
                {
                    PlayerDict.TryAdd((PlayerType) Enum.Parse(typeof(PlayerType), i), ConvertGameObjectPrefab(obj.Result, out BlobAssetStore blob));
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