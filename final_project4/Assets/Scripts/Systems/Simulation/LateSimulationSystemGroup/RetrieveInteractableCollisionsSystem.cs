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
    public HashSet<CollisionLink> PreviousFrameCollisions = new HashSet<CollisionLink>();

    protected override void OnCreate()
    {
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
    }
    
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

        HashSet<CollisionLink> currentFrameCollisions = new HashSet<CollisionLink>();

        //Retrieve all interactables collision with PlayerEntity
        foreach (TriggerEvent triggerEvent in triggerEvents)
        {
            if (interactables.Exists(triggerEvent.Entities.EntityA))
            {
                currentFrameCollisions.Add(new CollisionLink
                {
                    TriggerEntity = triggerEvent.Entities.EntityA,
                    NotTriggerEntity = triggerEvent.Entities.EntityB
                });
            }
            else if (interactables.Exists(triggerEvent.Entities.EntityB))
            {
                currentFrameCollisions.Add(new CollisionLink
                {
                    TriggerEntity = triggerEvent.Entities.EntityB,
                    NotTriggerEntity = triggerEvent.Entities.EntityA
                });
            }
        }

        foreach (CollisionLink link in currentFrameCollisions)
        {
            //if interactable was not already colliding previous frame -> OnTriggerEnter
            if (!PreviousFrameCollisions.Contains(link))
            {
                //Create Event
                EventsHolder.InteractableEvents.Add(new InteractableInfo
                {
                    TriggerEntity = link.TriggerEntity,
                    CollidedEntity = link.NotTriggerEntity,
                    Position = interactablePositions[link.TriggerEntity].Value,
                    Rotation = interactableRotations[link.TriggerEntity].Value,
                    InteractableType = interactables[link.TriggerEntity].Type,
                    ObjectType = interactables[link.TriggerEntity].ObjectType,
                    CollisionType = InteractableInfo.InteractableCollisionType.OnTriggerEnter
                });
            }
        }

        foreach (CollisionLink link in PreviousFrameCollisions)
        {
            if (EntityManager.Exists(link.TriggerEntity))
                //if previous collision not detected this frame -> OnTriggerExit
                if (!currentFrameCollisions.Contains(link))
                {
                    //Create Event
                    EventsHolder.InteractableEvents.Add(new InteractableInfo
                    {
                        TriggerEntity = link.TriggerEntity,
                        CollidedEntity = link.NotTriggerEntity,
                        Position = interactablePositions[link.TriggerEntity].Value,
                        Rotation = interactableRotations[link.TriggerEntity].Value,
                        InteractableType = interactables[link.TriggerEntity].Type,
                        ObjectType = interactables[link.TriggerEntity].ObjectType,
                        CollisionType = InteractableInfo.InteractableCollisionType.OnTriggerExit
                    });
                }
        }

        PreviousFrameCollisions = currentFrameCollisions;
    }

    public struct CollisionLink
    {
        public Entity TriggerEntity;
        public Entity NotTriggerEntity;
    }
}