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
        Entities.ForEach((ref PathFollowComponent pathFollow, ref DirectionData direction,
            in AttackRangeComponent range, in Translation translation) =>
        {
            direction.Value = new float2(0);

            //Make sure enemy is out of range
            if (range.IsInRange)
                return;
            //Make sure enemy has no position to go to
            if (pathFollow.PositionToGo.Equals(new int2(-1)))
                return;
            //Make sure enemy has reached 
            if (math.distancesq(pathFollow.PositionToGo,
                    translation.Value.xz) <= 1)
                return;

            float2 targetPos = new float2(pathFollow.PositionToGo.x, pathFollow.PositionToGo.y);
            //TODO WHY CAST INT2...
            targetPos = (int2) targetPos;
            float2 moveDir = math.normalizesafe(targetPos - translation.Value.xz);

            if (math.distancesq(targetPos, translation.Value.xz) <= 1)
            {
                pathFollow.EnemyReachedTarget = true;
            }

            direction.Value = moveDir;
        }).ScheduleParallel();
    }
}