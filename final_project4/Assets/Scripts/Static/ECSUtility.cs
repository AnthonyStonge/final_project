using JetBrains.Annotations;
using Unity.Entities;
using Unity.Physics.Authoring;
using Unity.Transforms;
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
    
    public static void MergeEntitiesTogether(EntityManager entityManager, Entity parent, Entity child)
    {
        if (!entityManager.HasComponent(child, typeof(Parent)) || entityManager.GetComponentData<Parent>(child).Value == Entity.Null)
        {
            entityManager.AddComponentData(child, new Parent { Value = parent });
            entityManager.AddComponentData(child, new LocalToParent() );
 
            DynamicBuffer<LinkedEntityGroup> buf = entityManager.GetBuffer<LinkedEntityGroup>(parent);
            buf.Add(child);
        }
    }
}