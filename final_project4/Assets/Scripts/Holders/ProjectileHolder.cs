using System;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static ECSUtility;

//TODO DO WE REALLY NEED THIS BECAUSE WHEN CONVERTING PREFABS WEAPON, IT CONVERT ITS BULLET WITH IT...
//TODO UNLESS WE MAKE BULLET THAT DONT HAVE WEAPON, WE SHOULD REMOVE THIS CLASS
public static class ProjectileHolder
{
    public static Dictionary<ProjectileType, Entity> projectileDict;
    
    private static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;

    public static void Initialize()
    {
        // ConvertPrefabs();
        projectileDict = new Dictionary<ProjectileType, Entity>();
       
        currentNumberOfLoadedAssets = 0;
        numberOfAssetsToLoad = Enum.GetNames(typeof(ProjectileType)).Length;
    }
    
    public static void LoadAssets()
    {
        foreach (var i in Enum.GetNames(typeof(ProjectileType)))
        {
            Addressables.LoadAssetAsync<GameObject>(i).Completed += obj =>
            {
                projectileDict.Add((ProjectileType) Enum.Parse(typeof(ProjectileType), i),
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
        blobAssetStores.ForEach(i=> { i.Dispose(); });
    }

    private static void ConvertPrefabs()
    {
       /* GameObject go;
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

        BulletPrefabs.TryAdd(BulletType.SHOTGUN_BULLET, shotgunBulletPrefab);*/
    }
}