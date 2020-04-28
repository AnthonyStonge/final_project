using Enums;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class PlayerAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
#if UNITY_EDITOR
        dstManager.SetName(entity, "Player");
#endif

        dstManager.AddComponentData(entity, new StateComponent
        {
            CurrentState = State.Idle
        });
        dstManager.AddComponent<PlayerTag>(entity);
        dstManager.AddComponent<TargetData>(entity);
        dstManager.AddComponent<AnimationData>(entity);
        dstManager.AddComponentData(entity, new InputComponent
        {
            Enabled = true
        });

        //TODO REMOVE SPEED AND REPLACE BY AFFECTING VELOCITY
        dstManager.AddComponentData(entity, new SpeedData
        {
            Value = 333
        });

    }
}
