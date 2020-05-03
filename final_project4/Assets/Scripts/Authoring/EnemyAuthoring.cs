using Unity.Entities;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class EnemyAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        #if UNITY_EDITOR
        dstManager.SetName(entity, "Enemy");
        #endif
        
        dstManager.AddComponent<EnemyTag>(entity);
        dstManager.AddComponent<TargetData>(entity);
        dstManager.AddComponent<AnimationData>(entity);
        dstManager.AddComponent<DirectionData>(entity);

        dstManager.AddComponent<PathPosition>(entity);
    }
}
