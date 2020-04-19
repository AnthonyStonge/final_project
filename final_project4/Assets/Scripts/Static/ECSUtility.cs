using Unity.Entities;
using Unity.Physics.Authoring;
using UnityEngine;

public class ECSUtility
{
    public static Entity ConvertPlayerPrefab(GameObject go)
    {
        Entity returnEntity = Entity.Null;
        var bodyAuthoring = go.GetComponent<PhysicsBodyAuthoring>();
        var colliderAuthoring = go.GetComponent<PhysicsShapeAuthoring>();

        if (bodyAuthoring != null || colliderAuthoring != null)
        {
            var newBlobAsset = new BlobAssetStore();

            returnEntity =
                GameObjectConversionUtility.ConvertGameObjectHierarchy(go,
                    GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, newBlobAsset));
        }
        else
        {
            returnEntity = GameObjectConversionUtility.ConvertGameObjectHierarchy(go,
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null));
        }

        return returnEntity;
    }
}