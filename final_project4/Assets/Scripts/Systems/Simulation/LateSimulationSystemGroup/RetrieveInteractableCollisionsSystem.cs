using System.Collections;
using System.Collections.Generic;
using EventStruct;
using Havok.Physics;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;
using Unity.Transforms;
using UnityEngine;


[DisableAutoCreation]
public class RetrieveInteractableCollisionsSystem : SystemBase
{
    private StepPhysicsWorld stepPhysicsWorld;
    public HashSet<Entity> PreviousFrameCollisions = new HashSet<Entity>();

    protected override void OnCreate()
    {
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }

    //TODO FIGURE OUT WHY ON EXIT IS CALLED EVEN WHEN NOT EXITED...
    //Will be slow if lots of TriggerEvent (Change to parallel)
    protected override void OnUpdate()
    {
        //Get Collision Events
        HavokTriggerEvents triggerEvents = ((HavokSimulation) stepPhysicsWorld.Simulation).TriggerEvents;

        //Get all Interactable Entities
        ComponentDataFromEntity<InteractableComponent> interactables =
            GetComponentDataFromEntity<InteractableComponent>(true);

        ComponentDataFromEntity<Translation> interactablePositions =
            GetComponentDataFromEntity<Translation>(true);

        ComponentDataFromEntity<Rotation> interactableRotations =
            GetComponentDataFromEntity<Rotation>(true);

        HashSet<Entity> currentFrameCollisions = new HashSet<Entity>();

        //Retrieve all interactables collision with PlayerEntity
        foreach (TriggerEvent triggerEvent in triggerEvents)
        {
            if (interactables.Exists(triggerEvent.Entities.EntityA))
            {
                currentFrameCollisions.Add(triggerEvent.Entities.EntityA);
            }
            else if (interactables.Exists(triggerEvent.Entities.EntityB))
            {
                currentFrameCollisions.Add(triggerEvent.Entities.EntityB);
            }
        }

        foreach (Entity entity in currentFrameCollisions)
        {
            //if interactable was not already colliding previous frame -> OnTriggerEnter
            if (!PreviousFrameCollisions.Contains(entity))
            {
                //Create Event
                EventsHolder.InteractableEvents.Add(new InteractableInfo
                {
                    Entity = entity,
                    Position = interactablePositions[entity].Value,
                    Rotation = interactableRotations[entity].Value,
                    InteractableType = interactables[entity].Type,
                    ObjectType = interactables[entity].ObjectType,
                    CollisionType = InteractableInfo.InteractableCollisionType.OnTriggerEnter
                });
            }
        }

        foreach (Entity entity in PreviousFrameCollisions)
        {
            if (EntityManager.Exists(entity))
                //if previous collision not detected this frame -> OnTriggerExit
                if (!currentFrameCollisions.Contains(entity))
                {
                    //Create Event
                    EventsHolder.InteractableEvents.Add(new InteractableInfo
                    {
                        Entity = entity,
                        Position = interactablePositions[entity].Value,
                        Rotation = interactableRotations[entity].Value,
                        InteractableType = interactables[entity].Type,
                        ObjectType = interactables[entity].ObjectType,
                        CollisionType = InteractableInfo.InteractableCollisionType.OnTriggerExit
                    });
                }
        }

        PreviousFrameCollisions = currentFrameCollisions;
    }
}