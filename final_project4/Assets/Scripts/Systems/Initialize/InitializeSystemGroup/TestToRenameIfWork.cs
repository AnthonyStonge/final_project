using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
[DisableAutoCreation]
[UpdateBefore(typeof(PathFinding))]
public class TestToRenameIfWork : SystemBase
{
    protected override void OnUpdate()
    {
        float3 posPlayer = EntityManager.GetComponentData<Translation>(GameVariables.Player.Entity).Value;
        Entities.ForEach((ref PathFollowComponent pathFollow, ref PathFindingComponent pathFindingComponent) =>
        {
            if (pathFollow.ennemyState == EnnemyState.Chase)
            {
                pathFindingComponent.endPos = new int2((int) posPlayer.x, (int) posPlayer.z);
            }
        }).ScheduleParallel();
    }
}
