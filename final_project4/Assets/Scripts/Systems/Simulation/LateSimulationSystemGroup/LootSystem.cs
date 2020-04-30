using System.Collections;
using System.Collections.Generic;
using Havok.Physics;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;
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
        
        var amunitionComponents = GetComponentDataFromEntity<AmunationComponent>(true);
        
        NativeList<Entity> entities = new NativeList<Entity>(Allocator.Temp);
        
        foreach (var triggerEvent in triggerEvents)
        {
            if(amunitionComponents.HasComponent(triggerEvent.Entities.EntityA))
            {
                if(!entities.Contains(triggerEvent.Entities.EntityA))
                    entities.Add(triggerEvent.Entities.EntityA);
            }
            if(amunitionComponents.HasComponent(triggerEvent.Entities.EntityB))
            {
                if(!entities.Contains(triggerEvent.Entities.EntityB))
                    entities.Add(triggerEvent.Entities.EntityB);
            }
        }
        
        foreach (var entity in entities)
        {
            AmunationComponent ac = EntityManager.GetComponentData<AmunationComponent>(entity);
            EntityManager.DestroyEntity(entity);
            if (GameVariables.Player.PlayerWeaponEntities.ContainsKey(GameVariables.Player.CurrentWeaponHeld))
            {
                gunComponent = EntityManager.GetComponentData<GunComponent>(GameVariables.Player.PlayerWeaponEntities[ac.TypeAmunation]);
                gunComponent.CurrentAmountBulletOnPlayer += ac.AmunationQuantity;
                EntityManager.SetComponentData(GameVariables.Player.PlayerWeaponEntities[ac.TypeAmunation], gunComponent);
            }
        }
        entities.Dispose();
    }
}

