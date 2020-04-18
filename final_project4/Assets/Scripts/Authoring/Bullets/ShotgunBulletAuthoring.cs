using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class ShotgunBulletAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.SetName(entity, "Bullet Shotgun");
        
        dstManager.AddComponent<BulletTag>(entity);
        dstManager.AddComponent<ShotgunBulletTag>(entity);
    }
}
