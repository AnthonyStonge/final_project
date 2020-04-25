using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Entities;

[UpdateAfter(typeof(RetrieveGunEventSystem))]
public class SoundEventSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //Weapons
        List<WeaponType> weaponTypesShot = new List<WeaponType>();
        foreach (WeaponInfo info in EventsHolder.WeaponEvents)
        {
            int soundId = SoundHolder.WeaponSounds[info.WeaponType][info.EventType];

            //Is sound ready to be played
            if (!SoundHolder.Sounds[soundId].IsAvailable)
                continue;

            //Play
            SoundManager.PlaySound(SoundHolder.WeaponSounds[info.WeaponType][info.EventType]);

            ResetSoundTimer(SoundHolder.Sounds[soundId]);
        }

        //Bullets
        List<ProjectileType> bulletTypesHit = new List<ProjectileType>();
        foreach (BulletInfo info in EventsHolder.BulletsEvents)
        {
            if (!bulletTypesHit.Contains(info.ProjectileType))
                continue;

            SoundManager.PlaySound(SoundHolder.BulletSounds[info.ProjectileType][info.CollisionType]);
            bulletTypesHit.Add(info.ProjectileType);
        }

        //Decrement all sounds not available
        float deltaTime = Time.DeltaTime;
        SoundManager.DecrementNotAvailableSounds(deltaTime);
    }

    private static void ResetSoundTimer(Clip clip)
    {
        clip.Timer = clip.ResetTimerValue;
    }
}