using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using Collider = Unity.Physics.Collider;
using SphereCollider = Unity.Physics.SphereCollider;

public class FlockSystem : SystemBase
{
    [ReadOnly]
    BuildPhysicsWorld physicsWorldSystem;
    private static EntityManager em;
    protected override void OnCreate()
    {
        physicsWorldSystem = World.GetExistingSystem<BuildPhysicsWorld>();
        em = World.DefaultGameObjectInjectionWorld.EntityManager;
    }
    protected unsafe override void OnUpdate()
    {
        float time = Time.DeltaTime;
        float dirForward = Input.GetAxis("Vertical");
        float dirAngle = Input.GetAxis("Horizontal");
        CollisionWorld world = physicsWorldSystem.PhysicsWorld.CollisionWorld;
        Entities.ForEach((Entity e, ref PhysicsVelocity physicsVelocity, ref Rotation rotation, ref Translation translation, ref PhysicsCollider physicsCollider) =>
        {
            var filter = new CollisionFilter()
            {
                BelongsTo = ~0u,
                CollidesWith = ~0u, // all 1s, so all layers, collide with everything
                GroupIndex = 0
            };
 
            SphereGeometry sphereGeometry = new SphereGeometry() { Center = translation.Value, Radius = 10f };
            BlobAssetReference<Collider> sphereCollider = SphereCollider.Create(sphereGeometry, filter);
            NativeList<ColliderCastHit> colliderHit = new NativeList<ColliderCastHit>(Allocator.Temp);
            ColliderCastInput input = new ColliderCastInput()
            {
                Collider = (Collider*)sphereCollider.GetUnsafePtr(),
                Orientation = quaternion.identity,
                Start = translation.Value,
                End = translation.Value
            };
            world.CastCollider(input, ref colliderHit);
            //for (int i = 0; i < colliderHit.Length; i++)
            //{
                //Debug.Log(colliderHit[i].Entity + " / " + e.Index);
            //}
            //Debug.Log(colliderHit.Length);
            colliderHit.Dispose();
            //NativeArray<Entity> context = GetNearbyObjects(e, translation.Value, rotation.Value, physicsCollider, test);
            //context.Dispose();
        }).ScheduleParallel();
    }
    
    public unsafe static NativeArray<Entity> GetNearbyObjects(Entity e, float3 translation, quaternion rotation, PhysicsCollider physicsCollider, BuildPhysicsWorld physicsWorldSystem )
    {
        
        CollisionWorld world = physicsWorldSystem.PhysicsWorld.CollisionWorld;;
        NativeArray<Entity> context;
        var filter = new CollisionFilter()
        {
            BelongsTo = ~0u,
            CollidesWith = ~0u, // all 1s, so all layers, collide with everything
            GroupIndex = 0
        };
 
        SphereGeometry sphereGeometry = new SphereGeometry() { Center = float3.zero, Radius = 3f };
        BlobAssetReference<Collider> sphereCollider = SphereCollider.Create(sphereGeometry, filter);
        NativeList<ColliderCastHit> colliderHit = new NativeList<ColliderCastHit>();
        ColliderCastInput input = new ColliderCastInput()
        {
            Collider = (Collider*)sphereCollider.GetUnsafePtr(),
            Orientation = quaternion.identity,
            Start = translation,
            End = translation
        };
        if (world.CastCollider(input, ref colliderHit))
        {
            int compteur = 0;
            context = new NativeArray<Entity>(colliderHit.Length, Allocator.Temp);
            foreach (var collider in colliderHit)
            {
                context[compteur] = collider.Entity;
                compteur++;
            }
            Debug.Log(context.Length);
        }
        else
        {
            context = new NativeArray<Entity>(0, Allocator.Temp);
            Debug.Log("inhere");
        }
        return context;
    }
}
