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
[UpdateBefore(typeof(EnnemieFollowSystem))]
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
        
        var physicsWorld = buildPhysicsWorld.PhysicsWorld;
        float time = Time.DeltaTime;
        float test = 0.5f;
        //bool findNewPath = true;
        ScriptableGrid scriptableGrid = GameVariables.grid;
        float3 posPlayer = EntityManager.GetComponentData<Translation>(GameVariables.Player.Entity).Value;
        Entities.ForEach((DynamicBuffer<PathPosition> pathPos, ref Translation translation, ref PathFollowComponent pathFollow, ref PathFindingComponent pathFindingComponent, ref PhysicsVelocity physicsVelocity) =>
        {
            if (pathFollow.EnemyReachedTarget)
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
                while (!findPlayer && (compteur != pathPos.Length && compteur != 5))
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
                        if (!HasComponent<PlayerTag>(hit.Entity))
                        {
                            pathFollow.PositionToGo = pathPos[(pathFollow.pathIndex - compteur) + 1].position;
                            pathFollow.EnemyReachedTarget = false;
                            Debug.Log("bob1");
                            break;
                            
                        }
                    }
                    compteur++;
                }
                if (compteur == 5)
                {
                    pathFollow.PositionToGo = pathPos[(pathFollow.pathIndex - compteur)].position;
                    pathFollow.EnemyReachedTarget = false;
                    Debug.Log("bob2");
                }
                else if (findPlayer)
                {
                    pathFollow.PositionToGo = new int2((int) posPlayer.x, (int) posPlayer.z);
                    pathFollow.EnemyReachedTarget = false;
                    Debug.Log("bob3");
                }
                
            }
            
        }).ScheduleParallel();
    }
}