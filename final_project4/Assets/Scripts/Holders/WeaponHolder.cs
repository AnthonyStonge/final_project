using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;

namespace Holder
{
    public static class WeaponHolder
    {
        public static ConcurrentDictionary<GunType, Entity> WeaponPrefabs = new ConcurrentDictionary<GunType, Entity>();
        
        private static List<BlobAssetStore> blobAssetStores = new List<BlobAssetStore>();
        
        
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
            GameObject go;
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

            WeaponPrefabs.TryAdd(GunType.SHOTGUN, shotgunPrefab);
        }
    }
}