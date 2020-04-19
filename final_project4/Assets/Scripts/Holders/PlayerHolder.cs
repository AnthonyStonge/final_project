using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static ECSUtility;

public static class PlayerHolder
{
    private static PlayerAssetsScriptableObject playerAssetsAssets;
    public static PlayerAssetsScriptableObject PlayerAssetsAssets => playerAssetsAssets;
    
    private static bool loaded = false;
    public static bool Loaded => loaded;

    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;

    public static Entity PlayerPrefabEntity;

    public static Dictionary<string, Entity> PlayerDict;

    //BlobAssetsReferences    //TODO KEEP IN ANOTHER HOLDER?
    private static BlobAssetStore playerBlobAsset;

    public static string[] FileNameToLoad =
    {
        
    };
    
    public static void Initialize()
    {
        //Convert PlayerPrefabs
        // ConvertPlayerPrefab();
        currentNumberOfLoadedAssets = 0;
        numberOfAssetsToLoad = FileNameToLoad.Length;

    }

    public static void LoadAssets()
    {
        foreach(var i in FileNameToLoad){
            Addressables.LoadAssetAsync<GameObject>(i).Completed +=
                obj =>
                {
                    PlayerDict.Add(i, ConvertPlayerPrefab(obj.Result));
                    currentNumberOfLoadedAssets++;
                };
        }
    }
    
    public static float CurrentLoadingPercentage()
    {
        return (float) currentNumberOfLoadedAssets / numberOfAssetsToLoad;
    }

    public static void OnDestroy()
    {
        playerBlobAsset.Dispose();
    }
}