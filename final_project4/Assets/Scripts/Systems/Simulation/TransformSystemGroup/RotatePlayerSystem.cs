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
    private RaycastInput rayInfo;
    private BuildPhysicsWorld physicSystem;
    private PhysicsWorld pw;
    private RaycastHit rayCastInfos;
    private UnityEngine.Ray camRay;

    protected override void OnStartRunning()
    {
        physicSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
        pw = physicSystem.PhysicsWorld;
    }
    protected override void OnUpdate()
    {
        bool hitSomething = false;
        camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        rayInfo = new RaycastInput
        {
            Start = camRay.origin,
            End = camRay.GetPoint(100),
            Filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = ~0u,
                GroupIndex = 0
            }
        };
        float3 hitPoint = float3.zero;
        if (pw.CastRay(rayInfo, out rayCastInfos))
        {
            hitSomething = true;
            hitPoint = rayCastInfos.Position;
        }
        Entities.WithAll<PlayerTag>().ForEach((ref Rotation rotation, in TargetData target, in Translation translation) =>
            {
                if (hitSomething)
                {
                    float3 forward = hitPoint - translation.Value;
                    quaternion rot = quaternion.LookRotation(forward, math.up());
                    rotation.Value = math.normalize(new quaternion(0, rot.value.y, 0, rot.value.w));
                }


            }).Schedule();
       
    }
}
