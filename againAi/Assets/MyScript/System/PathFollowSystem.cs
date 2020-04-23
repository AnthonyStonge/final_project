using System.Diagnostics;
using System.Security.Cryptography;
using Unity.Entities;
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
        float time = Time.DeltaTime;
        float3 playerPosition = float3.zero;
        float test = 0.5f;
        Entities.WithAll<PlayerTag>().ForEach((ref Translation translation) => { playerPosition = translation.Value; }).Run();
        Entities.ForEach((DynamicBuffer<PathPosition> pathPos, ref Translation translation, ref PathFollow pathFollow, ref PathFindingComponent pathFindingComponent, ref PhysicsVelocity physicsVelocity) =>
        {
            if (pathFindingComponent.timeBeforeCheck <= 0)
            {
                pathFindingComponent.endPos = new int2((int)playerPosition.x,(int)playerPosition.z);
                pathFindingComponent.findPath = 0;
                pathFindingComponent.timeBeforeCheck = test;
            }
            else
            {
                pathFindingComponent.timeBeforeCheck -= time;
            }
            if (pathFollow.pathIndex > 0)
            {
                int2 pathPosition = pathPos[pathFollow.pathIndex].position;
                float3 targetPos = new float3(pathPosition.x, 0, pathPosition.y);
                float3 moveDir = math.normalizesafe(targetPos - translation.Value);
                float moveSpeed = 200f;
                //translation.Value += moveDir * moveSpeed * time;
                physicsVelocity.Linear = moveDir * moveSpeed * time;
                
                if (math.distance(translation.Value, targetPos) < .1f)
                {
                    pathFollow.pathIndex--;
                }
            }
            /*
             else
            {
                pathFindingComponent.endPos = new int2((int)playerPosition.x,(int)playerPosition.z);
                pathFindingComponent.findPath = 0;
            }*/
        }).ScheduleParallel();
    }
}
