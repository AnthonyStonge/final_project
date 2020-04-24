using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(RetrieveGunEventSystem))]
public class SoundEventSystem : SystemBase
{
    protected override void OnUpdate()
    {
        List<WeaponType> weaponTypesShot = new List<WeaponType>();
        //Weapons
        foreach (WeaponInfo info in EventsHolder.WeaponEvents)
        {
            if (!weaponTypesShot.Contains(info.WeaponType))
            {
                SoundManager.PlaySound(SoundHolder.WeaponSounds[info.WeaponType][info.EventType]);
                weaponTypesShot.Add(info.WeaponType);
            }
        }

        //Bullets
        foreach (BulletInfo info in EventsHolder.BulletsEvents)
        {
        }
    }
}