using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public class RotatePlayerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((Entity e, ref Rotation rotation, ref TargetData target, ref PlayerTag playerTag, ref Translation translation) =>
            {
                Vector3 relativePos = new Vector3(target.Value.x, target.Value.y, target.Value.z) - new Vector3(translation.Value.x, translation.Value.y, translation.Value.z);
                rotation.Value = quaternion.LookRotation(relativePos, Vector3.up);
            }).Schedule();
    }
}
