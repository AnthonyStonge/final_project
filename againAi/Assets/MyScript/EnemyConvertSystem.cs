using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.AddressableAssets;
[DisallowMultipleComponent]
[RequiresEntityConversion]
public class EnemyConvertSystem : MonoBehaviour, IConvertGameObjectToEntity
{
    public static Entity ennemyEntity;

    public GameObject EnemyGO;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        /*using (BlobAssetStore blobAssetStore = new BlobAssetStore())
        {
            Entity e = GameObjectConversionUtility.ConvertGameObjectHierarchy(EnemyGO,
                GameObjectConversionSettings.FromWorld(dstManager.World, blobAssetStore));
            EnemyConvertSystem.ennemyEntity = entity;
        }*/
        
    }
}
