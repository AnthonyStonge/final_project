using System;
using Enums;
using EventStruct;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


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
        //Clear previous PistolBullet events
        EventsHolder.WeaponEvents.Clear(); //TODO MOVE TO CLEANUPSYSTEM

        //Create parallel writer
        NativeQueue<WeaponInfo>.ParallelWriter weaponFiredEvents = weaponFired.AsParallelWriter();

        //Create ECB
        EntityCommandBuffer.Concurrent ecb = entityCommandBuffer.CreateCommandBuffer().ToConcurrent();

        //Get all StateData components
        ComponentDataContainer<StateData> states = new ComponentDataContainer<StateData>
        {
            Components = GetComponentDataFromEntity<StateData>()
        };

        float deltaTime = Time.DeltaTime;

        JobHandle gunJob = Entities.ForEach(
            (int entityInQueryIndex, ref GunComponent gun, ref LocalToWorld transform, in Parent parent) =>
            {
                if (!states.Components.HasComponent(parent.Value))
                    return;

                //Variables local to job
                StateData state = states.Components[parent.Value];
                WeaponInfo.WeaponEventType weaponEventType = WeaponInfo.WeaponEventType.NONE;

                if (gun.IsBetweenShot)
                {
                    gun.BetweenShotTime -= deltaTime;
                }

                if (gun.IsReloading)
                {
                    gun.ReloadTime -= deltaTime;
                    if (gun.ReloadTime <= 0)
                    {
                        //Refill magazine
                        if (gun.CurrentAmountBulletOnPlayer >= gun.MaxBulletInMagazine)
                        {
                            gun.CurrentAmountBulletInMagazine = gun.MaxBulletInMagazine;
                            gun.CurrentAmountBulletOnPlayer -= gun.MaxBulletInMagazine;
                        }
                        else
                        {
                            gun.CurrentAmountBulletInMagazine = gun.CurrentAmountBulletOnPlayer;
                            gun.CurrentAmountBulletOnPlayer -= gun.CurrentAmountBulletOnPlayer;
                        }
                    }
                }
                else if (state.Value == StateActions.ATTACKING && !gun.IsBetweenShot &&
                         gun.CurrentAmountBulletInMagazine > 0)
                {
                    //Shoot event
                    gun.CurrentAmountBulletInMagazine--;

                    //Set EventType
                    weaponEventType = WeaponInfo.WeaponEventType.ON_SHOOT;

                    //Create entity in EntityCommandBuffer
                    //TODO GET PREFAB ENTITY LINK WITH GUNTYPE
                    switch (gun.GunType)
                    {
                        case GunType.PISTOL:
                            ShootPistol(entityInQueryIndex, ecb, gun.BulletPrefab, transform.Position,
                                transform.Rotation);
                            break;
                        case GunType.SHOTGUN:
                            ShootShotgun(entityInQueryIndex, ecb, gun.BulletPrefab, transform.Position,
                                transform.Rotation);
                            break;
                    }

                    gun.BetweenShotTime = 0.04f - gun.BetweenShotTime;
                }
                else if (gun.CurrentAmountBulletInMagazine <= 0 && gun.CurrentAmountBulletOnPlayer > 0)
                {
                    //Reload event
                    gun.ReloadTime = gun.ResetReloadTime;

                    //Set EventType
                    weaponEventType = WeaponInfo.WeaponEventType.ON_RELOAD;
                }

                if (weaponEventType != WeaponInfo.WeaponEventType.NONE)
                    //Add event to NativeQueue
                    weaponFiredEvents.Enqueue(new WeaponInfo
                    {
                        GunType = gun.GunType,
                        EventType = weaponEventType,
                        Position = transform.Position,
                        Rotation = transform.Rotation
                    });
            }).ScheduleParallel(Dependency);

        Dependency = JobHandle.CombineDependencies(gunJob, new EventQueueJob{ weaponInfos = weaponFired }.Schedule(gunJob));
        entityCommandBuffer.AddJobHandleForProducer(Dependency);
    }
    struct EventQueueJob : IJob
    {
        
        public NativeQueue<WeaponInfo> weaponInfos;
        public void Execute()
        {
            while (weaponInfos.TryDequeue(out WeaponInfo info))
            {
                EventsHolder.WeaponEvents.Add(info);
            }
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
        float degreeFarShot = math.radians(3);

        //Create center bullet
        Entity centerBullet = ecb.Instantiate(jobIndex, bulletPrefab);

        //Set position/rotation
        ecb.SetComponent(jobIndex, centerBullet, new Translation
        {
            Value = position
        });
        ecb.SetComponent(jobIndex, centerBullet, new Rotation
        {
            Value = rotation
        });

        //Create far left bullet
        Entity farLeftBullet = ecb.Instantiate(jobIndex, bulletPrefab);

        //Find rotation
        quaternion farLeftBulletRotation = math.mul(rotation, quaternion.RotateY(degreeFarShot));

        //Set position/rotation
        ecb.SetComponent(jobIndex, farLeftBullet, new Translation
        {
            Value = position
        });
        ecb.SetComponent(jobIndex, farLeftBullet, new Rotation
        {
            Value = farLeftBulletRotation
        });

        //Create far right bullet
        Entity farRightBullet = ecb.Instantiate(jobIndex, bulletPrefab);

        //Find rotation
        quaternion farRightBulletRotation = math.mul(rotation, quaternion.RotateY(-degreeFarShot));

        //Set position/rotation
        ecb.SetComponent(jobIndex, farRightBullet, new Translation
        {
            Value = position
        });
        ecb.SetComponent(jobIndex, farRightBullet, new Rotation
        {
            Value = farRightBulletRotation
        });
    }
}