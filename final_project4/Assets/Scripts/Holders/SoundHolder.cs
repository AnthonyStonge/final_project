using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using EventStruct;
using UnityEngine;

public static class SoundHolder
{
    public static Dictionary<GunType, Dictionary<WeaponInfo.WeaponEventType, AudioClip>> WeaponSounds =
        new Dictionary<GunType, Dictionary<WeaponInfo.WeaponEventType, AudioClip>>();

    public static Dictionary<BulletType, Dictionary<BulletInfo.BulletCollisionType, AudioClip>> BulletSounds =
        new Dictionary<BulletType, Dictionary<BulletInfo.BulletCollisionType, AudioClip>>();

    public static void Initialize()
    {
        //Weapons
        int gunTypeLength = Enum.GetNames(typeof(GunType)).Length;
        for (int i = 0; i < gunTypeLength; i++)
        {
            WeaponSounds.Add((GunType) i, new Dictionary<WeaponInfo.WeaponEventType, AudioClip>());
        }

        //Bullets
        int bulletTypeLength = Enum.GetNames(typeof(BulletType)).Length;
        for (int i = 0; i < bulletTypeLength; i++)
        {
            BulletSounds.Add((BulletType) i, new Dictionary<BulletInfo.BulletCollisionType, AudioClip>());
        }
    }
}