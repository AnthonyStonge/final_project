using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class WeaponAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
#if UNITY_EDITOR
        dstManager.SetName(entity, name);
#endif

        dstManager.AddComponent<GunTag>(entity);
        //Shouldn't add Component with values without setting them. 
        // dstManager.AddComponent<LocalToParent>(entity);
        // dstManager.AddComponent<Parent>(entity);
    }
}