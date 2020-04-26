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
[UpdateBefore(typeof(EndFramePhysicsSystem))]
[UpdateAfter(typeof(StepPhysicsWorld))]
[UpdateBefore(typeof(ProjectileHitDetectionSystem))]
public class LootSystem : SystemBase
{
    private GunComponent gunComponent;
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;
    private EndSimulationEntityCommandBufferSystem endSimulationEntityCommandBufferSystem;
    private EntityQuery entityQuery;
    
    protected override void OnCreate()
    {
        endSimulationEntityCommandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        EntityQueryDesc eqd = new EntityQueryDesc
        {
            All = new ComponentType[]
            {
                typeof(AmunationComponent)
            }
        };
        entityQuery = GetEntityQuery(eqd);
    }
    protected override void OnUpdate()
    {
        HavokTriggerEvents bob = ((HavokSimulation) stepPhysicsWorld.Simulation).TriggerEvents;
        ComponentDataContainer<AmunationComponent> lol = new ComponentDataContainer<AmunationComponent>
        {
            Components = GetComponentDataFromEntity<AmunationComponent>()
        };
        NativeList<Entity> tmp = new NativeList<Entity>(Allocator.Temp);
        
        foreach (var VARIABLE in bob)
        {
            if(lol.Components.HasComponent(VARIABLE.Entities.EntityA))
            {
                if(!tmp.Contains(VARIABLE.Entities.EntityA))
                    tmp.Add(VARIABLE.Entities.EntityA);
                break;
            }
            if(lol.Components.HasComponent(VARIABLE.Entities.EntityB))
            {
                if(!tmp.Contains(VARIABLE.Entities.EntityB))
                    tmp.Add(VARIABLE.Entities.EntityB);
                break;
            }
        }
        
        foreach (var VARIABLE in tmp)
        {
            AmunationComponent ac = EntityManager.GetComponentData<AmunationComponent>(VARIABLE);
            EntityManager.DestroyEntity(VARIABLE);
            if (GameVariables.Player.PlayerWeaponEntities.ContainsKey(GameVariables.Player.CurrentWeaponHeld))
            {
                gunComponent = EntityManager.GetComponentData<GunComponent>(GameVariables.Player.PlayerWeaponEntities[ac.TypeAmunation]);
                gunComponent.CurrentAmountBulletOnPlayer += ac.AmunationQuantity;
                EntityManager.SetComponentData(GameVariables.Player.PlayerWeaponEntities[ac.TypeAmunation], gunComponent);
            }
        }
        tmp.Dispose();
    }
}

