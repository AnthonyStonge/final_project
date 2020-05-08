using System.Diagnostics;
using Enums;
using EventStruct;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

[DisableAutoCreation]
public class DropSystem : SystemBase
{
    protected override void OnUpdate()
    {
     
    }

    public static void DropAmmunition(EntityManager em, float3 pos, WeaponType dropType)
    {
        Entity e = em.Instantiate(AmmunitionDropHolder.DropItemPrefabDict[(DropType)dropType]);
        em.SetComponentData(e, new Translation
        {
            Value = pos
        });
        em.SetComponentData(e, new AmmunitionComponent
        {
            TypeAmmunition = dropType,
            AmmunitionQuantity = 100
        });
    }
    
}
