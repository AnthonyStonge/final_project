﻿using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Entities;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[DisableAutoCreation]
public class SoundEventSystem : SystemBase
{
    private static int currentSoundPlaying = 1;
    private static int frameCounter;

    private static Random seed;
    private static bool rndSet;
    private static int rndNumber;
    protected override void OnStartRunning()
    {
        seed = new Random(12354);
        PlayGenericSoundtrack();
        PlayAmbienceSounds();
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ForceChangeGenericSoundtrack();
        }

        PlayGenericSoundtrack();
        PlayRandomlyAmbienceSFX();

        //Weapons
        foreach (WeaponInfo info in EventsHolder.WeaponEvents)
        {
            if (!SoundHolder.WeaponSounds.ContainsKey(info.WeaponType) ||
                !SoundHolder.WeaponSounds[info.WeaponType].ContainsKey(info.EventType))
                continue;
            int soundId = SoundHolder.WeaponSounds[info.WeaponType][info.EventType];

            if (TryPlaySound(soundId))
                PlaySound(soundId);
        }

        //Bullets
        foreach (BulletInfo info in EventsHolder.BulletsEvents)
        {
            if (!SoundHolder.BulletSounds.ContainsKey(info.ProjectileType) ||
                !SoundHolder.BulletSounds[info.ProjectileType].ContainsKey(info.CollisionType))
                continue;
            int soundId = SoundHolder.BulletSounds[info.ProjectileType][info.CollisionType];

            if (TryPlaySound(soundId))
                PlaySound(soundId);
        }
        

        //Decrement all sounds not available
        float deltaTime = Time.DeltaTime;
        SoundManager.DecrementNotAvailableSounds(deltaTime);
    }
    
    public static void PlayDashSound()
    {
        List<int> sfx = SoundHolder.PlayerSounds[State.Dashing];
        PlaySound(sfx[0]);
    }

    public static void PlayerDieSound()
    {
        List<int> sfx = SoundHolder.PlayerSounds[State.Dying];
        PlaySound(sfx[0]);
    }

    public static void PlayerHitSound()
    {
        
    }

    private static void PlayAmbienceSounds()
    {
        List<int> backgroundSounds = SoundHolder.GenericSounds[SoundType.Backgrounds];
        PlaySound(backgroundSounds[0]);
    }

    private static void PlayRandomlyAmbienceSFX()
    {
        frameCounter++;
        if (frameCounter > 300)
        {
            if (rndSet == false)
            {
                rndSet = true;
                rndNumber = seed.NextInt(1000);
            }

            if (frameCounter % 1000 == rndNumber)
            {
                List<int> SFXSounds = SoundHolder.GenericSounds[SoundType.SFX];
                PlaySound(SFXSounds[seed.NextInt(0, SFXSounds.Count - 1)]);
                frameCounter = 0;
                rndSet = false;
            }
        }
    }

    public static void PlayPickupSound(DropType t)
    {
        List<int> DropSounds = SoundHolder.PickupSounds[DropType.AmmunitionShotgun];
        if(DropSounds.Count != 0)
            PlaySound(DropSounds[seed.NextInt(0, DropSounds.Count)]);
    }
    
    private static void PlayGenericSoundtrack()
    {
        if (!SoundHolder.AudioSources[AudioSourceType.Soundtrack].AudioSource.isPlaying)
        {
            List<int> genericSoundId = SoundHolder.GenericSounds[SoundType.Soundtracks];
            PlaySound(genericSoundId[currentSoundPlaying]);
            currentSoundPlaying++;
            if (currentSoundPlaying == genericSoundId.Count)
            {
                currentSoundPlaying = 0;
            }
        }
    }

    private static void ForceChangeGenericSoundtrack()
    {
        List<int> genericSoundId = SoundHolder.GenericSounds[SoundType.Soundtracks];
        PlaySound(genericSoundId[currentSoundPlaying]);
        currentSoundPlaying++;
        if (currentSoundPlaying == genericSoundId.Count)
        {
            currentSoundPlaying = 0;
        }
    }

    private static bool TryPlaySound(int soundId)
    {
        return SoundHolder.Sounds[soundId].IsAvailable;
    }

    private static void PlaySound(int soundId)
    {
        SoundManager.PlaySound(soundId);
        ResetSoundTimer(soundId);
    }

    private static void ResetSoundTimer(int soundId)
    {
        Clip clip = SoundHolder.Sounds[soundId];
        clip.Timer = clip.ResetTimerValue;
    }
}