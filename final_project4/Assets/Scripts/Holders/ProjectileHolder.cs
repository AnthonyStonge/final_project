using System.Collections.Concurrent;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;



//TODO DO WE REALLY NEED THIS BECAUSE WHEN CONVERTING PREFABS WEAPON, IT CONVERT ITS BULLET WITH IT...
//TODO UNLESS WE MAKE BULLET THAT DONT HAVE WEAPON, WE SHOULD REMOVE THIS CLASS
namespace Holders
{
    public static class ProjectileHolder
    {
        public static GameObject pistolGameObject;
        public static Entity PistolPrefab;
        
        public static void LoadAssets()
        {

            Addressables.LoadAssetAsync<GameObject>("Pistol").Completed += 
                obj =>
                {
                    if (obj.Status == AsyncOperationStatus.Succeeded)
                    {
                        //using(var blob = new BlobAssetStore())
                        {
                            PistolPrefab = GameObjectConversionUtility.
                                ConvertGameObjectHierarchy(obj.Result, 
                                    GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore()));
                        }
                    }

                };
        }

        public static void Test()
        {
            using(var blob = new BlobAssetStore())
            {
                PistolPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(pistolGameObject,GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob));
            }
        }
        
        public static ConcurrentDictionary<BulletType, Entity> BulletPrefabs = new ConcurrentDictionary<BulletType, Entity>();
        
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
}
