using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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
    }
}
