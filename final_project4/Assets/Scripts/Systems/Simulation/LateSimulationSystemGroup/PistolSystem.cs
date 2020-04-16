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
    private NativeQueue<BulletInfo> bulletToShoot;
    
    protected override void OnCreate()
    {
        this.bulletToShoot = new NativeQueue<BulletInfo>(Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        this.bulletToShoot.Dispose();
        EventsHolder.PistolBulletToShoot.Dispose();    //TODO DO SOMEWHERE ELSE
    }

    protected override void OnUpdate()
    {
        //Clear previous PistolBullet events
        EventsHolder.PistolBulletToShoot.Clear();
        
        //Create parallel writer
        NativeQueue<BulletInfo>.ParallelWriter events = this.bulletToShoot.AsParallelWriter();
        
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
                events.Enqueue(new BulletInfo
                {
                    Position = trans.Position + trans.Forward * -pistol.BetweenShotTime,
                    Rotation = trans.Rotation,
                    Type = BulletType.PISTOL
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

        //Move BulletInfo to EventsHolder class
        while (this.bulletToShoot.TryDequeue(out BulletInfo info))
        {
            EventsHolder.PistolBulletToShoot.Add(info);
        }
    }
}