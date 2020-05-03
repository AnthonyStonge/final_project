using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using Ray = Unity.Physics.Ray;
using RaycastHit = Unity.Physics.RaycastHit;
[DisableAutoCreation]
public class RotateSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Rotation rotation, ref Translation translation, in TargetData target) =>
        {
            
            float3 forward = target.Value - translation.Value;
            quaternion rot = quaternion.LookRotationSafe(forward, math.up());
            rotation.Value = math.normalize(new quaternion(0, rot.value.y, 0, rot.value.w));
        }).ScheduleParallel();
    }
}
