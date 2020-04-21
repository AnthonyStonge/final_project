using System.Collections;
using System.Collections.Generic;
using EventStruct;
using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(ProjectileHitDetectionSystem))]
public class RetrieveVisualEventSystem : SystemBase
{
    
    
    protected override void OnUpdate()
    {
        //Take event and play it at desired position
        //Weapons
        foreach (WeaponInfo info in EventsHolder.WeaponEvents)
        {
            VisualEffectManager.PlayVisualEffect(info);
        }
        
        //Bullets
        foreach (BulletInfo info in EventsHolder.BulletsEvents)
        {
            VisualEffectManager.PlayVisualEffect(info);
        }
    }
}
