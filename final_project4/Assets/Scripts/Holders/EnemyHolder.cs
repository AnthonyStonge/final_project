using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Enums;
using UnityEngine.AddressableAssets;
using static ECSUtility;

public static class EnemyHolder
{
    public static ConcurrentDictionary<EnemyType, Entity> EnemyPrefabsEntities =
        new ConcurrentDictionary<EnemyType, Entity>();

    public static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();

    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;
    
    public static Dictionary<string, Entity> EnemyDict;
    
    public static string[] FileNameToLoad =
    {

    };
    
    public static void Initialize()
    {
        //ConvertPrefabs();
        EnemyDict = new Dictionary<string, Entity>();
        
        currentNumberOfLoadedAssets = 0;
        numberOfAssetsToLoad = FileNameToLoad.Length;
    }

    public static void LoadAssets()
    {
        foreach (var i in FileNameToLoad)
        {
            Addressables.LoadAssetAsync<GameObject>(i).Completed += obj =>
            {
                EnemyDict.Add(i, ConvertGameObjectPrefab(obj.Result));
                currentNumberOfLoadedAssets++;
            };
        }
    }

    public static void OnDestroy()
    {
        //Not sure if foreach would have work, didnt try it
        for (int i = blobAssetStores.Count - 1; i >= 0; i--)
        {
            blobAssetStores[i].Dispose();
        }
    }

    private static void ConvertPrefabs()
    {
        //TODO LOAD GAMEOBJECT FROM ADDRESSABLE
        GameObject go = MonoGameVariables.instance.Enemy1;

        BlobAssetStore blob = new BlobAssetStore();
        blobAssetStores.Add(blob);

        Entity enemy1 =
            GameObjectConversionUtility.ConvertGameObjectHierarchy(go,
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob));

        EnemyPrefabsEntities.TryAdd(EnemyType.GABICHOU, enemy1);
    }
}