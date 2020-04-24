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
public class PathFollowSystem : SystemBase
{
    protected override void OnUpdate()
    {
        NativeArray<float3> p = new NativeArray<float3>(1, Allocator.TempJob);
        float time = Time.DeltaTime;
        float3 playerPosition = float3.zero;
        float test = 0.5f;
        JobHandle playerJob = Entities.WithAll<PlayerTag>().ForEach((ref Translation translation) => { p[0] = translation.Value; }).Schedule(Dependency);
        JobHandle bob = Entities.ForEach((DynamicBuffer<PathPosition> pathPos, ref Translation translation, ref PathFollow pathFollow, ref PathFindingComponent pathFindingComponent, ref PhysicsVelocity physicsVelocity) =>
        {
            if (pathFindingComponent.timeBeforeCheck <= 0)
            {
                pathFindingComponent.endPos = new int2((int)p[0].x,(int)p[0].z);
                pathFindingComponent.findPath = 0;
                pathFindingComponent.timeBeforeCheck = test;
            }
            else
            {
                pathFindingComponent.timeBeforeCheck -= time;
            }
            if (pathFollow.pathIndex > 0)
            {
                //int2 pathPosition = translation.Value
                int2 pathPosition2 = pathPos[pathFollow.pathIndex - 1].position.xy;
                //float3 targetPos = new float3(pathPosition.x, 0, pathPosition.y);
                float3 targetPos2 = new float3(pathPosition2.x, 0, pathPosition2.y);
                float3 moveDir = math.normalizesafe(targetPos2 - translation.Value);
                float moveSpeed = 300;
                physicsVelocity.Linear = moveDir * moveSpeed * time;
                
                if (math.distance(translation.Value, targetPos2) < .1f)
                {
                    pathFollow.pathIndex--;
                }
            }
        }).WithDeallocateOnJobCompletion(p).ScheduleParallel(playerJob);
        
        Dependency = JobHandle.CombineDependencies(playerJob, bob);
    }
}
