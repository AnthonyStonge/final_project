using System;
using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Assertions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;
public class Clip
{
    public AudioClip AudioClip;
    public float Timer;
    public float ResetTimerValue;
    public bool IsAvailable => Timer <= 0;
}

public class Source
{
    public AudioSource AudioSource;
    public bool IsPlayOneShotOnly;
}

public static class SoundHolder
{
    public static Dictionary<int, Clip> Sounds;
    public static Dictionary<int, AudioSourceType> SoundsToAudioSource;
    public static Dictionary<AudioSourceType, Source> AudioSources;

    public static Dictionary<WeaponType, Dictionary<WeaponInfo.WeaponEventType, int>> WeaponSounds;
    public static Dictionary<ProjectileType, Dictionary<BulletInfo.BulletCollisionType, int>> BulletSounds;

    private static int currentNumberOfLoadedAssets = 0;
    private static int numberOfAssetsToLoad = 1;
    
    public static void Initialize()
    {
        Sounds = new Dictionary<int, Clip>();
        SoundsToAudioSource = new Dictionary<int, AudioSourceType>();
        AudioSources = new Dictionary<AudioSourceType, Source>();

        //Weapons
        WeaponSounds = new Dictionary<WeaponType, Dictionary<WeaponInfo.WeaponEventType, int>>();
        for (int i = 0; i < Enum.GetNames(typeof(WeaponType)).Length; i++)
        {
            WeaponSounds.Add((WeaponType) i, new Dictionary<WeaponInfo.WeaponEventType, int>());
        }

        //Bullets
        BulletSounds = new Dictionary<ProjectileType, Dictionary<BulletInfo.BulletCollisionType, int>>();
        for (int i = 0; i < Enum.GetNames(typeof(ProjectileType)).Length; i++)
        {
            BulletSounds.Add((ProjectileType) i, new Dictionary<BulletInfo.BulletCollisionType, int>());
        }
    }

    public static void LoadAssets()
    {
        //Get ScriptableObject SoundsContainer
        Addressables.LoadAssetAsync<SoundsContainer>("SoundsContainer").Completed += handle =>
        {
            ExtractDataFromContainer(handle.Result);
            currentNumberOfLoadedAssets++;
        };
        Addressables.LoadAssetAsync<AudioSourcesContainer>("AudioSourceContainer").Completed += handle =>
        {
            InstantiateAudioSources(handle.Result);
        };
    }

    private static void ExtractDataFromContainer(SoundsContainer container)
    {
        int nextClipID = 0;

        foreach (SoundLinksScriptableObjects links in container.SoundLinkList)
        {
            //Add AudioClip to dictionary
            Sounds.Add(nextClipID, new Clip
            {
                AudioClip = links.Clip,
                ResetTimerValue = links.Delay
            });
            SoundsToAudioSource.Add(nextClipID, links.AudioSourceType);

            //Weapons
            foreach (SoundLinksScriptableObjects.WeaponLinks weapon in links.Weapons)
            {
                //Add to weapon dictionary
                if (!WeaponSounds[weapon.WeaponType].ContainsKey(weapon.EventType))
                    WeaponSounds[weapon.WeaponType].Add(weapon.EventType, nextClipID);
                else
                {
                    //Duplicates -> LogError
                    Debug.LogError("You tried to add multiple sound effects for " + weapon.WeaponType + " " +
                                   weapon.EventType + " action. \n" +
                                   "Current Sound: " + Sounds[WeaponSounds[weapon.WeaponType][weapon.EventType]].AudioClip.name +
                                   "\n" +
                                   "Desired Sound: " + links.Clip.name + "\n");
                }
            }

            //Bullets
            foreach (SoundLinksScriptableObjects.BulletLinks bullet in links.Bullets)
            {
                //TODO MAKE SURE THERES NO DUPLICATES
                //Add to bullet dictionary
                BulletSounds[bullet.BulletType].Add(bullet.CollisionType, nextClipID);
            }

            //Increment ID
            nextClipID++;
        }
    }

    private static void InstantiateAudioSources(AudioSourcesContainer container)
    {
        Assert.IsNotNull(MonoGameVariables.Instance.CameraTransform);

        foreach (AudioSourcesContainer.AudioSourceLink link in container.AudioSources)
        {
            //Instantiate (with camera as parent)
            AudioSource source = Object.Instantiate(link.Source, MonoGameVariables.Instance.CameraTransform);

            //Add to dictonary
            AudioSources.Add(link.Type, new Source
            {
                AudioSource = source,
                IsPlayOneShotOnly = link.IsPlayOneShotOnly
            });
        }
    }
    
    public static float CurrentLoadingPercentage()
    {
        return (float) currentNumberOfLoadedAssets / numberOfAssetsToLoad;
    }
}