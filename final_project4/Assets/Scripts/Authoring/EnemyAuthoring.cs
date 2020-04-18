using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisallowMultipleComponent]
[RequiresEntityConversion]
public class EnemyAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        dstManager.SetName(entity, "Enemy");

        dstManager.AddComponent<EnemyTag>(entity);
        dstManager.AddComponent<TargetData>(entity);
    }
}
