using Enums;
using Static.Events;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using static GameVariables;
using EventStruct;

[DisableAutoCreation]
public class PistolSystem : SystemBase
{
    private NativeQueue<WeaponInfo> weaponFired;
    
    protected override void OnCreate()
    {
        this.weaponFired = new NativeQueue<WeaponInfo>(Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        this.weaponFired.Dispose();
    }

    protected override void OnUpdate()
    {
        //Clear previous PistolBullet events
        EventsHolder.WeaponEvents.Clear();    //TODO MOVE TO CLEANUPSYSTEM
        
        //Create parallel writer
        NativeQueue<WeaponInfo>.ParallelWriter weaponFiredEvents = this.weaponFired.AsParallelWriter();

        //
        float deltaTime = Time.DeltaTime;

        StateActions state = PlayerVars.CurrentState;
        int magazineSize = PistolVars.MagazineSize;
        float betweenShotTime = PistolVars.BetweenShotTime;
        float reloadTime = PistolVars.ReloadTime;

        JobHandle job = Entities.ForEach((int entityInQueryIndex, ref PistolComponent pistol, in LocalToWorld trans) =>
        {
            if (pistol.IsReloading)
            {
                pistol.ReloadTime -= deltaTime;
                if (pistol.ReloadTime <= 0)
                {
                    pistol.CurrentBulletInMagazine = magazineSize;
                }
            }
            else if (state == StateActions.ATTACKING && !pistol.IsBetweenShot)
            {
                pistol.CurrentBulletInMagazine--;
                if (pistol.CurrentBulletInMagazine == 0)
                {
                    pistol.ReloadTime = reloadTime;
                }
                
                
                //Add event to queue
                weaponFiredEvents.Enqueue(new WeaponInfo
                {
                    GunType = GunType.PISTOL,
                    EventType = WeaponInfo.WeaponEventType.ON_SHOOT,
                    
                    Position = trans.Position + trans.Forward * -pistol.BetweenShotTime,
                    Rotation = trans.Rotation
                });
                
                pistol.BetweenShotTime = betweenShotTime;

            }
            else if (pistol.IsBetweenShot)
            {
                pistol.BetweenShotTime -= deltaTime;
            }
        }).ScheduleParallel(Dependency);
        
        //Terminate job before reading from array
        job.Complete();
/*
        //Move BulletInfo to EventsHolder class
        while (this.bulletToShoot.TryDequeue(out BulletInfo info))
        {
            EventsHolder.PistolBulletToShoot.Add(info);
            EventsHolder.SoundsToPlay.Add(new SoundInfo
            {
                Type = SoundType.ON_PISTOL_SHOT
            });
        }*/
    }
}