using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[DisableAutoCreation]
public class RotatePlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity e, ref Rotation rotation, ref TargetData target, ref PlayerTag playerTag, ref Translation translation) =>
            {
                float3 relativePos = target.Value - translation.Value;
                rotation.Value = quaternion.LookRotation(relativePos, new float3(0,1,0));
            }).Schedule();
    }
}
