using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using Ray = Unity.Physics.Ray;
using RaycastHit = Unity.Physics.RaycastHit;
[DisableAutoCreation]
public class RotatePlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.WithAll<PlayerTag>().ForEach((ref Rotation rotation, in TargetData target, in Translation translation) =>
        {
            float3 forward = target.Value - translation.Value;
                quaternion rot = quaternion.LookRotation(forward, math.up());
                rotation.Value = math.normalize(new quaternion(0, rot.value.y, 0, rot.value.w));
        }).Schedule();
    }
}
