using Enums;
using EventStruct;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;

[DisableAutoCreation]
[UpdateAfter(typeof(TranslateSystem))] // Important
public class ProjectileHitDetectionSystem : SystemBase
{
    // private static readonly CollisionFilter Filter = new CollisionFilter
    // {
    //     BelongsTo = 1,
    //     CollidesWith = 1 << 10 | 1 << 2,
    //     GroupIndex = 0
    // };
    
    private BuildPhysicsWorld buildPhysicsWorld;
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;
    private NativeQueue<BulletInfo> bulletEvents;

    protected override void OnCreate()
    {
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        bulletEvents = new NativeQueue<BulletInfo>(Allocator.Persistent);
    }
    
    protected override void OnDestroy()
    {
        bulletEvents.Dispose();
    }
    
    protected override void OnUpdate()
    {
        
        var physicsWorld = buildPhysicsWorld.PhysicsWorld;
        var entityCommandBuffer = endSimulationEntityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
        
        //Create parallel writer
        NativeQueue<BulletInfo>.ParallelWriter events = bulletEvents.AsParallelWriter();
        
        //Get all enemy existing
        ComponentDataContainer<EnemyTag> enemies = new ComponentDataContainer<EnemyTag>
        {
            Components = GetComponentDataFromEntity<EnemyTag>(true)
        };
        //Get all entities with life
        ComponentDataContainer<LifeComponent> entitiesLife = new ComponentDataContainer<LifeComponent>
        {
            Components = GetComponentDataFromEntity<LifeComponent>(true)
        };
        //
        ComponentDataContainer<DropChanceComponent> dropChance = new ComponentDataContainer<DropChanceComponent>
        {
            Components = GetComponentDataFromEntity<DropChanceComponent>(true)
        };
        //Get Player entity
        Entity player = GameVariables.Player.Entity;

        
        JobHandle job = Entities.ForEach((Entity entity, int entityInQueryIndex, ref DamageProjectile projectile, ref Translation translation, in Rotation rotation, in BulletCollider bulletCollider) =>
        {
            CollisionFilter Filter = new CollisionFilter
            {
                BelongsTo = bulletCollider.BelongsTo.Value,
                CollidesWith = bulletCollider.CollidesWith.Value,
                GroupIndex = bulletCollider.GroupIndex
            };
            RaycastInput raycastInput = new RaycastInput
            {
                Start = projectile.PreviousPosition,
                End = translation.Value,
                Filter = Filter
            };
            
            //Cast ray
            if (physicsWorld.CollisionWorld.CastRay(raycastInput, out var hit))
            {
                //**If it gets to this point, it mean u should delete bullet and if HitEntity is not a wall -> Decrease life**
                
                Entity hitEntity = physicsWorld.Bodies[hit.RigidBodyIndex].Entity;

                //Collision = Wall until proven opposite
                BulletInfo.BulletCollisionType collisionType = BulletInfo.BulletCollisionType.ON_WALL;

                //Look if HitEntity is Player's
                if (hitEntity == player)
                {
                    collisionType = BulletInfo.BulletCollisionType.ON_PLAYER;
                }
                //Look if HitEntity is an enemy
                else if (enemies.Components.HasComponent(hitEntity))
                {
                    collisionType = BulletInfo.BulletCollisionType.ON_ENEMY;
                }

                
                //Damage entity
                if (collisionType != BulletInfo.BulletCollisionType.ON_WALL)
                {
                    //Make sure HitEntity has LifeComponent
                    if (entitiesLife.Components.HasComponent(hitEntity))
                    {
                        //Make sure HitEntity has LifeComponent
                        if (entitiesLife.Components.HasComponent(hitEntity))
                        {
                            //Get LifeComponent of this entity
                            LifeComponent life = entitiesLife.Components[hitEntity];
                            
                            //Decrease life
                            if (player == hitEntity)
                            {
                                DoDamage(ref life);
                            }
                            else
                            {
                                life.Life.Value--;
                            }
                            

                            //Set Back
                            entityCommandBuffer.SetComponent(entityInQueryIndex, hitEntity, life);
                        }
                    }
                }

                //Destroy bullet
                entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
                events.Enqueue(new BulletInfo
                {
                    ProjectileType = projectile.Type,
                    CollisionType = collisionType,
                    HitPosition = hit.Position,
                    HitRotation = rotation.Value
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

    private static void DoDamage(ref LifeComponent component)
    {
        component.DecrementLife();
    }

}