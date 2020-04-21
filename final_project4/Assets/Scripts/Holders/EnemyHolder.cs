using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Enums;
using UnityEngine.AddressableAssets;
using static ECSUtility;

public static class EnemyHolder
{
    public static Dictionary<EnemyType, Entity> EnemyDict;
    
    public static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;
    
    public static void Initialize()
    {
        EnemyDict = new Dictionary<EnemyType, Entity>();
        
        currentNumberOfLoadedAssets = 0;
        numberOfAssetsToLoad = Enum.GetNames(typeof(EnemyType)).Length;
    }

    public static void LoadAssets()
    {
        foreach (var i in Enum.GetNames(typeof(EnemyType)))
        {
            Addressables.LoadAssetAsync<GameObject>(i).Completed += obj =>
            {
                EnemyDict.Add((EnemyType) Enum.Parse(typeof(EnemyType), i), 
                    ConvertGameObjectPrefab(obj.Result, out BlobAssetStore blob));
                currentNumberOfLoadedAssets++;
                if (blob != null)
                {
                    blobAssetStores.Add(blob);
                }
            };
        }
    }

    public static void OnDestroy()
    {
        blobAssetStores.ForEach(i=>{ i.Dispose(); });
    }

    private static void ConvertPrefabs()
    {
        /*//TODO LOAD GAMEOBJECT FROM ADDRESSABLE
        GameObject go = MonoGameVariables.instance.Enemy1;

        BlobAssetStore blob = new BlobAssetStore();
        blobAssetStores.Add(blob);

        Entity enemy1 =
            GameObjectConversionUtility.ConvertGameObjectHierarchy(go,
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob));

        EnemyPrefabsEntities.TryAdd(EnemyType.GABICHOU, enemy1);*/
    }
}