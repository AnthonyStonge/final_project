using Enums;
using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class InteractableAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public InteractableType Type;
    
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.AddComponentData(entity, new InteractableComponent
        {
            Type = Type
        });
    }
}
