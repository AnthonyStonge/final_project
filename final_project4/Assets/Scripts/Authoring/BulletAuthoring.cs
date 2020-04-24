using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class BulletAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        #if UNITY_EDITOR
        dstManager.SetName(entity, "Pistol Bullet");
        #endif
    }
}
