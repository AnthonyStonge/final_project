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
    
    protected override void OnUpdate()
    {
        if(EventsHolder.BulletsEvents.Length != 0 && EventsHolder.BulletsEvents.IsCreated)
            foreach (BulletInfo info in EventsHolder.BulletsEvents)
            {
                if (info.CollisionType == BulletInfo.BulletCollisionType.ON_ENEMY)
                {
                        //Debug.Log(AmunationDropHolder.DropItemPrefabDict.Count);
                        Entity e = EntityManager.Instantiate(
                            AmunationDropHolder.DropItemPrefabDict[DropType.Amunation]);
                        EntityManager.SetComponentData(e, new Translation
                        {
                            Value = info.HitPosition
                        });
                        EntityManager.SetComponentData(e, new AmunationComponent()
                        {
                            TypeAmunation = ((WeaponType) Random.Range(0, 2)),
                            AmunationQuantity = Random.Range(1, 5)
                        });
                }
            }
        
        //entityPass.Dispose();
    }
}
