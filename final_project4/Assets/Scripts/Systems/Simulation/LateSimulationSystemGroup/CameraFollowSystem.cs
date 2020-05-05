using System.Linq;
using Cinemachine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
public class CameraFollowSystem : SystemBase
{
    private float length = 8;
    private float offsetRad = math.radians(-45f);

    protected override void OnUpdate()
    {
        //Get Player Translation, which was set by the physic system
        Translation t = EntityManager.GetComponentData<Translation>(GameVariables.Player.Entity);
        var currentPosition = t.Value;

        //Calculate Input
        InputComponent input = EntityManager.GetComponentData<InputComponent>(GameVariables.Player.Entity);
        
        var screenPos = new float2(input.Mouse.x - Screen.width * 0.5f, input.Mouse.y - Screen.height * 0.5f);
        var distance = math.clamp(math.distance(float2.zero, screenPos), 0, length);
        var oldPos = math.normalize(screenPos);

        var newDir = float3.zero;
        newDir.x = oldPos.x * math.cos(offsetRad) - oldPos.y * math.sin(offsetRad);
        newDir.z = oldPos.x * math.sin(offsetRad) + oldPos.y * math.cos(offsetRad);
        newDir.y = 0;
        newDir *= distance;

        GameVariables.MouseToTransform.position = currentPosition + newDir;
        GameVariables.Player.Transform.position = currentPosition;
        
        //Debug Ray
        var e = GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld];
        var localToWorld = EntityManager.GetComponentData<LocalToWorld>(e);
        #if UNITY_EDITOR
        Debug.DrawRay(localToWorld.Position, localToWorld.Forward * 5, Color.yellow);
        Debug.DrawRay(t.Value,newDir , Color.red);
        #endif 
    }
}