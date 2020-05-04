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
        bool isHit = false;
        Job.WithCode(() =>
        {
            foreach (var collisionEvent in collisionEvents)
            {
                if (player.HasComponent(collisionEvent.Entities.EntityA))
                {
                    if (enemy.HasComponent(collisionEvent.Entities.EntityB))
                    {
                        isHit = true;
                    }
                }
                if (player.HasComponent(collisionEvent.Entities.EntityB))
                {
                    if (enemy.HasComponent(collisionEvent.Entities.EntityA))
                    {
                        isHit = true;
                    }
                }
            }
        }).Run();
        if (isHit)
        {
            LifeComponent lifeComponent = EntityManager.GetComponentData<LifeComponent>(playerEntity);
            lifeComponent.DecrementLife();
            EntityManager.SetComponentData(playerEntity, lifeComponent);
        }
    }

}
