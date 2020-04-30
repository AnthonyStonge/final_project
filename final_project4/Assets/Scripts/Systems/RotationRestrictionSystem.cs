using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RotationRestrictionSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref RotationBlockComponent rotationBlockComponent, ref Rotation rotation) =>
        {
             rotation.Value = quaternion.Euler(0,rotation.Value.value.y,0);
        }).ScheduleParallel();
    }
}
