using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using Unity.Physics.Systems;
using UnityEngine;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class CollisionTest : SystemBase
{
    private EndSimulationEntityCommandBufferSystem endECB;
    
    private BuildPhysicsWorld buildPhysicsWorld;
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        this.endECB = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        this.buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        this.stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        Debug.Log("On Collision Test Updated");
        
        EntityCommandBuffer ecb = endECB.CreateCommandBuffer();

        BulletWallCollision job = new BulletWallCollision
        {
            ecb = ecb,
            bulletEntities = GetComponentDataFromEntity<BulletTag>(true),    //Retrieve only entities with bullet tags?
        };

        JobHandle lol = job.Schedule(this.stepPhysicsWorld.Simulation, ref this.buildPhysicsWorld.PhysicsWorld, Dependency);
        lol.Complete();

        endECB.AddJobHandleForProducer(lol);
    }
    
    public struct BulletWallCollision : ITriggerEventsJob
    {
        public EntityCommandBuffer ecb;
        [ReadOnly] public ComponentDataFromEntity<BulletTag> bulletEntities;

        //Delete bullet entity
        public void Execute(TriggerEvent triggerEvent)
        {
            Debug.Log("Collision");
            //Find which entity is the bullet
            /*if (bulletEntities.Exists(triggerEvent.Entities.EntityA))
            {
                Debug.Log("Entities A is the bullet");
                ecb.DestroyEntity(triggerEvent.Entities.EntityA);
            }*/
            if (bulletEntities.Exists(triggerEvent.Entities.EntityB))
            {
                Debug.Log("Entities B is the bullet");
                ecb.DestroyEntity(triggerEvent.Entities.EntityB);
            }
        }
    }
}
