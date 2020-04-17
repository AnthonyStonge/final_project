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

    //
    public static Entity PlayerPrefabEntity;

    public static void LoadAssets()
    {
        //Set number of assets

        if (playerAssetsAssets == null)
        {
            Addressables.LoadAssetAsync<PlayerAssetsScriptableObject>("PlayerScriptableObject").Completed +=
                obj =>
                {
                    playerAssetsAssets = obj.Result;
                    currentNumberOfLoadedAssets++;
                };
        }
    }

    public static void Initialize()
    {
        //Convert PlayerPrefabs
        ConvertPlayerPrefab();
    }

    private static void ConvertPlayerPrefab()
    {
        //TODO LOAD FROM ADDRESSABLE
        //Load GameObject
        GameObject playerGO = MonoGameVariables.instance.Player;

        //Convert
        using (BlobAssetStore blob = new BlobAssetStore())
        {
            PlayerPrefabEntity =
                GameObjectConversionUtility.ConvertGameObjectHierarchy(playerGO,
                    GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob));
        }
    }

    public static float CurrentLoadingPercentage()
    {
        return (float) currentNumberOfLoadedAssets / numberOfAssetsToLoad;
    }
}