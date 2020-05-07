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
using RaycastHit = Unity.Physics.RaycastHit;

[DisableAutoCreation]
[UpdateBefore(typeof(TranslateSystem))] // Important
public class ProjectileHitDetectionSystem : SystemBase
{
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
        //Get Physic World
        PhysicsWorld physicsWorld = buildPhysicsWorld.PhysicsWorld;

        //Create ECB
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

        JobHandle job = Entities.ForEach((Entity entity, int entityInQueryIndex, ref DamageProjectile projectile,
            ref Translation translation, in Rotation rotation, in BulletCollider bulletCollider,
            in BulletPreviousPositionData previousPosition) =>
        {
            //Create Personal Filter for each Bullet
            CollisionFilter filter = new CollisionFilter
            {
                BelongsTo = bulletCollider.BelongsTo.Value,
                CollidesWith = bulletCollider.CollidesWith.Value,
                GroupIndex = bulletCollider.GroupIndex
            };

            //Find direction
            float3 direction = translation.Value - previousPosition.Value;
            direction = math.normalizesafe(direction);

            //Find Vector to add
            float3 offset = default;
            float offsetRad = math.radians(90f);
            float cos = math.cos(offsetRad);
            float sin = math.sin(offsetRad);
            offset.x = direction.x * cos - direction.z * sin;
            offset.z = direction.x * sin + direction.z * cos;
            offset *= projectile.Radius;

            //Create Ray Inputs
            RaycastInput middleRayCast =
                GetRayCastInput(filter, translation.Value, previousPosition.Value, float3.zero);
            RaycastInput rightRayCast =
                GetRayCastInput(filter, translation.Value, previousPosition.Value, offset);
            RaycastInput leftRayCast =
                GetRayCastInput(filter, translation.Value, previousPosition.Value,
                    -offset); //Adding a negative radius

            bool hitDetected = false;
            Entity hitEntity = Entity.Null;
            RaycastHit hit = default;
            float3 hitPosition = default;

            //Try first RayCast
            if (physicsWorld.CollisionWorld.CastRay(middleRayCast, out RaycastHit middleRayCastHit))
            {
                hitDetected = true;
                hit = middleRayCastHit;
                hitPosition = float3.zero;

                //Get Hit Entity
                hitEntity = physicsWorld.Bodies[middleRayCastHit.RigidBodyIndex].Entity;
            }
            else if (physicsWorld.CollisionWorld.CastRay(rightRayCast, out RaycastHit rightRayCastHit))
            {
                hitDetected = true;
                hit = rightRayCastHit;
                hitPosition = -offset;

                //Get Hit Entity
                hitEntity = physicsWorld.Bodies[rightRayCastHit.RigidBodyIndex].Entity;
            }
            //Try second RayCast (Only if first one didnt hit)
            else if (physicsWorld.CollisionWorld.CastRay(leftRayCast, out RaycastHit leftRayCastHit))
            {
                hitDetected = true;
                hit = leftRayCastHit;
                hitPosition = offset;

                //Get Hit Entity
                hitEntity = physicsWorld.Bodies[leftRayCastHit.RigidBodyIndex].Entity;
            }

            //Make sure theres a collision
            if (!hitDetected)
                return;
            //Make sure an Entity was found
            if (hitEntity == Entity.Null)
                return;

            //Make sure collision hasn't been found before
            //TODO

            //Treat Collision

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

            bool shouldBulletBeDestroyed = true;

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
                            shouldBulletBeDestroyed = life.DecrementLifeWithInvincibility();
                        else
                            life.DecrementLife();

                        //Set Back
                        entityCommandBuffer.SetComponent(entityInQueryIndex, hitEntity, life);
                    }
                }
            }

            //Destroy bullet
            if (shouldBulletBeDestroyed)
            {
                entityCommandBuffer.DestroyEntity(entityInQueryIndex, entity);
                events.Enqueue(new BulletInfo
                {
                    HitEntity = hitEntity,
                    ProjectileType = projectile.Type,
                    CollisionType = collisionType,
                    HitPosition = hit.Position + hitPosition,
                    HitRotation = rotation.Value
                });
            }
        }).ScheduleParallel(Dependency);

        Dependency = JobHandle.CombineDependencies(job, new EventQueueJob {BulletInfos = bulletEvents}.Schedule(job));

        endSimulationEntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
    }

    private static RaycastInput GetRayCastInput(CollisionFilter filter, float3 bulletPosition,
        float3 previousBulletPosition, float3 offset)
    {
        return new RaycastInput
        {
            Start = previousBulletPosition + offset,
            End = bulletPosition + offset,
            Filter = filter
        };
    }

    struct EventQueueJob : IJob
    {
        public NativeQueue<BulletInfo> BulletInfos;

        public void Execute()
        {
            while (BulletInfos.TryDequeue(out BulletInfo info)) EventsHolder.BulletsEvents.Add(info);
        }
    }
}