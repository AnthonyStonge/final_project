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
            in AttackRangeComponent range, in TypeData typeData, in StateComponent state) =>
        {
            
            if (state.CurrentState == State.Dying)
                return;
            direction.Value = new float2(0);
        //Make sure enemy has no position to go to
        if (pathFollow.WonderingPosition.Equals(new int2(-1)))
                return;
            //Make sure enemy has reached 
            
            if (math.distancesq(pathFollow.WonderingPosition, translation.Value.xz) <= 1)
            {
                if (pathFollow.BeginWalk)
                    pathFollow.BeginWalk = false;
                pathFollow.EnemyState = EnemyState.Wondering;
                return;
            }
            float2 targetPos;
            if(pathFollow.EnemyState != EnemyState.Wondering)
                if(pathFollow.BeginWalk)
                    targetPos = pathFollow.WonderingPosition;
                else
                    targetPos = new float2(pathFollow.PlayerPosition.x, pathFollow.PlayerPosition.z);
            else
                targetPos = pathFollow.WonderingPosition;
            
            float2 moveDir = math.normalizesafe(targetPos - translation.Value.xz);
            
            if (!range.IsInRange) 
                direction.Value = moveDir;
            
            if (typeData.Value == Type.Pig && pathFollow.EnemyState == EnemyState.Attack)
            {
                if (range.FleeDistance > math.distance(pathFollow.PlayerPosition, translation.Value))
                {
                    float2 directionPig = math.normalizesafe(pathFollow.BackPosition - translation.Value.xz);
                    direction.Value = directionPig;
                }
            }
            targetData.Value.xz = translation.Value.xz + moveDir;
        }).ScheduleParallel();
    }
}