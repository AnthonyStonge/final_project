using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class PlayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.SetName(entity, "Player");

        dstManager.AddComponentData(entity, new PlayerTag());
        dstManager.AddComponentData(entity, new TargetData());
        dstManager.AddComponentData(entity, new InputComponent());
    }
}
