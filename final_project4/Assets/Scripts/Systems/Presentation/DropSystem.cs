using System.Diagnostics;
using Enums;
using EventStruct;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Random = UnityEngine.Random;

[DisableAutoCreation]
public class DropSystem : SystemBase
{
    protected override void OnUpdate()
    {
     
    }

    public static void DropAmmunition(EntityManager em, float3 pos)
    {
        Entity e = em.Instantiate(
            AmmunitionDropHolder.DropItemPrefabDict[DropType.Ammunition]);
        em.SetComponentData(e, new Translation
        {
            Value = pos
        });
        em.SetComponentData(e, new AmmunitionComponent
        {
            TypeAmmunition = (WeaponType) Random.Range(0, 2),
            AmmunitionQuantity = Random.Range(1, 5)
        });
    }
    
}
