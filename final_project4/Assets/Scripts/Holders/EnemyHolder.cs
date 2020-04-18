using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Enums;
using Unity.Collections;

public static class EnemyHolder
{
    public static ConcurrentDictionary<EnemyType, Entity> EnemyPrefabsEntities =
        new ConcurrentDictionary<EnemyType, Entity>();

    public static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();

    public static void Initialize()
    {
        ConvertPrefabs();
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