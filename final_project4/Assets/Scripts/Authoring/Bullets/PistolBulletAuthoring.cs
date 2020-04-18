using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class PistolBulletAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.SetName(entity, "Bullet Pistol");
        
        dstManager.AddComponent<BulletTag>(entity);
        dstManager.AddComponent<PistolBulletTag>(entity);
    }
}
