using EventStruct;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

public class RetrieveGunEventSystem : SystemBase
{
    private EntityCommandBufferSystem simulationECB;
    private NativeQueue<WeaponInfo> weaponFired;

    protected override void OnCreate()
    {
        this.simulationECB = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
        if (this.simulationECB == null)
            Debug.Log("GET DOWN! Problem incoming...");
        this.weaponFired = new NativeQueue<WeaponInfo>(Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        this.weaponFired.Dispose();
    }

    protected override void OnUpdate()
    {
        //Clear previous PistolBullet events
        EventsHolder.WeaponEvents.Clear(); //TODO MOVE TO CLEANUPSYSTEM

        //Create parallel writer
        NativeQueue<WeaponInfo>.ParallelWriter weaponFiredEvents = this.weaponFired.AsParallelWriter();

        //Create ECB
        EntityCommandBuffer.Concurrent ecb = this.simulationECB.CreateCommandBuffer().ToConcurrent();

        //Get all StateData components
        Container states = new Container
        {
            allStateData = GetComponentDataFromEntity<StateData>()
        };

        float deltaTime = Time.DeltaTime;
        EntityArchetype entityArchetype = StaticArchetypes.BulletArchetype;

        JobHandle job = Entities.ForEach(
            (int entityInQueryIndex, ref GunComponent gun, ref LocalToWorld transform, in Parent parent) =>
            {
                if (!states.allStateData.HasComponent(parent.Value))
                    return;
                
                //Variables local to job
                StateData state = states.allStateData[parent.Value];
                WeaponInfo.WeaponEventType weaponEventType = WeaponInfo.WeaponEventType.NONE;

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
                    Entity e = ecb.CreateEntity(entityInQueryIndex, entityArchetype);
                    ecb.SetComponent(entityInQueryIndex, e, new Translation
                    {
                        Value = transform.Position
                    });
                    ecb.SetComponent(entityInQueryIndex, e, new Rotation
                    {
                        Value = transform.Rotation
                    });
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
            }).ScheduleParallel(this.Dependency);

        //Terminate job so we can read from NativeQueue
        job.Complete();

        //Transfer NativeQueue info to static NativeList
        while (weaponFired.TryDequeue(out WeaponInfo info))
        {
            EventsHolder.WeaponEvents.Add(info);
        }

        this.simulationECB.AddJobHandleForProducer(this.Dependency);
    }
    
    private struct Container
    {
        [ReadOnly] public ComponentDataFromEntity<StateData> allStateData;
    }
}