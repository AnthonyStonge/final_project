using System.Diagnostics;
using System.Security.Cryptography;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
[DisableAutoCreation]
public class PathFollowSystem : SystemBase
{
    private static readonly CollisionFilter Filter = new CollisionFilter
    {
        BelongsTo = 1 << 2,
        CollidesWith = 1 << 10 | 1 << 1,
        GroupIndex = 0
    };
    private BuildPhysicsWorld buildPhysicsWorld;
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;
    protected override void OnCreate()
    {
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }
    protected override void OnUpdate()
    {
        //NativeArray<float3> p = new NativeArray<float3>(1, Allocator.TempJob);
        var physicsWorld = buildPhysicsWorld.PhysicsWorld;
        float time = Time.DeltaTime;
        float3 playerPosition = float3.zero;
        float test = 0.5f;
        //bool findNewPath = true;
        ScriptableGrid scriptableGrid = GameVariables.grid;
        float3 posPlayer = EntityManager.GetComponentData<Translation>(GameVariables.Player.Entity).Value;
        //p[0] = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Translation>(GameVariables.Player.Entity).Value;
        Entities.WithoutBurst().ForEach((DynamicBuffer<PathPosition> pathPos, ref Translation translation, ref PathFollowComponent pathFollow, ref PathFindingComponent pathFindingComponent, ref PhysicsVelocity physicsVelocity) =>
        {
            if (pathFindingComponent.timeBeforeCheck <= 0)
            {
                pathFindingComponent.endPos = new int2((int) posPlayer.x, (int) posPlayer.z);
                pathFindingComponent.findPath = 0;
                pathFindingComponent.timeBeforeCheck = test;
            }
            else
            {
                pathFindingComponent.timeBeforeCheck -= time;
            }
            if (pathFollow.pathIndex > 0)
            {
                //previousPos = PATHRESET;
                pathFollow.PositionToGo = new int2(1000);
                int compteur = 1;
                bool findPlayer = false;
                RaycastInput raycastInput2 = new RaycastInput
                {
                    Start = translation.Value,
                    End = posPlayer,
                    Filter = Filter
                };
                if (physicsWorld.CollisionWorld.CastRay(raycastInput2, out var hit2))
                {
                    if (HasComponent<PlayerTag>(hit2.Entity))
                    {
                        findPlayer = true;
                    }
                }
                while (!findPlayer && compteur != pathPos.Length)
                {
                    
                    int2 pathPosition = pathPos[pathFollow.pathIndex - compteur].position;
                    //Debug.Log(pathPosition);
                    float3 pathPositionConvert = new float3(pathPosition.x, 0.2f, pathPosition.y);
                    RaycastInput raycastInput = new RaycastInput
                    {
                        Start = translation.Value,
                        End = pathPositionConvert,
                        Filter = Filter
                    };
                    if (physicsWorld.CollisionWorld.CastRay(raycastInput, out var hit))
                    {
                        if (!HasComponent<PlayerTag>(hit.Entity))
                        {
                            pathFollow.PositionToGo = pathPosition;
                            break;
                        }
                    }
                    compteur++;
                }
                if (pathFollow.PositionToGo.x != 1000)
                {
                    float3 targetPos = new float3(pathFollow.PositionToGo.x, 0, pathFollow.PositionToGo.y);
                    float3 moveDir = math.normalizesafe(targetPos - translation.Value);
                    float moveSpeed = 300;
                    physicsVelocity.Linear = moveDir * moveSpeed * time;
                }
                else
                {
                    Debug.Log("bob");
                    float3 targetPos = new float3(playerPosition.x, 0, playerPosition.z);
                    float3 moveDir = math.normalizesafe(targetPos - translation.Value);
                    float moveSpeed = 300;
                    physicsVelocity.Linear = moveDir * moveSpeed * time;
                }
            }
        }).ScheduleParallel();
    }
}