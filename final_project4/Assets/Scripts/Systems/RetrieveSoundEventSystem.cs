using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(RetrieveGunEventSystem))]
public class RetrieveSoundEventSystem : SystemBase
{
    private Dictionary<WeaponType, Dictionary<WeaponInfo.WeaponEventType, int>> weaponSounds =
        new Dictionary<WeaponType, Dictionary<WeaponInfo.WeaponEventType, int>>();

    private Dictionary<ProjectileType, Dictionary<BulletInfo.BulletCollisionType, int>> bulletSounds =
        new Dictionary<ProjectileType, Dictionary<BulletInfo.BulletCollisionType, int>>();


    protected override void OnCreate()
    {
        //Weapons
        int gunTypeLength = Enum.GetNames(typeof(WeaponType)).Length;
        int gunEventTypeLength = Enum.GetNames(typeof(WeaponInfo.WeaponEventType)).Length;
        for (int i = 0; i < gunTypeLength; i++)
        {
            weaponSounds.Add((WeaponType) i, new Dictionary<WeaponInfo.WeaponEventType, int>());

            for (int j = 0; j < gunEventTypeLength; j++)
            {
                weaponSounds[(WeaponType) i].Add((WeaponInfo.WeaponEventType) j, 0);
            }
        }

        //Bullets
        int bulletTypeLength = Enum.GetNames(typeof(ProjectileType)).Length;
        int bulletCollisionEventType = Enum.GetNames(typeof(BulletInfo.BulletCollisionType)).Length;
        for (int i = 0; i < bulletTypeLength; i++)
        {
            bulletSounds.Add((ProjectileType) i, new Dictionary<BulletInfo.BulletCollisionType, int>());

            for (int j = 0; j < bulletCollisionEventType; j++)
            {
                bulletSounds[(ProjectileType) i].Add((BulletInfo.BulletCollisionType) j, 0);
            }
        }
    }

    protected override void OnUpdate()
    {
        //Clear previous sounds events
        ResetValues(weaponSounds);
        ResetValues(bulletSounds);

        //Count how many sounds of each type needs to be played
        //Weapons
        foreach (WeaponInfo info in EventsHolder.WeaponEvents)
        {
            weaponSounds[info.WeaponType][info.EventType]++;
        }

        //Bullets
        foreach (BulletInfo info in EventsHolder.BulletsEvents)
        {
            bulletSounds[info.ProjectileType][info.CollisionType]++;
        }

        //Resolve how many sound of each should be played
        //TODO

        //Play sounds
        
    }

    private static void ResetValues<T1, T2>(Dictionary<T1, Dictionary<T2, int>> dictionary) where T1 : System.Enum
    {
        //Reset values to 0 (as if no event ever happend)
        for (int i = 0; i < dictionary.Count; i++)
        {
            for (int j = 0; j < dictionary[(T1) (i as object)].Count; j++)
            {
                dictionary[(T1) (i as object)][(T2) (j as object)] = 0;
            }
        }
    }
}