using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
[DisableAutoCreation]
public class EnnemieFollowSystem : SystemBase
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
        float time = Time.DeltaTime;
        Entities.WithoutBurst().ForEach((DynamicBuffer<PathPosition> pathPos, ref Translation translation,
            ref PathFollowComponent pathFollow, ref PathFindingComponent pathFindingComponent,
            ref PhysicsVelocity physicsVelocity, ref EnnemyComponent ennemyComponent) =>
        {
            if (!pathFollow.PositionToGo.Equals(new int2(-1)) && math.distance(new float3(pathFollow.PositionToGo.x, 0.5f, pathFollow.PositionToGo.y), translation.Value) > 1f && !ennemyComponent.inRange)
            {
                float3 targetPos = new float3(pathFollow.PositionToGo.x, 0, pathFollow.PositionToGo.y);
                float3 moveDir = math.normalizesafe((int3)targetPos - translation.Value);
                float moveSpeed = 200;
                //translation.Value += moveDir * moveSpeed * time;
                if (math.distance((int3)targetPos, translation.Value) <= 1f)
                {
                    pathFollow.EnemyReachedTarget = true;
                }
                physicsVelocity.Linear = moveDir * moveSpeed * time;
            }
            else
            {
                physicsVelocity.Linear = 0;
            }
        }).ScheduleParallel();
    }
}
