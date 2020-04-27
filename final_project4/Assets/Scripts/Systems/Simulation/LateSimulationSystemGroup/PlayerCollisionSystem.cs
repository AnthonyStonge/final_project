using Havok.Physics;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
[DisableAutoCreation]
[UpdateBefore(typeof(EndFramePhysicsSystem))]
[UpdateAfter(typeof(StepPhysicsWorld))]
[UpdateBefore(typeof(ProjectileHitDetectionSystem))]
public class PlayerCollisionSystem : SystemBase
{
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;
    private EntityQuery entityQuery;
    
    protected override void OnCreate()
    {
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }
    
    protected override void OnUpdate()
    {
        HavokCollisionEvents bob = ((HavokSimulation) stepPhysicsWorld.Simulation).CollisionEvents;
        
        ComponentDataContainer<PlayerTag> player = new ComponentDataContainer<PlayerTag>
        {
            Components = GetComponentDataFromEntity<PlayerTag>()
        };
        ComponentDataContainer<EnemyTag> enemy = new ComponentDataContainer<EnemyTag>
        {
            Components = GetComponentDataFromEntity<EnemyTag>()
        };
        foreach (var VARIABLE in bob)
        {
            
            bool isHit = false;
            if(player.Components.HasComponent(VARIABLE.Entities.EntityA))
            {
                if (enemy.Components.HasComponent(VARIABLE.Entities.EntityB))
                {
                    isHit = true;
                }
            }
            if(player.Components.HasComponent(VARIABLE.Entities.EntityB))
            {
                if (enemy.Components.HasComponent(VARIABLE.Entities.EntityA))
                {
                    isHit = true;
                }
            }
            if (isHit)
            {
                LifeComponent lifeComponent = EntityManager.GetComponentData<LifeComponent>(GameVariables.Player.Entity);
                lifeComponent.CurrentLife -= 1;
                EntityManager.SetComponentData(GameVariables.Player.Entity, lifeComponent);
            }
            
        }
    }
}
