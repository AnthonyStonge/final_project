using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class SoundEventSystem : SystemBase
{
    private static int currentSoundPlaying;
    protected override void OnStartRunning()
    {
        PlayGenericSoundtrack();
    }

    protected override void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            PlayGenericSoundtrack();
        }

        //Weapons
        foreach (WeaponInfo info in EventsHolder.WeaponEvents)
        {
            int soundId = SoundHolder.WeaponSounds[info.WeaponType][info.EventType];

            if (TryPlaySound(soundId))
                PlaySound(soundId);
        }

        //Bullets
        foreach (BulletInfo info in EventsHolder.BulletsEvents)
        {
            int soundId = SoundHolder.BulletSounds[info.ProjectileType][info.CollisionType];

            if (TryPlaySound(soundId))
                PlaySound(soundId);
        }

        //Decrement all sounds not available
        float deltaTime = Time.DeltaTime;
        SoundManager.DecrementNotAvailableSounds(deltaTime);
    }

    private static void PlayGenericSoundtrack()
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