using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using RaycastHit = Unity.Physics.RaycastHit;

[UpdateAfter(typeof(BuildPhysicsWorld))]
public class NewBehaviourScript : SystemBase
{
    private BuildPhysicsWorld physicSystem;
    private PhysicsWorld pw;
    private RaycastInput rayCastInfos;

    protected override void OnCreate() 
    {
        
    }

    protected override void OnStartRunning()
    {
        physicSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystem<BuildPhysicsWorld>();
        pw = physicSystem.PhysicsWorld;
    }

    protected override void OnUpdate()
    { 
        rayCastInfos = new RaycastInput
        {
            Start = new float3(0, 5, 0),
            End = new float3(0, -5, 0),
            Filter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = ~0u,
                GroupIndex = 0
            }
        };
        
        CollisionWorld collisionWorld = pw.CollisionWorld;
        RaycastHit raycastHits = new RaycastHit();

        if (collisionWorld.CastRay(rayCastInfos, out raycastHits))
        {
            Debug.Log(raycastHits.Entity.Index);
        }
    }
}
