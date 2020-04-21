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
    private Dictionary<GunType, Dictionary<WeaponInfo.WeaponEventType, int>> weaponSounds =
        new Dictionary<GunType, Dictionary<WeaponInfo.WeaponEventType, int>>();

    private Dictionary<BulletType, Dictionary<BulletInfo.BulletCollisionType, int>> bulletSounds =
        new Dictionary<BulletType, Dictionary<BulletInfo.BulletCollisionType, int>>();


    protected override void OnCreate()
    {
        Debug.Log("Create RetrieveSoundEvent");
        //Weapons
        int gunTypeLength = Enum.GetNames(typeof(GunType)).Length;
        int gunEventTypeLength = Enum.GetNames(typeof(WeaponInfo.WeaponEventType)).Length;
        for (int i = 0; i < gunTypeLength; i++)
        {
            weaponSounds.Add((GunType) i, new Dictionary<WeaponInfo.WeaponEventType, int>());

            for (int j = 0; j < gunEventTypeLength; j++)
            {
                weaponSounds[(GunType) i].Add((WeaponInfo.WeaponEventType) j, 0);
            }
        }

        //Bullets
        int bulletTypeLength = Enum.GetNames(typeof(BulletType)).Length;
        int bulletCollisionEventType = Enum.GetNames(typeof(BulletInfo.BulletCollisionType)).Length;
        for (int i = 0; i < bulletTypeLength; i++)
        {
            bulletSounds.Add((BulletType) i, new Dictionary<BulletInfo.BulletCollisionType, int>());

            for (int j = 0; j < bulletCollisionEventType; j++)
            {
                bulletSounds[(BulletType) i].Add((BulletInfo.BulletCollisionType) j, 0);
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
            weaponSounds[info.GunType][info.EventType]++;
        }

        //Bullets
        foreach (BulletInfo info in EventsHolder.BulletsEvents)
        {
            bulletSounds[info.BulletType][info.CollisionType]++;
        }

        //Resolve how many sound of each should be played
        //TODO

        //Play sounds
        foreach (KeyValuePair<GunType, Dictionary<WeaponInfo.WeaponEventType, int>> gunType in weaponSounds)
        {
            foreach (KeyValuePair<WeaponInfo.WeaponEventType, int> eventType in gunType.Value)
            {
                if (eventType.Value > 0)
                    SoundManager.PlaySound(SoundHolder.WeaponSounds[gunType.Key][eventType.Key]);
            }
        }

        foreach (KeyValuePair<BulletType,Dictionary<BulletInfo.BulletCollisionType,int>> bulletType in bulletSounds)
        {
            foreach (KeyValuePair<BulletInfo.BulletCollisionType,int> eventType in bulletType.Value)
            {
                if (eventType.Value > 0)
                    SoundManager.PlaySound(SoundHolder.BulletSounds[bulletType.Key][eventType.Key]);
            }
        }
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