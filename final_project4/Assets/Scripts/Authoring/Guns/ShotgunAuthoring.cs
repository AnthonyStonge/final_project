using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class ShotgunAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.SetName(entity, "Weapon Shotgun");
        
        dstManager.AddComponent<GunTag>(entity);
        dstManager.AddComponent<ShotgunTag>(entity);
        dstManager.AddComponent<LocalToParent>(entity);
        dstManager.AddComponent<Parent>(entity);
    }
}
