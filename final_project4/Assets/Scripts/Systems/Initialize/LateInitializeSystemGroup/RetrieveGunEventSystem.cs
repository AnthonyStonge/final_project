using Enums;
using EventStruct;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

struct EmptyEventQueueJob : IJob
{
    public NativeQueue<WeaponInfo> EventsQueue;

    public void Execute()
    {
        while (EventsQueue.TryDequeue(out WeaponInfo info))
        {
            EventsHolder.WeaponEvents.Add(info);
        }
    }
}

[DisableAutoCreation]
public class RetrieveGunEventSystem : SystemBase
{
    private EntityCommandBufferSystem entityCommandBuffer;
    private NativeQueue<WeaponInfo> weaponFired;

    protected override void OnCreate()
    {
        entityCommandBuffer = World.GetExistingSystem<EndInitializationEntityCommandBufferSystem>();
        if (entityCommandBuffer == null)
            Debug.Log("GET DOWN! Problem incoming...");
        weaponFired = new NativeQueue<WeaponInfo>(Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        weaponFired.Dispose();
    }

    protected override void OnUpdate()
    {
        //Create parallel writer
        NativeQueue<WeaponInfo>.ParallelWriter weaponFiredEvents = weaponFired.AsParallelWriter();

        //Create ECB
        EntityCommandBuffer.Concurrent ecb = entityCommandBuffer.CreateCommandBuffer().ToConcurrent();

        //Get all StateData components
        ComponentDataContainer<StateComponent> states = new ComponentDataContainer<StateComponent>
        {
            Components = GetComponentDataFromEntity<StateComponent>()
        };
        
        //Get Player inputs
        InputComponent inputs = EntityManager.GetComponentData<InputComponent>(GameVariables.Player.Entity);

        float deltaTime = Time.DeltaTime;

        JobHandle gunJob = Entities.ForEach(
            (Entity e, int entityInQueryIndex, ref GunComponent gun, in LocalToWorld transform, in Parent parent) =>
            {
                if (!inputs.Enabled)
                    return;
                
                //Make sure SwapDelay < 0
                if (gun.SwapTimer > 0)
                {
                    gun.SwapTimer -= deltaTime;
                    return;
                }
                
                //Make sure gun has a parent
                if (!states.Components.HasComponent(parent.Value))
                    return;

                //Variables local to job
                StateComponent state = states.Components[parent.Value];
                WeaponInfo.WeaponEventType? weaponEventType = null;

                if (gun.IsReloading)
                {
                    //Decrease time
                    gun.ReloadTime -= deltaTime;

                    if (!gun.IsReloading)
                    {
                        Reload(ref gun);
                    }
                }
                //Only if not reloading
                else if (gun.IsBetweenShot)
                {
                    //Decrease time
                    gun.BetweenShotTime -= deltaTime;
                }

                if (state.CurrentState == State.Reloading)
                    if (TryReload(ref gun))
                        weaponEventType = WeaponInfo.WeaponEventType.ON_RELOAD;

                //Should weapon be reloading?    //Deactivate this line to block auto reload
                if (TryStartReload(ref gun))
                    weaponEventType = WeaponInfo.WeaponEventType.ON_RELOAD;
                
                if (state.CurrentState == State.Attacking)
                    if (TryShoot(ref gun))
                    {
                        
                        weaponEventType = WeaponInfo.WeaponEventType.ON_SHOOT;
                        Shoot(entityInQueryIndex, ecb, ref gun, transform);
                    }

                //Add event to NativeQueue
                if (weaponEventType != null)
                {
                    weaponFiredEvents.Enqueue(new WeaponInfo
                    {
                        WeaponType = gun.WeaponType,
                        EventType = (WeaponInfo.WeaponEventType) weaponEventType,
                        Position = transform.Position,
                        Rotation = transform.Rotation
                    });
                }
            }).ScheduleParallel(Dependency);

        //Create job
        JobHandle emptyEventQueueJob = new EmptyEventQueueJob
        {
            EventsQueue = weaponFired
        }.Schedule(gunJob);

        //Link all jobs
        Dependency = JobHandle.CombineDependencies(gunJob, emptyEventQueueJob);
        entityCommandBuffer.AddJobHandleForProducer(Dependency);
    }

    //Returns true if weapons starts reloading
    private static bool TryStartReload(ref GunComponent gun)
    {
        //Check if still ammo in magazine
        if (gun.CurrentAmountBulletInMagazine > 0)
            return false;
        //Make sure gun isnt reloading already
        if (gun.IsReloading)
            return false;
        //Make sure there is ammo to reload
        if (gun.CurrentAmountBulletOnPlayer <= 0)
            return false;

        //
        StartReload(ref gun);
        return true;
    }

    //Returns true if weapons starts reloading
    private static bool TryReload(ref GunComponent gun)
    {
        //Make sure magazine isnt full yet
        if (gun.CurrentAmountBulletInMagazine == gun.MaxBulletInMagazine)
            return false;
        //Make sure gun isnt reloading already
        if (gun.IsReloading)
            return false;
        //Make sure there is ammo to reload
        if (gun.CurrentAmountBulletOnPlayer <= 0)
            return false;

        StartReload(ref gun);
        return true;
    }

    private static void StartReload(ref GunComponent gun)
    {
        gun.ReloadTime = gun.ResetReloadTime;
    }

    private static void Reload(ref GunComponent gun)
    {
        int amountAmmoToPutInMagazine = gun.MaxBulletInMagazine;

        //Make sure enough ammo on player to refill entire magazine
        if (gun.CurrentAmountBulletOnPlayer < gun.MaxBulletInMagazine)
        {
            amountAmmoToPutInMagazine = gun.CurrentAmountBulletOnPlayer;
        }

        //
        gun.CurrentAmountBulletOnPlayer -= amountAmmoToPutInMagazine;
        gun.CurrentAmountBulletInMagazine = amountAmmoToPutInMagazine;
    }

    //Returns true if weapons shoot
    private static bool TryShoot(ref GunComponent gun)
    {
        //Make sure not reloading or not between shot
        if (gun.IsBetweenShot || gun.IsReloading)
            return false;
        //Make sure magazine isnt empty
        if (gun.CurrentAmountBulletInMagazine <= 0)
            return false;

        return true;
    }

    private static void Shoot(int jobIndex, EntityCommandBuffer.Concurrent ecb, ref GunComponent gun,
        in LocalToWorld transform)
    {
        //Decrease bullets
        gun.CurrentAmountBulletInMagazine--;
        
        //Reset between shot timer
        gun.BetweenShotTime = gun.ResetBetweenShotTime - gun.BetweenShotTime;
         
        switch (gun.WeaponType)
        {
            case WeaponType.Pistol:
                ShootPistol(jobIndex, ecb, gun.BulletPrefab, transform.Position, transform.Rotation);
                break;
            case WeaponType.Shotgun:
                ShootShotgun(jobIndex, ecb, gun.BulletPrefab, transform.Position, transform.Rotation);
                break;
        }
    }

    private static void ShootPistol(int jobIndex, EntityCommandBuffer.Concurrent ecb, Entity bulletPrefab,
        float3 position, quaternion rotation)
    {
        //Create entity with prefab
        Entity bullet = ecb.Instantiate(jobIndex, bulletPrefab);

        //Set position/rotation
        ecb.SetComponent(jobIndex, bullet, new Translation
        {
            Value = position
        });
        ecb.SetComponent(jobIndex, bullet, new Rotation
        {
            Value = rotation
        });
    }

    private static void ShootShotgun(int jobIndex, EntityCommandBuffer.Concurrent ecb, Entity bulletPrefab,
        float3 position, quaternion rotation)
    {
        int nbBullet = 100;
        float degreeFarShot = math.radians(nbBullet * 2);
        float angle = degreeFarShot / nbBullet;
        quaternion startRotation = math.mul(rotation, quaternion.RotateY(-(degreeFarShot / 2)));

        for (int i = 0; i < nbBullet; i++)
        {
            Entity bullet = ecb.Instantiate(jobIndex, bulletPrefab);

            //Find rotation
            quaternion bulletRotation = math.mul(startRotation, quaternion.RotateY(angle * i));

            //Set position/rotation
            ecb.SetComponent(jobIndex, bullet, new Translation
            {
                Value = position
            });
            ecb.SetComponent(jobIndex, bullet, new Rotation
            {
                Value = bulletRotation
            });
        }
    }
}