using System;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static ECSUtility;

public static class WeaponHolder
{
    public static Dictionary<WeaponType, Entity> WeaponDict;

    private static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();
    private static int currentNumberOfLoadedAssets;
    private static int numberOfAssetsToLoad;

    public static void Initialize()
    {
        WeaponDict = new Dictionary<WeaponType, Entity>();

        currentNumberOfLoadedAssets = 0;
        numberOfAssetsToLoad = Enum.GetNames(typeof(WeaponType)).Length;
    }

    public static void LoadAssets()
    {
        foreach (var i in Enum.GetNames(typeof(WeaponType)))
        {
            Addressables.LoadAssetAsync<GameObject>(i).Completed += obj =>
            {
                WeaponDict.Add((WeaponType) Enum.Parse(typeof(WeaponType), i),
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
        /* GameObject go;
         BlobAssetStore blob;
 
         //Pistol
         //TODO LOAD GAMEOBJECT FROM ADDRESSABLE
 
         go = MonoGameVariables.instance.Pistol;
 
         blob = new BlobAssetStore();
         blobAssetStores.Add(blob);
 
         Entity pistolPrefab =
             GameObjectConversionUtility.ConvertGameObjectHierarchy(go,
                 GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob));
 
         WeaponPrefabs.TryAdd(GunType.PISTOL, pistolPrefab);
 
         //Shotgun
         //TODO LOAD GAMEOBJECT FROM ADDRESSABLE
 
         go = MonoGameVariables.instance.Shotgun;
 
         blob = new BlobAssetStore();
         blobAssetStores.Add(blob);
 
         Entity shotgunPrefab =
             GameObjectConversionUtility.ConvertGameObjectHierarchy(go,
                 GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob));
 
         WeaponPrefabs.TryAdd(GunType.SHOTGUN, shotgunPrefab);*/
    }
}