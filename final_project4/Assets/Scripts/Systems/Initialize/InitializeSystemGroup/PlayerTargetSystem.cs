using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
using Collider = UnityEngine.Collider;
using RaycastHit = Unity.Physics.RaycastHit;

[DisableAutoCreation]
public class PlayerTargetSystem : SystemBase
{
    private EntityManager entityManager;

    private BuildPhysicsWorld physicSystem;
    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        physicSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        RaycastHit rayCastInfos;
        PhysicsWorld pw = physicSystem.PhysicsWorld;

        //Get Player InputsComponents
        InputComponent input = entityManager.GetComponentData<InputComponent>(GameVariables.Player.Entity);
        
        //Create ray cast
        UnityEngine.Ray camRay = GameVariables.MainCamera.ScreenPointToRay(input.Mouse);
        RaycastInput rayInfo = new RaycastInput
        {
            Start = camRay.origin,
            End = camRay.GetPoint(2000),
            Filter = new CollisionFilter
            {
                BelongsTo = 1u << 31,
                CollidesWith = 1u << 30,
                GroupIndex = 0
            }
        };
        
        //Create TargetData
        TargetData target = new TargetData();
        
        //Do ray cast
        if (pw.CastRay(rayInfo, out rayCastInfos))
        {
             var newPos = rayCastInfos.Position;
             newPos.x += 0.5f;
             newPos.y = 0f;
             target.Value = newPos;
        }
        
        //Set Player new TargetData
        entityManager.SetComponentData(GameVariables.Player.Entity, target);
    }
}