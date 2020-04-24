using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Enums;
using EventStruct;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Analytics;
using Debug = UnityEngine.Debug;


public static class SoundHolder
{
    private static string[] ScriptableSoundName = 
    {
        "ShotgunSounds",
        "PistolSounds"
    };
    
    public static AudioSource audioSource;

    public static Dictionary<WeaponType, Dictionary<WeaponInfo.WeaponEventType, AudioClip>> WeaponSounds;
    public static Dictionary<ProjectileType, Dictionary<BulletInfo.BulletCollisionType, AudioClip>> BulletSounds;

    public static void Initialize()
    {
        WeaponSounds = new Dictionary<WeaponType, Dictionary<WeaponInfo.WeaponEventType, AudioClip>>();
        BulletSounds = new Dictionary<ProjectileType, Dictionary<BulletInfo.BulletCollisionType, AudioClip>>();

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

        foreach (var name in ScriptableSoundName)
        {
            //TODO Addressable, load all sounds
            Addressables.LoadAssetAsync<WeaponSoundScriptable>(name).Completed += obj =>
            {
                var newObj = obj.Result;
                WeaponType newWeaponType = newObj.WeaponType;

                if (WeaponSounds.TryGetValue(newWeaponType,
                    out Dictionary<WeaponInfo.WeaponEventType, AudioClip> value))
                {
                    for (int i = 0; i < newObj.SFXList.Count; i++)
                    {
                        WeaponInfo.WeaponEventType newEventType = newObj.SFXList[i].eventType;
                        value.TryGetValue(newEventType, out AudioClip audioClip);

                        if (audioClip != null)
                        {
                            Debug.Log("Sound already loaded for " + newEventType + ", Overriding.");
                        }

                        audioClip = newObj.SFXList[i].sound;
                    }
                }
                else
                {
                    Debug.Log(newWeaponType + " : WeaponType does not exist.");
                    return;
                }
            };
        }
    }
}