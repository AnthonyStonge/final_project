using System.Linq;
using Cinemachine;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

//TODO TEMPORARY SYSTEM UNTIL WE INCLUDE CINEMACHINE
[UpdateAfter(typeof(PlayerTargetSystem))]
public class CameraFollowSystem : SystemBase
{
    private EntityManager entityManager;
    public bool entered = false;
    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        GameVariables.MouseToTransform = new GameObject().transform;
        GameVariables.PlayerVars.Transform = new GameObject().transform;
        GameVariables.PlayerVars.Transform.SetPositionAndRotation(Vector3.one, Quaternion.identity);
        GameVariables.MouseToTransform.SetPositionAndRotation(Vector3.one, Quaternion.identity);
    }

    protected override void OnUpdate()
    {
        if (!entered && MonoGameVariables.instance != null)
        {
            MonoGameVariables.instance.TargetGroupCamera.AddMember(GameVariables.PlayerVars.Transform, 1, 0);
            MonoGameVariables.instance.TargetGroupCamera.AddMember(GameVariables.MouseToTransform, 1, 0);
            entered = true;
        }
        if (entered)
        {

            GameVariables.PlayerVars.Transform.position = GameVariables.PlayerVars.CurrentPosition;
            InputComponent input = entityManager.GetComponentData<InputComponent>(GameVariables.PlayerVars.Entity);
            float3 pos = new float3(Screen.width/2 - input.Mouse.x,Screen.height/2 - input.Mouse.y, 0);
            TargetData target = entityManager.GetComponentData<TargetData>(GameVariables.PlayerVars.Entity);
            // GameVariables.MouseToTransform.position = GameVariables.PlayerVars.CurrentPosition + math.normalize(pos) * 25;
        }
        //Update MainCamera to player position
        // if (MonoGameVariables.instance.MainCamera == null) return;

        // MonoGameVariables.instance.MainCamera.transform.position = math.lerp(MonoGameVariables.instance.MainCamera.transform.position,
        //     GameVariables.PlayerVars.CurrentPosition + GameVariables.CameraVars.PlayerOffset, 0.1f);
    }
}