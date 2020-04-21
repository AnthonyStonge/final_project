using JetBrains.Annotations;
using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

public class ECSUtility
{
    public static Entity ConvertGameObjectPrefab(GameObject go, [CanBeNull] out BlobAssetStore blob)
    {
        Entity returnEntity = Entity.Null;
        
            blob = new BlobAssetStore();

            returnEntity =
                GameObjectConversionUtility.ConvertGameObjectHierarchy(go,
                    GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob));

            return returnEntity;
    }
}