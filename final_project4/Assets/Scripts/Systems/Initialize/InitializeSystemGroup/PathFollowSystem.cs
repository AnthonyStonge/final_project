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
        //NativeArray<float3> p = new NativeArray<float3>(1, Allocator.TempJob);
        float time = Time.DeltaTime;
        float3 playerPosition = float3.zero;
        float test = 0.5f;
        bool findNewPath = true;
        ScriptableGrid scriptableGrid = GameVariables.grid;
        float3 posPlayer = EntityManager.GetComponentData<Translation>(GameVariables.Player.Entity).Value;
        //p[0] = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<Translation>(GameVariables.Player.Entity).Value;
        Entities.ForEach((DynamicBuffer<PathPosition> pathPos, ref Translation translation, ref PathFollow pathFollow, ref PathFindingComponent pathFindingComponent, ref PhysicsVelocity physicsVelocity) =>
        {
            if (pathFindingComponent.timeBeforeCheck <= 0)
            {
                

                //Debug.Log( new int2((((int) p[0].x < 0) ? (int) p[0].x - 1 : (int) p[0].x), (((int) p[0].z < 0) ? (int) p[0].z - 1 : (int) p[0].z)));
                //pathFindingComponent.endPos = new int2((((int) p[0].x < 0) ? (int) p[0].x - 1 : (int) p[0].x), (((int) p[0].z < 0) ? (int) p[0].z - 1 : (int) p[0].z));
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
                //int2 pathPosition = translation.Value
                int2 pathPosition2 = pathPos[pathFollow.pathIndex - 1].position.xy;
                //float3 targetPos = new float3(pathPosition.x, 0, pathPosition.y);
                float3 targetPos2 = new float3(pathPosition2.x, 0, pathPosition2.y);
                float3 moveDir = math.normalizesafe(targetPos2 - translation.Value);
                float moveSpeed = 300;
                physicsVelocity.Linear = moveDir * moveSpeed * time;
                //translation.Value = moveDir * moveSpeed * time;

                if (math.distance(translation.Value, targetPos2) < .5f)
                {
                    pathFollow.pathIndex--;
                }
            }
        }).ScheduleParallel();
    }
}