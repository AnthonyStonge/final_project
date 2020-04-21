using System.Linq;
using Cinemachine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public class CameraFollowSystem : SystemBase
{
    private float3 min, max;

    protected override void OnCreate()
    {
        min = new float3(-8, 0, -8);
        max = new float3(8, 0, 8);
    }

    protected override void OnUpdate()
    {
        //Get Player Translation, which was set by the physic system
        Translation t = EntityManager.GetComponentData<Translation>(GameVariables.PlayerVars.Entity);
        var currentPosition = t.Value;
        
        //Calculate Input
        InputComponent input = EntityManager.GetComponentData<InputComponent>(GameVariables.PlayerVars.Entity);
        float3 pos = new float3(input.Mouse.x - Screen.width * 0.5f, 0, input.Mouse.y - Screen.height * 0.5f) / 20;
        float3 actualpos = math.clamp(pos, min, max);
        
        GameVariables.MouseToTransform.position = currentPosition + actualpos;
        GameVariables.PlayerVars.Transform.position = currentPosition;
    }
}