using EventStruct;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateAfter(typeof(MoveSystem))] // Important
public class ProjectileHitDetectionSystem : SystemBase
{
    private static readonly CollisionFilter Filter = new CollisionFilter
    {
        BelongsTo = 1,
        CollidesWith = 1 << 10 | 1 << 2,
        GroupIndex = 0
    };
    
    private BuildPhysicsWorld physicsWorld;
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;
    private NativeQueue<BulletInfo> bulletEvents;

    protected override void OnCreate()
    {
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        bulletEvents = new NativeQueue<BulletInfo>(Allocator.Persistent);
    }
    
    protected override void OnUpdate()
    {
        
        PhysicsWorld PhysicsWorld = physicsWorld.PhysicsWorld;
        var entityCommandBuffer = endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
        
        //Create parallel writer
        NativeQueue<BulletInfo>.ParallelWriter events = bulletEvents.AsParallelWriter();
        //Get all enemy existing
        ComponentDataContainer<EnemyTag> enemies = new ComponentDataContainer<EnemyTag>
        {
            Components = GetComponentDataFromEntity<EnemyTag>(true)
        };
        ComponentDataContainer<DropChanceComponent> dropChance = new ComponentDataContainer<DropChanceComponent>
        {
            Components = GetComponentDataFromEntity<DropChanceComponent>(true)
        };


        
        
        JobHandle job = Entities.ForEach((Entity entity, int entityInQueryIndex, ref DamageProjectile projectile, ref Translation translation, in Rotation rotation) =>
        {
            RaycastInput raycastInput = new RaycastInput
            {
                Start = projectile.PreviousPosition,
                End = translation.Value,
                Filter = Filter
            };
            
            if (PhysicsWorld.CollisionWorld.CastRay(raycastInput, out var hit))
            {
                BulletInfo.BulletCollisionType collisionType = BulletInfo.BulletCollisionType.ON_WALL;
                
                Entity hitEntity = PhysicsWorld.Bodies[hit.RigidBodyIndex].Entity;

                if (enemies.Components.HasComponent(hitEntity))
                {
                    collisionType = BulletInfo.BulletCollisionType.ON_ENEMY;
                    entityCommandBuffer.DestroyEntity(entityInQueryIndex, hitEntity);
                }
                //TODO Handle everything that a projectile can collide with

                //Destroy bullet
                entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
                
                events.Enqueue(new BulletInfo
                {
                    ProjectileType = projectile.Type,
                    CollisionType = collisionType,
                    HitPosition = hit.Position,
                    HitRotation = math.inverse(rotation.Value)
                });
            }
        }).ScheduleParallel(Dependency);
        
        Dependency = JobHandle.CombineDependencies(job, new EventQueueJob{ weaponInfos = bulletEvents}.Schedule(job));

        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        
    }
    
    struct EventQueueJob : IJob
    {
        public NativeQueue<BulletInfo> weaponInfos;
        public void Execute()
        {
            while (weaponInfos.TryDequeue(out BulletInfo info))
            {
                EventsHolder.BulletsEvents.Add(info);
            }
        }
    }

    protected override void OnDestroy()
    {
        bulletEvents.Dispose();
    }
}