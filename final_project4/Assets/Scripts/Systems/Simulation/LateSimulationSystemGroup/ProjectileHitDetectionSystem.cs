using EventStruct;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using RaycastHit = Unity.Physics.RaycastHit;

[DisableAutoCreation]
public class ProjectileHitDetectionSystem : SystemBase
{
    private BuildPhysicsWorld physicsWorld;
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;
    
    private NativeQueue<BulletInfo> bulletEvents;

    protected override void OnCreate()
    {
        base.OnCreate();

        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        physicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        
        bulletEvents = new NativeQueue<BulletInfo>(Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        bulletEvents.Dispose();
    }

    protected override void OnUpdate()
    {
        
        EventsHolder.BulletsEvents.Clear();    //TODO DO SOMEWHERE ELSE
        
        PhysicsWorld PhysicsWorld = physicsWorld.PhysicsWorld;
        var entityCommandBuffer = endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
        
        //Create parallel writer
        NativeQueue<BulletInfo>.ParallelWriter events = bulletEvents.AsParallelWriter();

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
        
        JobHandle job = Entities.ForEach((Entity entity, int entityInQueryIndex, ref DamageProjectile projectile, in Translation translation, in Rotation rotation) =>
        {
            RaycastInput raycastInput = new RaycastInput
            {
                Start = translation.Value,
                End = translation.Value + (math.forward(rotation.Value) * projectile.Speed * deltaTime),
                Filter = filter
            };
            
            RaycastHit hit = new RaycastHit();
            if (PhysicsWorld.CollisionWorld.CastRay(raycastInput, out hit))
            {
                BulletInfo.BulletCollisionType collisionType = BulletInfo.BulletCollisionType.ON_WALL;
                
                Entity hitEntity = PhysicsWorld.Bodies[hit.RigidBodyIndex].Entity;

                if (enemies.Components.HasComponent(hitEntity))
                {
                    collisionType = BulletInfo.BulletCollisionType.ON_ENEMY;
                    entityCommandBuffer.DestroyEntity(entityInQueryIndex, hitEntity);
                }
                
                //TODO PLAYER COLLISION

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
    
    //TODO CHANGE PLS LOL
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
}