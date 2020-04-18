using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class PlayerHolder
{
    private static PlayerAssetsScriptableObject playerAssetsAssets;
    public static PlayerAssetsScriptableObject PlayerAssetsAssets => playerAssetsAssets;
    
    private static bool loaded = false;
    public static bool Loaded => loaded;

    private static int currentNumberOfLoadedAssets = 0;
    private static int numberOfAssetsToLoad = 1;

    public static Entity PlayerPrefabEntity;

    public static Dictionary<string, Entity> PlayerDict;

    //BlobAssetsReferences    //TODO KEEP IN ANOTHER HOLDER?
    private static BlobAssetStore playerBlobAsset;

    public static string[] FileNameToLoad =
    {
        
    };

    public static void LoadAssets()
    {
        foreach(var i in FileNameToLoad){
            Addressables.LoadAssetAsync<PlayerAssetsScriptableObject>(i).Completed +=
                obj =>
                {
                  //  PlayerDict.Add(i, obj.Result);
                  //  currentNumberOfLoadedAssets++;
                };
        }
    }

    public static void Initialize()
    {
        //Convert PlayerPrefabs
        ConvertPlayerPrefab();
    }

    public static void OnDestroy()
    {
        playerBlobAsset.Dispose();
    }

    private static void ConvertPlayerPrefab()
    {
        //TODO LOAD FROM ADDRESSABLE
        //Load GameObject
        GameObject playerGO = MonoGameVariables.instance.Player;

        //Convert
        playerBlobAsset = new BlobAssetStore();

        PlayerPrefabEntity =
            GameObjectConversionUtility.ConvertGameObjectHierarchy(playerGO,
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, playerBlobAsset));
    }

    public static float CurrentLoadingPercentage()
    {
        return (float) currentNumberOfLoadedAssets / numberOfAssetsToLoad;
    }
}