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
[DisableAutoCreation]
[UpdateBefore(typeof(EnemyFollowSystem))]
public class PathFollowSystem : SystemBase
{
    
    private static readonly CollisionFilter Filter = new CollisionFilter
    {
        BelongsTo = 1 << 2,
        CollidesWith = 1 << 10 | 1 << 1,
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
        
        Entities.ForEach((int nativeThreadIndex, DynamicBuffer<PathPosition> pathPos, ref PathFollowComponent pathFollow, ref AttackRangeComponent range, ref Translation translation, ref BulletCollider filter) =>
        {
            if (!pathFollow.BeginWalk)
            {
                range.IsInRange = false;
                switch (pathFollow.EnemyState)
                {
                    case EnemyState.Attack:
                        AttackFollow(posPlayer, ref pathFollow, ref range, translation);
                        break;
                    case EnemyState.Chase:
                        ChaseFollow(ref pathFollow, ref physicsWorld, ref player, pathPos);
                        break;
                    case EnemyState.Wondering:
                        WonderingFollow(ref pathFollow, ref physicsWorld, ref randomArray, translation, time, deltaTime,
                            nativeThreadIndex);
                        break;
                }

                //State changer 
                if (math.distancesq(translation.Value, posPlayer) <= range.AgroDistance * range.AgroDistance)
                {
                    RaycastInput raycastInput = new RaycastInput
                    {
                        Start = translation.Value,
                        End = posPlayer,
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
                            pathFollow.EnemyState = EnemyState.Attack;
                        }
                        else
                        {
                            if (pathFollow.EnemyState == EnemyState.Attack)
                            {
                                pathFollow.EnemyState = EnemyState.Chase;
                            }
                            if (pathFollow.EnemyState != EnemyState.Chase)
                                pathFollow.EnemyState = EnemyState.Wondering;
                        }
                    }
                }
                else
                {
                    pathFollow.EnemyState = EnemyState.Wondering;
                }
            }
        }).ScheduleParallel();
    }

    private static void ChaseFollow(ref PathFollowComponent pathFollow, ref PhysicsWorld physicsWorld,
        ref ComponentDataContainer<PlayerTag> player, in DynamicBuffer<PathPosition> pathPos)
    {
        /*pathFollow.PositionToGo = new int2(-1);
        int compteur = 0;
        bool loopBreak = false;
        while ((compteur != pathPos.Length && compteur != 4) && pathPos.Length > 0)
        {
            int2 basePos = pathPos[pathFollow.pathIndex].position;
            int2 pathPosition;
            if (pathFollow.pathIndex - compteur > 0)
                pathPosition = pathPos[pathFollow.pathIndex - compteur].position;
            else
                pathPosition = pathPos[0].position;

            float3 pathPositionConvert = new float3(pathPosition.x, 0.2f, pathPosition.y);
            float3 basePosConvert = new float3(basePos.x, 0.2f, basePos.y);
            RaycastInput raycastInput = new RaycastInput
            {
                Start = basePosConvert,
                End = pathPositionConvert,
                Filter = Filter
            };
            if (physicsWorld.CollisionWorld.CastRay(raycastInput, out var hit))
            {
                if (!player.Components.HasComponent(hit.Entity))
                {
                    if ((pathFollow.pathIndex - compteur + 1) != pathPos.Length)
                        pathFollow.PositionToGo = pathPos[(pathFollow.pathIndex - compteur) + 1].position;
                    else
                        pathFollow.PositionToGo = pathPos[(pathFollow.pathIndex - compteur)].position;
                    loopBreak = true;
                    break;
                }
            }

            compteur++;
        }

        if (!loopBreak)
        {
            if (pathPos.Length != 0)
            {
                if (pathFollow.pathIndex - compteur < 0)
                {
                    pathFollow.PositionToGo = pathPos[0].position;
                }

                pathFollow.PositionToGo = pathPos[(pathFollow.pathIndex - compteur) + 1].position;
            }
        }*/
    }
    private static void WonderingFollow(ref PathFollowComponent pathFollow,ref PhysicsWorld physicsWorld, ref NativeArray<Unity.Mathematics.Random> RandomArray, in Translation translation, in double time, in float deltaTime, in int naticeThreadIndex)
    {
        if (pathFollow.timeWonderingCounter < 0)
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
            
            pathFollow.PositionToGo = (int2)(translation.Value.xz + pos);
            
            //Check if it collides with anything
            RaycastInput raycastInput = new RaycastInput
            {
                Start = translation.Value,
                End = new float3(pathFollow.PositionToGo.x, 0.5f, pathFollow.PositionToGo.y),
                Filter = Filter
            };
            //If collides, do not move
            if (physicsWorld.CollisionWorld.CastRay(raycastInput))
            {
                
                pathFollow.PositionToGo = new int2(-1);
                pathFollow.timeWonderingCounter = 0;
            }
        }
        else
        {
            pathFollow.timeWonderingCounter -= deltaTime;
        }
    }

    private static void AttackFollow(float3 pos, ref PathFollowComponent pathFollow,
        ref AttackRangeComponent range, in Translation translation)
    {
        pathFollow.PositionToGo = (int2) pos.xz;
        pathFollow.player = pos;
        if (math.distance(pos, translation.Value) < range.AttackDDistance)
            range.IsInRange = true;
    }
    protected override void OnDestroy()
    {
        RandomArray.Dispose();
    }
}