using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class MachinegunAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
#if UNITY_EDITOR
        dstManager.SetName(entity, "Weapon Shotgun");
#endif
        
        dstManager.AddComponent<GunTag>(entity);
        dstManager.AddComponent<MachineGunTag>(entity);
        dstManager.AddComponent<LocalToParent>(entity);
        dstManager.AddComponent<Parent>(entity);
    }
}
