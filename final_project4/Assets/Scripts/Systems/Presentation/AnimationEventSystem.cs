using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

[DisableAutoCreation]
[UpdateAfter(typeof(StateEventSystem))]
public class AnimationEventSystem : SystemBase
{
    private EntityManager entityManager;

    public Dictionary<Type, HashSet<State>> UnHandledStates = new Dictionary<Type, HashSet<State>>();

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }


    protected override void OnUpdate()
    {
        //Cycle through all AnimationEvents and change there animation frame
        var s = EventsHolder.AnimationEvents;
        while (s.TryDequeue(out EventStruct.AnimationInfo info))
        {
            

        // foreach (EventStruct.AnimationInfo info in EventsHolder.AnimationEvents)
        // {
            //Get Type of Entity
            TypeData type = entityManager.GetComponentData<TypeData>(info.Entity);

            //TODO CAN REMOVE UNDER IN THEORIE
            //Make sure animation exists for this type/state
            if (!AnimationHolder.Animations.ContainsKey(type.Value) ||
                !AnimationHolder.Animations[type.Value].ContainsKey(info.NewState))
                continue;

            //Get AnimationComponent
            AnimationData animation = entityManager.GetComponentData<AnimationData>(info.Entity);

            //Set new frame
            animation.MeshIndexAt = 0;
            entityManager.SetSharedComponentData(info.Entity, new RenderMesh
            {
                mesh = AnimationHolder.Animations[type.Value][info.NewState].Frames[animation.MeshIndexAt],
                material = AnimationHolder.Animations[type.Value][info.NewState].Material
            });

            //Set new Refresh Group
            entityManager.SetComponentData(info.Entity, animation);
            entityManager.SetSharedComponentData(info.Entity, new AnimationBatch
            {
                BatchId = AnimationSystem.BatchIdToUpdate
            });
        }
    }

    // }
}