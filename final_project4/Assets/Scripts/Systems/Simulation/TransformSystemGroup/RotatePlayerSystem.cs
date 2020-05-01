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
        //Act on player to rotate it toward its target
        Entity player = GameVariables.Player.Entity;

        TargetData target = EntityManager.GetComponentData<TargetData>(player);
        Translation translation = EntityManager.GetComponentData<Translation>(player);
        Rotation rotation = EntityManager.GetComponentData<Rotation>(player);
        
        float3 forward = target.Value - translation.Value;
        quaternion rot = quaternion.LookRotationSafe(forward, math.up());
        rotation.Value = math.normalize(new quaternion(0, rot.value.y, 0, rot.value.w));
        
        EntityManager.SetComponentData(player, rotation);
    }
}
