using System.Diagnostics;
using System.Security.Cryptography;
using Enums;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Math = System.Math;
using Random = UnityEngine.Random;
[DisableAutoCreation]
[UpdateBefore(typeof(EnnemieFollowSystem))]
public class PathFollowSystem : SystemBase
{
    private static readonly CollisionFilter Filter = new CollisionFilter
    {
        BelongsTo = 1 << 2,
        CollidesWith = 1 << 10 | 1 << 1,
        GroupIndex = 0
    };
    private static EntityManager em;
    private BuildPhysicsWorld buildPhysicsWorld;
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;
    protected override void OnCreate()
    {
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }
    protected override void OnUpdate()
    {
        Random random = new Random();
        var physicsWorld = buildPhysicsWorld.PhysicsWorld;
        float time = Time.DeltaTime;
        float test = 0.5f;
        float3 posPlayer = EntityManager.GetComponentData<Translation>(GameVariables.Player.Entity).Value;
        Entities.ForEach((int entityInQueryIndex, DynamicBuffer<PathPosition> pathPos, ref Translation translation, ref PathFollowComponent pathFollow, ref PathFindingComponent pathFindingComponent, ref PhysicsVelocity physicsVelocity) =>
        {
            switch (pathFollow.ennemyState)
            {
                case EnnemyState.Attack:
                    
                    break;
                case EnnemyState.Chase:
                    ChaseFollow(ref pathFollow, translation, posPlayer, ref physicsWorld, pathPos);
                    break;
                
                case EnnemyState.Wondering:
                    WonderingFollow(entityInQueryIndex, ref pathFollow, translation, ref physicsWorld, time);
                    break;
            }
            if (math.distance(translation.Value, posPlayer) <= 20)
            {
               // pathFollow.ennemyState = EnnemyState.Chase;
                /*RaycastInput raycastInput2 = new RaycastInput
                {
                    Start = translation.Value,
                    End = posPlayer,
                    Filter = Filter
                };
                if (physicsWorld.CollisionWorld.CastRay(raycastInput2, out var hit2))
                {
                    /*if (em.HasComponent<PlayerTag>(hit2.Entity))
                    {
                        //pathFollow.ennemyState = EnnemyState.Attack;
                    }*/
                //}
            }
            else
            {
                pathFollow.ennemyState = EnnemyState.Wondering;
            }
            
        }).ScheduleParallel();
    }
    private static void ChaseFollow(ref PathFollowComponent pathFollow, in Translation translation, in float3 posPlayer, ref PhysicsWorld physicsWorld, in DynamicBuffer<PathPosition> pathPos)
    {
        pathFollow.PositionToGo = new int2(-1);
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
                if (!em.HasComponent<PlayerTag>(hit.Entity))
                {
                    pathFollow.PositionToGo = pathPos[(pathFollow.pathIndex - compteur) + 2].position;
                    pathFollow.EnemyReachedTarget = false;
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
            pathFollow.EnemyReachedTarget = false;
        }
    }
    private static void WonderingFollow(in int seed, ref PathFollowComponent pathFollow, in Translation translation, ref PhysicsWorld physicsWorld, in float time)
    {
        if (pathFollow.timeWonderingCounter < 0)
        {
            var rSeed = new System.Random(seed);
            Unity.Mathematics.Random random = new Unity.Mathematics.Random();
            int randomAngle = new Unity.Mathematics.Random((uint) rSeed.Next()).NextInt(0, 360);
            int rayDistance = new Unity.Mathematics.Random((uint) rSeed.Next()).NextInt(1, 10);
            pathFollow.timeWonderingCounter = new Unity.Mathematics.Random((uint) rSeed.Next()).NextInt(0, 6);
            float angle = math.radians(randomAngle);
            pathFollow.PositionToGo = (int2)(translation.Value + new float3(math.cos(angle), 0, math.sin(angle)) * rayDistance).xz;
            RaycastInput raycastInput = new RaycastInput
            {
                Start = translation.Value,
                End = new float3(pathFollow.PositionToGo.x,0.5f,pathFollow.PositionToGo.y),
                Filter = Filter
            };
            if (physicsWorld.CollisionWorld.CastRay(raycastInput, out var hit))
            {
                pathFollow.PositionToGo = new int2(-1);
                pathFollow.timeWonderingCounter = 0;
            }
        }
        else
        {
            pathFollow.timeWonderingCounter -= time;
        }
        
        
    }
    private void AttackFollow()
    {
        
    }
}
