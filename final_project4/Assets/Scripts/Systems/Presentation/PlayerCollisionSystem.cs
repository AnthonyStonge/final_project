using Havok.Physics;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
[DisableAutoCreation]
public class PlayerCollisionSystem : SystemBase
{
    private StepPhysicsWorld stepPhysicsWorld;
    private EntityQuery entityQuery;
    
    protected override void OnCreate()
    {
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }
    
    protected override void OnUpdate()
    {
        HavokCollisionEvents collisionEvents = ((HavokSimulation) stepPhysicsWorld.Simulation).CollisionEvents;
        
        var player = GetComponentDataFromEntity<PlayerTag>(true);
        var enemy = GetComponentDataFromEntity<EnemyTag>(true);
        
        foreach (var collisionEvent in collisionEvents)
        {
            
            bool isHit = false;
            if(player.HasComponent(collisionEvent.Entities.EntityA))
            {
                if (enemy.HasComponent(collisionEvent.Entities.EntityB))
                {
                    isHit = true;
                }
            }
            if(player.HasComponent(collisionEvent.Entities.EntityB))
            {
                if (enemy.HasComponent(collisionEvent.Entities.EntityA))
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
