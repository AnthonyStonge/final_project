using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;
using static Unity.Mathematics.math;
using quaternion = Unity.Mathematics.quaternion;
using RaycastHit = Unity.Physics.RaycastHit;
[UpdateAfter(typeof(MoveSystem))]
[UpdateBefore(typeof(EndFramePhysicsSystem))]
public class ProjectileHitDetectionSystem : JobComponentSystem
{
    private BuildPhysicsWorld _physicsWorld;
    private PreTransformGroupBarrier preTransformBarrier;
    private EntityQuery ProjectilesQuery;
    
    //[BurstCompile]
    struct HitDetectionJob : IJob
    {
        [ReadOnly]
        public PhysicsWorld PhysicsWorld;
        public EntityCommandBuffer entityCommandBuffer;

        public float deltaTime;

        [DeallocateOnJobCompletion]
        public NativeArray<RaycastHit> RaycastHits;

        [DeallocateOnJobCompletion]
        public NativeArray<DamageProjectile> Projectiles;
        [DeallocateOnJobCompletion]
        public NativeArray<Entity> ProjectileEntities;
        [DeallocateOnJobCompletion]
        public NativeArray<Translation> ProjectileTranslations;
        [DeallocateOnJobCompletion]
        public NativeArray<Rotation> ProjectileRotations;
        // public ComponentDataFromEntity<Health> HealthsFromEntity;

        public void Execute()
        {
            CollisionFilter filter = new CollisionFilter
            {
                BelongsTo = 1u << 0,
                CollidesWith = 1u << 2,
                GroupIndex = 0
            };
            for (int i = 0; i < Projectiles.Length; i++)
            {
                

                // RaycastInput raycastInput = new RaycastInput
                // {
                //     Start = Projectiles[i].PreviousPosition,
                //     End = ProjectileTranslations[i].Value,
                //     Filter = filter
                // };
                
                RaycastInput raycastInput = new RaycastInput
                {
                    Start = ProjectileTranslations[i].Value,
                    End = ProjectileTranslations[i].Value + (math.forward(ProjectileRotations[i].Value) * Projectiles[i].Speed * deltaTime),
                    Filter = filter
                };
                MaxHitsCollector<RaycastHit> collector = new MaxHitsCollector<RaycastHit>(1.0f, ref RaycastHits);

                if (PhysicsWorld.CastRay(raycastInput, ref collector))
                {
                    if (collector.NumHits > 0)
                    {
                        RaycastHit closestHit = new RaycastHit();
                        closestHit.Fraction = 2f;

                        for (int j = 0; j < collector.NumHits; j++)
                        {
                            if(RaycastHits[j].Fraction < closestHit.Fraction)
                            {
                                closestHit = RaycastHits[j];
                            }
                        }

                        // Apply damage to hit rigidbody/collider
                        Entity hitEntity = PhysicsWorld.Bodies[closestHit.RigidBodyIndex].Entity;
                        // if (HealthsFromEntity.Exists(hitEntity))
                        // {
                        //     Health h = HealthsFromEntity[hitEntity];
                        //     h.Value -= Projectiles[i].Damage;
                        //     HealthsFromEntity[hitEntity] = h;
                        // }

                        // Destroy projectile
                        entityCommandBuffer.DestroyEntity(ProjectileEntities[i]);
                    }
                }
            }
        }
    }

    protected override void OnCreate()
    {
        base.OnCreate();

        preTransformBarrier = World.GetOrCreateSystem<PreTransformGroupBarrier>();
        _physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();

        EntityQueryDesc queryDesc = new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                ComponentType.ReadOnly<DamageProjectile>(),
                ComponentType.ReadOnly<Translation>(), 
                ComponentType.ReadOnly<Rotation>()
            }
        };
        ProjectilesQuery = GetEntityQuery(queryDesc);
    }

    protected override JobHandle OnUpdate(JobHandle inputDependencies)
    {
        // TODO: launch multiple separate IJobs for projectile collision detection? like one per thread

        HitDetectionJob hitDetectionJob = new HitDetectionJob
        {
            PhysicsWorld = _physicsWorld.PhysicsWorld,
            entityCommandBuffer = preTransformBarrier.CreateCommandBuffer(),
            RaycastHits = new NativeArray<RaycastHit>(64, Allocator.TempJob),
            Projectiles = ProjectilesQuery.ToComponentDataArray<DamageProjectile>(Allocator.TempJob),
            ProjectileEntities = ProjectilesQuery.ToEntityArray(Allocator.TempJob),
            ProjectileTranslations = ProjectilesQuery.ToComponentDataArray<Translation>(Allocator.TempJob),
            ProjectileRotations = ProjectilesQuery.ToComponentDataArray<Rotation>(Allocator.TempJob),
            deltaTime = Time.DeltaTime
            // HealthsFromEntity = GetComponentDataFromEntity<Health>()
            
        };
        
        inputDependencies = hitDetectionJob.Schedule(inputDependencies);
        preTransformBarrier.AddJobHandleForProducer(inputDependencies);

        inputDependencies.Complete(); 

        return inputDependencies;
    }
}