using Enums;
using Havok.Physics;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

[DisableAutoCreation]
public class LootSystem : SystemBase
{
    private GunComponent gunComponent;
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        HavokTriggerEvents triggerEvents = ((HavokSimulation) stepPhysicsWorld.Simulation).TriggerEvents;

        var amunitionComponents = GetComponentDataFromEntity<AmmunitionComponent>(true);
        NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);

        foreach (var triggerEvent in triggerEvents)
        {
            if (amunitionComponents.HasComponent(triggerEvent.Entities.EntityA))
            {
                if (!entities.Contains(triggerEvent.Entities.EntityA))
                    entities.Add(triggerEvent.Entities.EntityA);
            }

            if (amunitionComponents.HasComponent(triggerEvent.Entities.EntityB))
            {
                if (!entities.Contains(triggerEvent.Entities.EntityB))
                    entities.Add(triggerEvent.Entities.EntityB);
            }
        }

        foreach (var entity in entities)
        {
            AmmunitionComponent ac = EntityManager.GetComponentData<AmmunitionComponent>(entity);
            EntityManager.DestroyEntity(entity);
            if (GameVariables.Player.PlayerWeaponEntities.ContainsKey(GameVariables.Player.CurrentWeaponHeld))
            {
                gunComponent =
                    EntityManager.GetComponentData<GunComponent>(
                        GameVariables.Player.PlayerWeaponEntities[ac.TypeAmmunition]);
                gunComponent.CurrentAmountBulletOnPlayer += ac.AmmunitionQuantity;
                EntityManager.SetComponentData(GameVariables.Player.PlayerWeaponEntities[ac.TypeAmmunition],
                    gunComponent);
                
                SoundEventSystem.PlayPickupSound(DropType.AmmunitionShotgun);
                //TODO Play VFX
            }
        }

        entities.Dispose();
    }
}