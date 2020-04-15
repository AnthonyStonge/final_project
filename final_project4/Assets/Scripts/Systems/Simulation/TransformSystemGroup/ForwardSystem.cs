using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class ForwardSystem : SystemBase
{
    protected override void OnUpdate()
    {
        // //Player
        // Entities.ForEach((ref ForwardData forwardData, in InputComponent ic) =>
        // {
        //     if (ic.Move.x == 0 && ic.Move.y == 0)
        //     {
        //         forwardData.Value = float3.zero;
        //     }
        //     else
        //     {
        //         float3 tmp = new float3(ic.Move.x, 0f, ic.Move.y);
        //         forwardData.Value = math.normalize(tmp);
        //     }
        //     
        // }).Schedule();
        //
        // //Enemies
        // Entities.WithNone<PlayerTag>().ForEach((ref ForwardData forwardData, in Rotation rotation) =>
        // {
        //     forwardData.Value = math.forward(rotation.Value);
        // }).ScheduleParallel();
    }
}
