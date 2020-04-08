using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using RaycastHit = Unity.Physics.RaycastHit;

[DisableAutoCreation]
[UpdateAfter(typeof(InputSystem))]
public class PlayerTargetSystem : SystemBase
{
    private RaycastInput rayInfo;
    private BuildPhysicsWorld physicSystem;
    private PhysicsWorld pw;
    private RaycastHit rayCastInfos;
    private UnityEngine.Ray camRay;
    
    protected override void OnCreate()
    {
        physicSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
        pw = physicSystem.PhysicsWorld;
    }

    protected override void OnUpdate()
    {
        //Act on all entities with Target, Input and PlayerTag
        Entities.WithAll<PlayerTag>().ForEach((ref TargetData target, ref InputComponent inputs) =>
        {
            camRay = GameVariables.MainCamera.ScreenPointToRay(inputs.Mouse);
        
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
        
            if (pw.CastRay(rayInfo, out rayCastInfos))
            {
                target.Value = rayCastInfos.Position;
            }
        }).Run();
    }
}