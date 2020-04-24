using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class ShotgunBulletAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
#if UNITY_EDITOR
        dstManager.SetName(entity, "Bullet Shotgun");
#endif

        dstManager.AddComponent<BulletTag>(entity);
        dstManager.AddComponent<ShotgunBulletTag>(entity);
    }
}