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
        /*var bulletEvents = EventsHolder.BulletsEvents;
        if(bulletEvents.Length != 0 && bulletEvents.IsCreated)
            for (var index = 0; index < bulletEvents.Length; index++)
            {
                var info = bulletEvents[index];
                if (info.CollisionType == BulletInfo.BulletCollisionType.ON_ENEMY)
                {
                    //Debug.Log(AmunationDropHolder.DropItemPrefabDict.Count);
                    Entity e = EntityManager.Instantiate(
                        AmmunitionDropHolder.DropItemPrefabDict[DropType.Ammunition]);
                    EntityManager.SetComponentData(e, new Translation
                    {
                        Value = info.HitPosition
                    });
                    EntityManager.SetComponentData(e, new AmmunitionComponent
                    {
                        TypeAmmunition = (WeaponType) Random.Range(0, 2),
                        AmmunitionQuantity = Random.Range(1, 5)
                    });
                }
            }*/
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
