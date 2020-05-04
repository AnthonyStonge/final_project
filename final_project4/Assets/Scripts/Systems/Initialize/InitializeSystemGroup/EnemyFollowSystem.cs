using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public class EnemyFollowSystem : SystemBase
{
    private static readonly CollisionFilter Filter = new CollisionFilter
    {
        BelongsTo = 1 << 2,
        CollidesWith = 1 << 10 | 1 << 1,
        GroupIndex = 0
    };

    protected override void OnUpdate()
    {
        Entities.ForEach((ref PathFollowComponent pathFollow, ref DirectionData direction, ref TargetData targetData,ref Translation translation,
            in AttackRangeComponent range) =>
        {
            direction.Value = new float2(0);

            //Make sure enemy is out of range
           // if (range.IsInRange)
             //   return;
            //Make sure enemy has no position to go to
            if (pathFollow.PositionToGo.Equals(new int2(-1)))
                return;
            //Make sure enemy has reached 
            if (math.distancesq(pathFollow.PositionToGo,
                    translation.Value.xz) <= 1)
                return;
            
            float2 targetPos;
            if(pathFollow.EnemyState == EnemyState.Attack)
                targetPos = new float2(pathFollow.player.x, pathFollow.player.z);
            else
                targetPos = new float2(pathFollow.PositionToGo.x, pathFollow.PositionToGo.y);
            
            float2 moveDir = math.normalizesafe(targetPos - translation.Value.xz);
            if (!range.IsInRange) 
                direction.Value = moveDir;
            targetData.Value.xz = translation.Value.xz + moveDir;
        }).ScheduleParallel();
    }
}