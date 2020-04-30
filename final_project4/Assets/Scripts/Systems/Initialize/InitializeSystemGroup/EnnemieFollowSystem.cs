using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
[DisableAutoCreation]
public class EnnemieFollowSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float time = Time.DeltaTime;
        Entities.WithoutBurst().ForEach((DynamicBuffer<PathPosition> pathPos, ref Translation translation,
            ref PathFollowComponent pathFollow, ref PathFindingComponent pathFindingComponent,
            ref PhysicsVelocity physicsVelocity) =>
        {
            if (pathPos.Length > 0)
            {
                float3 targetPos = new float3(pathFollow.PositionToGo.x, 0, pathFollow.PositionToGo.y);
                float3 moveDir = math.normalizesafe(targetPos - translation.Value);
                float moveSpeed = 300;
                physicsVelocity.Linear = moveDir * moveSpeed * time;
                //translation.Value += moveDir * moveSpeed * time;
                if (new int3((int)targetPos.x, 0, (int)targetPos.z).Equals(new int3((int)translation.Value.x, 0, (int)translation.Value.z)))
                    pathFollow.EnemyReachedTarget = true;
            }
        }).ScheduleParallel();
    }
}
