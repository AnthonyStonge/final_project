using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class MapHolder
{
    public static ConcurrentDictionary<string, Entity> MapPrefabDict;

    public static List<BlobAssetStore> BloblAssetList = new List<BlobAssetStore>();
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;

    public static void Initialize()
    {
        currentNumberOfLoadedAssets = 0;
        numberOfAssetsToLoad = 1;
        
        MapPrefabDict = new ConcurrentDictionary<string, Entity>();
    }

    public static void LoadAssets()
    {
        
            //Addressables.LoadAssetAsync<
        
    }
    
    public static float CurrentLoadingPercentage()
    {
        return (float) currentNumberOfLoadedAssets / numberOfAssetsToLoad;
    }

    public static void OnDestroy()
    {
        BloblAssetList.ForEach(i=>{ i.Dispose(); });
    }

}