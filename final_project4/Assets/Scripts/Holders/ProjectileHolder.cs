using System.Collections.Concurrent;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static ECSUtility;


//TODO DO WE REALLY NEED THIS BECAUSE WHEN CONVERTING PREFABS WEAPON, IT CONVERT ITS BULLET WITH IT...
//TODO UNLESS WE MAKE BULLET THAT DONT HAVE WEAPON, WE SHOULD REMOVE THIS CLASS

public static class ProjectileHolder
{
    public static GameObject pistolGameObject;
    public static Entity PistolPrefab;

    public static ConcurrentDictionary<BulletType, Entity> BulletPrefabs =
        new ConcurrentDictionary<BulletType, Entity>();

    public static Dictionary<string, Entity> BulletDict;
    
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;

    private static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();
    
    public static string[] FileNameToLoad =
    {

    };
    
    public static void LoadAssets()
    {
        foreach (var i in FileNameToLoad)
        {
            Addressables.LoadAssetAsync<GameObject>(i).Completed += obj =>
            {
                BulletDict.Add(i, ConvertGameObjectPrefab(obj.Result));
                currentNumberOfLoadedAssets++;
            };
        }
        
        Addressables.LoadAssetAsync<GameObject>("Pistol").Completed +=
            obj =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    PistolPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(obj.Result,
                        GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld,
                            new BlobAssetStore()));
                }
            };
    }

    public static void Initialize()
    {
       // ConvertPrefabs();
       BulletDict = new Dictionary<string, Entity>();
       
       currentNumberOfLoadedAssets = 0;
       numberOfAssetsToLoad = FileNameToLoad.Length;
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
        GameObject go;
        BlobAssetStore blob;

        //PistolBullet
        //TODO LOAD GAMEOBJECT FROM ADDRESSABLE

        go = MonoGameVariables.instance.PistolBullet;

        blob = new BlobAssetStore();
        blobAssetStores.Add(blob);

        Entity pistolBulletPrefab =
            GameObjectConversionUtility.ConvertGameObjectHierarchy(go,
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob));

        BulletPrefabs.TryAdd(BulletType.PISTOL_BULLET, pistolBulletPrefab);

        //ShotgunBullet
        //TODO LOAD GAMEOBJECT FROM ADDRESSABLE

        go = MonoGameVariables.instance.ShotgunBullet;

        blob = new BlobAssetStore();
        blobAssetStores.Add(blob);

        Entity shotgunBulletPrefab =
            GameObjectConversionUtility.ConvertGameObjectHierarchy(go,
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob));

        BulletPrefabs.TryAdd(BulletType.SHOTGUN_BULLET, shotgunBulletPrefab);
    }
}