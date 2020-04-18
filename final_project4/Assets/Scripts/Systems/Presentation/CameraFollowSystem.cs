using System.Linq;
using Cinemachine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[DisableAutoCreation]
public class CameraFollowSystem : SystemBase
{
    private EntityManager entityManager;
    private float3 min, max;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        //TODO Should be initialized elsewhere
        GameObject player = new GameObject("PlayerTransform");
        GameObject cursor = new GameObject("CursorTransform");

        GameVariables.MouseToTransform = cursor.transform;
        GameVariables.PlayerVars.Transform = player.transform;

        min = new float3(-8, 0, -8);
        max = new float3(8, 0, 8);

        MonoGameVariables.instance.TargetGroupCamera.AddMember(GameVariables.PlayerVars.Transform, 2, 0);
        MonoGameVariables.instance.TargetGroupCamera.AddMember(GameVariables.MouseToTransform, 1, 0);
    }

    protected override void OnUpdate()
    {
        if (GameVariables.PlayerVars.Entity != Entity.Null)
        {
            // #Math#TransposeThatTransform#GroupSelfie
            InputComponent input = entityManager.GetComponentData<InputComponent>(GameVariables.PlayerVars.Entity);
            float3 pos = new float3(input.Mouse.x - Screen.width * 0.5f, 0, input.Mouse.y - Screen.height * 0.5f) / 20;
            float3 actualpos = math.clamp(pos, min, max);
            GameVariables.MouseToTransform.position = GameVariables.PlayerVars.CurrentPosition + actualpos;
        }
    }
}