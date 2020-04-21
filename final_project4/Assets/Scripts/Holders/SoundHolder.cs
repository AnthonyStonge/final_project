using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using EventStruct;
using UnityEngine;

public static class SoundHolder
{
    public static Dictionary<WeaponType, Dictionary<WeaponInfo.WeaponEventType, AudioClip>> WeaponSounds =
        new Dictionary<WeaponType, Dictionary<WeaponInfo.WeaponEventType, AudioClip>>();

    public static Dictionary<ProjectileType, Dictionary<BulletInfo.BulletCollisionType, AudioClip>> BulletSounds =
        new Dictionary<ProjectileType, Dictionary<BulletInfo.BulletCollisionType, AudioClip>>();

    public static void Initialize()
    {
        //Weapons
        int gunTypeLength = Enum.GetNames(typeof(WeaponType)).Length;
        for (int i = 0; i < gunTypeLength; i++)
        {
            WeaponSounds.Add((WeaponType) i, new Dictionary<WeaponInfo.WeaponEventType, AudioClip>());
        }

        //Bullets
        int bulletTypeLength = Enum.GetNames(typeof(ProjectileType)).Length;
        for (int i = 0; i < bulletTypeLength; i++)
        {
            BulletSounds.Add((ProjectileType) i, new Dictionary<BulletInfo.BulletCollisionType, AudioClip>());
        }
    }
}