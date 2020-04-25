using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Entities;

[DisableAutoCreation]
public class SoundEventSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //Weapons
        List<WeaponType> weaponTypesShot = new List<WeaponType>();
        foreach (WeaponInfo info in EventsHolder.WeaponEvents)
        {
            int soundId = SoundHolder.WeaponSounds[info.WeaponType][info.EventType];

            if(TryPlaySound(soundId))
                PlaySound(soundId); 
        }

        //Bullets
        foreach (BulletInfo info in EventsHolder.BulletsEvents)
        {
            int soundId = SoundHolder.BulletSounds[info.ProjectileType][info.CollisionType];
            
            if(TryPlaySound(soundId))
                PlaySound(soundId); 
        }

        //Decrement all sounds not available
        float deltaTime = Time.DeltaTime;
        SoundManager.DecrementNotAvailableSounds(deltaTime);
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