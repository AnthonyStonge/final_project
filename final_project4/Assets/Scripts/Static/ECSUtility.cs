using JetBrains.Annotations;
using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

public class ECSUtility
{
    public static Entity ConvertGameObjectPrefab(GameObject go, [CanBeNull] out BlobAssetStore blob)
    {
        Entity returnEntity = Entity.Null;
        //var bodyAuthoring = go.GetComponent<PhysicsBodyAuthoring>();
        //var colliderAuthoring = go.GetComponent<PhysicsShapeAuthoring>();
        //if (bodyAuthoring != null || colliderAuthoring != null)
        //{
            blob = new BlobAssetStore();

            returnEntity =
                GameObjectConversionUtility.ConvertGameObjectHierarchy(go,
                    GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blob));
        //}
        /*else
        {
            returnEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(go,
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null));
        }*/

        return returnEntity;
    }
}