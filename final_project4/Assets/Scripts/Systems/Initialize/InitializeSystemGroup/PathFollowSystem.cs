using System;
using System.ComponentModel;
using Enums;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;
using RaycastHit = Unity.Physics.RaycastHit;
using Type = Enums.Type;
[DisableAutoCreation]
[UpdateBefore(typeof(EnemyFollowSystem))]
public class PathFollowSystem : SystemBase
{
    private static readonly CollisionFilter Filter = new CollisionFilter
    {
        BelongsTo = 1 << 2,
        CollidesWith = 1 << 10 | 1 << 1 | 1 << 21,
        GroupIndex = 0
    };
    public NativeArray<Unity.Mathematics.Random> RandomArray { get; private set; }
    private BuildPhysicsWorld buildPhysicsWorld;
    protected override void OnCreate()
    {
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        var randomArray = new Random[1000];
        var seed = new System.Random();

        for (int i = 0; i < 1000; ++i)
            randomArray[i] = new Random((uint)seed.Next());

        RandomArray = new NativeArray<Random>(randomArray, Allocator.Persistent);
    }

    protected override void OnUpdate()
    {
        var randomArray = World.GetExistingSystem<PathFollowSystem>().RandomArray;
        ComponentDataContainer<PlayerTag> player = new ComponentDataContainer<PlayerTag>
        {
            Components = GetComponentDataFromEntity<PlayerTag>()
        };
        var physicsWorld = buildPhysicsWorld.PhysicsWorld;
        double time = Time.ElapsedTime;
        float deltaTime = Time.DeltaTime;
        float3 posPlayer = EntityManager.GetComponentData<Translation>(GameVariables.Player.Entity).Value;
        var level = EventsHolder.LevelEvents.CurrentLevel;
        
        Entities.ForEach((int nativeThreadIndex, ref PathFollowComponent pathFollow, ref AttackRangeComponent range, ref Translation translation, ref BulletCollider filter, ref TypeData typeData) =>
        {
            //if (math.distance(posPlayer, translation.Value) > 25 && level != MapType.Level_Hell)
            //    return;
            if (pathFollow.BeginWalk)
                return;
            range.IsInRange = false;
            range.CanAttack = false;
            
            //State changer 
            if (math.distancesq(translation.Value, posPlayer) <= range.AgroDistance * range.AgroDistance || level == MapType.Level_Hell)
            {
                RaycastInput raycastInput = new RaycastInput
                {
                    Start = new float3(translation.Value.x, 0, translation.Value.z),
                    End = new float3(posPlayer.x, 0, posPlayer.z),
                    Filter = new CollisionFilter()
                    {
                        BelongsTo = filter.BelongsTo.Value,
                        CollidesWith = filter.CollidesWith.Value,
                        GroupIndex = filter.GroupIndex
                    }
                };
                if (physicsWorld.CollisionWorld.CastRay(raycastInput, out var hit))
                {
                    if (player.Components.HasComponent(hit.Entity))
                    {
                        range.CanAttack = true;
                        pathFollow.EnemyState = EnemyState.Attack;
                    }
                    else
                    {
                        if (pathFollow.EnemyState == EnemyState.Attack)
                        {
                            pathFollow.EnemyState = EnemyState.Chase;
                        }
                    }
                }

            }
            else
            {
                pathFollow.EnemyState = EnemyState.Wondering;
            }
            switch (pathFollow.EnemyState)
            {
                case EnemyState.Attack:
                    AttackFollow(posPlayer, ref pathFollow, ref range, ref typeData, ref filter, ref physicsWorld, translation);
                    break;
                case EnemyState.Chase:
                    break;
                case EnemyState.Wondering:
                    WonderingFollow(ref pathFollow, ref physicsWorld, ref randomArray, translation, deltaTime,
                        nativeThreadIndex);
                    break;
            }
        }).ScheduleParallel();
    }
    
    private static void WonderingFollow(ref PathFollowComponent pathFollow,ref PhysicsWorld physicsWorld, ref NativeArray<Random> RandomArray, in Translation translation, in float deltaTime, in int naticeThreadIndex)
    {
        
        if (pathFollow.timeWonderingCounter <= 0)
        {
            //Get next seed
            var rSeed = RandomArray[naticeThreadIndex];
            
            //Get random Angle, distance and time to wonder
            int randomAngle = rSeed.NextInt(0, 360);
            int rayDistance = rSeed.NextInt(3, 7);
            pathFollow.timeWonderingCounter = rSeed.NextInt(1, 6);
            RandomArray[naticeThreadIndex] = rSeed;
            //Set the angle of wondering
            float angle = math.radians(randomAngle);
            float2 pos = new float2(math.cos(angle), math.sin(angle) * rayDistance);
            
            pathFollow.WonderingPosition = (int2)(translation.Value.xz + pos);
            
            //Check if it collides with anything
            RaycastInput raycastInput = new RaycastInput
            {
                Start = new float3(translation.Value.x, 0, translation.Value.z),
                End = new float3(pathFollow.WonderingPosition.x, 0, pathFollow.WonderingPosition.y),
                Filter = Filter
            };
            //If collides, do not move
            if (physicsWorld.CollisionWorld.CastRay(raycastInput, out RaycastHit hit))
            {
                pathFollow.WonderingPosition = new int2(-1);
                pathFollow.timeWonderingCounter = 0;
            }
        }
        else
        {
            pathFollow.timeWonderingCounter -= deltaTime;
        }
    }
    private static void ChaseFollow(ref PathFollowComponent pathFollow, in Translation translation)
    {
        /*if (math.distancesq(pathFollow.WonderingPosition, translation.Value.xz) <= 1)
        {
            pathFollow.EnemyState = EnemyState.Wondering;
        }*/
    }
    private static void AttackFollow(float3 pos, ref PathFollowComponent pathFollow,
        ref AttackRangeComponent range, ref TypeData typeData, ref BulletCollider bulletCollider, ref PhysicsWorld physicsWorld, in Translation translation)
    {
        pathFollow.WonderingPosition = new int2(1);
        pathFollow.PlayerPosition = pos;
        if (math.distance(pos, translation.Value) < range.AttackDDistance)
            range.IsInRange = true;
        if (typeData.Value == Type.Pig)
        {
            if (range.FleeDistance >= math.distance(translation.Value, pos))
            {
                float3 targetData = math.normalizesafe(pos - translation.Value);
                float3 direction = translation.Value + (-targetData * 2);
                RaycastInput raycastInput = new RaycastInput
                {
                    Start = new float3(translation.Value.x, 0, translation.Value.z),
                    End = new float3(direction.x, 0, direction.z),
                    Filter = new CollisionFilter()
                    {
                        BelongsTo = bulletCollider.BelongsTo.Value,
                        CollidesWith = bulletCollider.CollidesWith.Value,
                        GroupIndex = bulletCollider.GroupIndex
                    }
                };
                if (!physicsWorld.CollisionWorld.CastRay(raycastInput, out var hit))
                {
                    pathFollow.BackPosition = direction.xz;
                }
            }
        }
    }
    protected override void OnDestroy()
    {
        RandomArray.Dispose();
    }
}