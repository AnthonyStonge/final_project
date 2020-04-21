using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class PlayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.SetName(entity, "Player");

        dstManager.AddComponent<PlayerTag>(entity);
        dstManager.AddComponent<TargetData>(entity);
        dstManager.AddComponentData(entity, new InputComponent
        {
            Enabled = true
        });

        //TODO REMOVE SPEED AND REPLACE BY AFFECTING VELOCITY
        dstManager.AddComponentData(entity, new SpeedData
        {
            Value = 1000
        });
        
      
    }
}
