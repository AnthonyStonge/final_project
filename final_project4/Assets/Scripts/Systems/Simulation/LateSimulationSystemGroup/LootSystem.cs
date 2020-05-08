using Enums;
using Havok.Physics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
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
        NativeList<Entity> ammunitions = new NativeList<Entity>(Allocator.Temp);

        foreach (var triggerEvent in triggerEvents)
        {
            if (amunitionComponents.HasComponent(triggerEvent.Entities.EntityA))
            {
                if (!ammunitions.Contains(triggerEvent.Entities.EntityA))
                    ammunitions.Add(triggerEvent.Entities.EntityA);
            }

            if (amunitionComponents.HasComponent(triggerEvent.Entities.EntityB))
            {
                if (!ammunitions.Contains(triggerEvent.Entities.EntityB))
                    ammunitions.Add(triggerEvent.Entities.EntityB);
            }
            
        }

        foreach (var entity in ammunitions)
        {
            AmmunitionComponent ac = EntityManager.GetComponentData<AmmunitionComponent>(entity);
            EntityManager.DestroyEntity(entity);
            
            
            gunComponent = EntityManager.GetComponentData<GunComponent>(GameVariables.Player.PlayerWeaponEntities[ac.TypeAmmunition]);
            gunComponent.CurrentAmountBulletOnPlayer = math.clamp(gunComponent.CurrentAmountBulletOnPlayer + ac.AmmunitionQuantity, 0, gunComponent.MaxBulletOnPlayer);
            EntityManager.SetComponentData(GameVariables.Player.PlayerWeaponEntities[ac.TypeAmmunition],gunComponent);
            
            SoundEventSystem.PlayPickupSound(DropType.AmmunitionShotgun);
            
            UIManager.RefreshBulletsText();
            //TODO Play VFX
        }

        ammunitions.Dispose();
    }
}