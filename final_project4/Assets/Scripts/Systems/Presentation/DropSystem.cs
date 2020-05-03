using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;
[DisableAutoCreation]
public class DropSystem : SystemBase
{
    protected override void OnCreate()
    {
        base.OnCreate();
    }

    protected override void OnUpdate()
    {
        if(EventsHolder.BulletsEvents.Length != 0 && EventsHolder.BulletsEvents.IsCreated)
            foreach (BulletInfo info in EventsHolder.BulletsEvents)
            {
                if (info.CollisionType == BulletInfo.BulletCollisionType.ON_ENEMY)
                {
                        //Debug.Log(AmunationDropHolder.DropItemPrefabDict.Count);
                        Entity e = EntityManager.Instantiate(
                            AmmunitionDropHolder.DropItemPrefabDict[DropType.Amunation]);
                        EntityManager.SetComponentData(e, new Translation
                        {
                            Value = info.HitPosition
                        });
                        EntityManager.SetComponentData(e, new AmmunitionComponent
                        {
                            TypeAmunation = (WeaponType) Random.Range(0, 2),
                            AmmunitionQuantity = Random.Range(1, 5)
                        });
                }
            }
        
        //entityPass.Dispose();
    }
}
