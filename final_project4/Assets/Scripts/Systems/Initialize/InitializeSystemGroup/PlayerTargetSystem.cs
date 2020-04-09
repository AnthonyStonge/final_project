using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

[DisableAutoCreation]
public class PlayerTargetSystem : SystemBase
{
    private BuildPhysicsWorld physicSystem;
    protected override void OnCreate()
    {
        physicSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        RaycastHit rayCastInfos;
        PhysicsWorld pw = physicSystem.PhysicsWorld;
        RaycastInput rayInfo;
        
        //Act on all entities with Target, Input and PlayerTag
        Entities.WithoutBurst().WithAll<PlayerTag>().ForEach((ref TargetData target, ref InputComponent inputs) =>
        {
            UnityEngine.Ray camRay = GameVariables.MainCamera.ScreenPointToRay(inputs.Mouse);
            rayInfo = new RaycastInput
            {
                Start = camRay.origin,
                End = camRay.GetPoint(2000),
                Filter = new CollisionFilter
                {
                    BelongsTo = 1u << 1,
                    CollidesWith = 1u << 1,
                    GroupIndex = 0
                }
            };
            if (pw.CastRay(rayInfo, out rayCastInfos))
            {
                target.Value = rayCastInfos.Position;
            }
        }).Run();
    }
}