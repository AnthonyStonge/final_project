using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class PistolAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.SetName(entity, "Weapon Pistol");
        
        dstManager.AddComponent<GunTag>(entity);
        dstManager.AddComponent<PistolTag>(entity);
        dstManager.AddComponent<LocalToParent>(entity);
        dstManager.AddComponent<Parent>(entity);
    }
}
