using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using RaycastHit = Unity.Physics.RaycastHit;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class ProjectileHitDetectionSystem : SystemBase
{
    private BuildPhysicsWorld physicsWorld;
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;

    protected override void OnCreate()
    {
        base.OnCreate();

        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        PhysicsWorld PhysicsWorld = physicsWorld.PhysicsWorld;
        var entityCommandBuffer = endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();

        //Get all enemy existing
        ComponentDataContainer<EnemyTag> enemies = new ComponentDataContainer<EnemyTag>
        {
            Components = GetComponentDataFromEntity<EnemyTag>()
        };

        float deltaTime = Time.DeltaTime;

        CollisionFilter filter = new CollisionFilter
        {
            BelongsTo = 1 << 0,
            CollidesWith = 1 << 10 | 1 << 2,
            GroupIndex = 0
        };
        
        Entities.ForEach((Entity entity, int entityInQueryIndex, ref DamageProjectile projectile, in Translation translation, in Rotation rotation) =>
        {
            //System.IndexOutOfRangeException: Index {0} is out of range of '{4}' Length.
            //If collides with more than 4 things
            
            RaycastInput raycastInput = new RaycastInput
            {
                Start = translation.Value,
                End = translation.Value + (math.forward(rotation.Value) * projectile.Speed * deltaTime),
                Filter = filter
            };
                
            // MaxHitsCollector<RaycastHit> collector = new MaxHitsCollector<RaycastHit>(1.0f, ref raycastHits);
            RaycastHit hit = new RaycastHit();
            if (PhysicsWorld.CollisionWorld.CastRay(raycastInput, out hit))
            {
                Entity hitEntity = PhysicsWorld.Bodies[hit.RigidBodyIndex].Entity;
                
                if(enemies.Components.HasComponent(hitEntity))
                    entityCommandBuffer.DestroyEntity(entityInQueryIndex, hitEntity);

                entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
            }
        }).ScheduleParallel();

        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }
}

// public class ProjectileHitDetectionSystem : JobComponentSystem
// {
//     private BuildPhysicsWorld _physicsWorld;
//     private PostTransformGroupBarrier postTransformBarrier;
//     private EntityQuery ProjectilesQuery;
//     
//     //[BurstCompile]
//     struct HitDetectionJob : IJob
//     {
//         [ReadOnly]
//         public PhysicsWorld PhysicsWorld;
//         public EntityCommandBuffer entityCommandBuffer;
//
//         public float deltaTime;
//
//         [DeallocateOnJobCompletion]
//         public NativeArray<RaycastHit> RaycastHits;
//
//         [DeallocateOnJobCompletion]
//         public NativeArray<DamageProjectile> Projectiles;
//         [DeallocateOnJobCompletion]
//         public NativeArray<Entity> ProjectileEntities;
//         [DeallocateOnJobCompletion]
//         public NativeArray<Translation> ProjectileTranslations;
//         [DeallocateOnJobCompletion]
//         public NativeArray<Rotation> ProjectileRotations;
//         // public ComponentDataFromEntity<Health> HealthsFromEntity;
//         public CollisionFilter filter;
//         
//
//         public void Execute()
//         {
//             
//             for (int i = 0; i < Projectiles.Length; i++)
//             {
//                 RaycastInput raycastInput = new RaycastInput
//                 {
//                     Start = ProjectileTranslations[i].Value,
//                     End = ProjectileTranslations[i].Value + (math.forward(ProjectileRotations[i].Value) * Projectiles[i].Speed * deltaTime),
//                     Filter = filter
//                 };
//                 
//                 MaxHitsCollector<RaycastHit> collector = new MaxHitsCollector<RaycastHit>(1.0f, ref RaycastHits);
//
//                 if (PhysicsWorld.CastRay(raycastInput, ref collector))
//                 {
//                     if (collector.NumHits > 0)
//                     {
//                         RaycastHit closestHit = new RaycastHit();
//                         closestHit.Fraction = 2f;
//
//                         for (int j = 0; j < collector.NumHits; j++)
//                         {
//                             if(RaycastHits[j].Fraction < closestHit.Fraction)
//                             {
//                                 closestHit = RaycastHits[j];
//                             }
//                         }
//
//                         // Apply damage to hit rigidbody/collider
//                         Entity hitEntity = PhysicsWorld.Bodies[closestHit.RigidBodyIndex].Entity;
//                         // if (HealthsFromEntity.Exists(hitEntity))
//                         // {
//                         //     Health h = HealthsFromEntity[hitEntity];
//                         //     h.Value -= Projectiles[i].Damage;
//                         //     HealthsFromEntity[hitEntity] = h;
//                         // }
//
//                         // Destroy projectile
//                         entityCommandBuffer.DestroyEntity(ProjectileEntities[i]);
//                     }
//                 }
//             }
//         }
//     }
//
//     protected override void OnCreate()
//     {
//         base.OnCreate();
//
//         postTransformBarrier = World.GetOrCreateSystem<PostTransformGroupBarrier>();
//         _physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
//
//         EntityQueryDesc queryDesc = new EntityQueryDesc
//         {
//             All = new ComponentType[]
//             {
//                 ComponentType.ReadOnly<DamageProjectile>(),
//                 ComponentType.ReadOnly<Translation>(), 
//                 ComponentType.ReadOnly<Rotation>()
//             }
//         };
//         ProjectilesQuery = GetEntityQuery(queryDesc);
//     }
//
//     protected override JobHandle OnUpdate(JobHandle inputDependencies)
//     {
//         // TODO: launch multiple separate IJobs for projectile collision detection? like one per thread
//
//         HitDetectionJob hitDetectionJob = new HitDetectionJob
//         {
//             PhysicsWorld = _physicsWorld.PhysicsWorld,
//             entityCommandBuffer = postTransformBarrier.CreateCommandBuffer(),
//             RaycastHits = new NativeArray<RaycastHit>(64, Allocator.TempJob),
//             Projectiles = ProjectilesQuery.ToComponentDataArray<DamageProjectile>(Allocator.TempJob),
//             ProjectileEntities = ProjectilesQuery.ToEntityArray(Allocator.TempJob),
//             ProjectileTranslations = ProjectilesQuery.ToComponentDataArray<Translation>(Allocator.TempJob),
//             ProjectileRotations = ProjectilesQuery.ToComponentDataArray<Rotation>(Allocator.TempJob),
//             deltaTime = Time.DeltaTime,
//             filter = new CollisionFilter
//             {
//                 BelongsTo = 1u << 0,
//                 CollidesWith = 1u << 2,
//                 GroupIndex = 0
//             }
//             // HealthsFromEntity = GetComponentDataFromEntity<Health>()
//             
//         };
//         
//         inputDependencies = hitDetectionJob.Schedule(inputDependencies);
//         postTransformBarrier.AddJobHandleForProducer(inputDependencies);
//
//         inputDependencies.Complete(); 
//
//         return inputDependencies;
//     }
// }