using Static.Events;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using static GameVariables;

[DisableAutoCreation]
public class PistolSystem : SystemBase
{
    private NativeQueue<BulletInfo> bulletsToCreate;

    protected override void OnCreate()
    {
        this.bulletsToCreate = new NativeQueue<BulletInfo>(Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        this.bulletsToCreate.Dispose();
    }

    protected override void OnUpdate()
    {
        NativeQueue<BulletInfo>.ParallelWriter events = this.bulletsToCreate.AsParallelWriter();
        
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
                pistol.BetweenShotTime = betweenShotTime;
                pistol.CurrentBulletInMagazine--;
                if (pistol.CurrentBulletInMagazine == 0)
                {
                    pistol.ReloadTime = reloadTime;
                }

                //Add event to queue
                events.Enqueue(new BulletInfo
                {
                    position = trans.Position,
                    rotation = trans.Rotation
                });
            }
            else if (pistol.IsBetweenShot)
            {
                pistol.BetweenShotTime -= deltaTime;
            }
        }).ScheduleParallel(this.Dependency);

        //Terminate job before reading from array
        job.Complete();

        BulletInfo bulletInfo;
        //Call events for each bullets
        while (this.bulletsToCreate.TryDequeue(out bulletInfo))
        {
            GunEvents.OnShootPistol.Invoke(bulletInfo.position, bulletInfo.rotation);
        }
    }

    private struct BulletInfo
    {
        public float3 position;
        public quaternion rotation;
    }
}