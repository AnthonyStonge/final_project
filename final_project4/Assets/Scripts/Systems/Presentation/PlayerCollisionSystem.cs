using Enums;
using Havok.Physics;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

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

        var playerEntity = GameVariables.Player.Entity;
        Entity entity = Entity.Null;
        bool isHit = false;
        Job.WithCode(() =>
        {
            
            foreach (var collisionEvent in collisionEvents)
            {
                if (player.HasComponent(collisionEvent.Entities.EntityA))
                {
                    if (enemy.HasComponent(collisionEvent.Entities.EntityB))
                    {
                        entity = collisionEvent.Entities.EntityB;
                        isHit = true;
                    }
                }

                if (player.HasComponent(collisionEvent.Entities.EntityB))
                {
                    if (enemy.HasComponent(collisionEvent.Entities.EntityA))
                    {
                        entity = collisionEvent.Entities.EntityA;
                        isHit = true;
                    }
                }
            }
        }).Run();
        if (isHit)
        {
            LifeComponent lifeComponent = EntityManager.GetComponentData<LifeComponent>(playerEntity);
            bool lifeDecremented = lifeComponent.DecrementLifeWithInvincibility();
            LifeComponent lifeComponentRat = EntityManager.GetComponentData<LifeComponent>(entity);
            if (EntityManager.GetComponentData<TypeData>(entity).Value == Type.Rat)
            {
                lifeComponentRat.DecrementLife();
                EntityManager.SetComponentData(entity, lifeComponentRat);
            }
            if (lifeComponent.DecrementLifeWithInvincibility())
                UIManager.OnPlayerHit();
            EntityManager.SetComponentData(playerEntity, lifeComponent);
            if(lifeDecremented)
                UIManager.OnPlayerHit();
        }
    }
}