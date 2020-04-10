using System.Collections.Generic;
using Havok.Physics;
using Unity.Entities;
using Unity.Physics.Systems;

[UpdateAfter(typeof(EndFramePhysicsSystem))]
public class CollisionTest : SystemBase
{
    private EntityManager entityManager;
    
    private StepPhysicsWorld stepPhysicsWorld;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    protected override void OnUpdate()
    {
        //Debug.Log("On Collision Test Updated");

        //Debug.Log("Simulation Type: " + stepPhysicsWorld.Simulation.GetType());
        var events = ((HavokSimulation) stepPhysicsWorld.Simulation).TriggerEvents;

        var getter = GetComponentDataFromEntity<BulletTag>();
        
        Stack<Entity> toDelete = new Stack<Entity>();

        foreach (var i in events)
        {
            //Debug.Log("Collision between " + i.Entities.EntityA + " and " + i.Entities.EntityB);
            if (getter.Exists(i.Entities.EntityB))
            {
                //Debug.Log("Deleting entity... ID: " + i.Entities.EntityB);
                toDelete.Push(i.Entities.EntityB);
            }
        }

        foreach (Entity entity in toDelete)
        {
            entityManager.DestroyEntity(entity);
        }
        
        toDelete.Clear();
    }
}