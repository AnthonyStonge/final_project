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

        foreach (EventStruct.AnimationInfo info in EventsHolder.AnimationEvents)
        {
            //Get Type of Entity
            TypeData type = entityManager.GetComponentData<TypeData>(info.Entity);
            
            //Make sure State is handled
            if(UnHandledStates.ContainsKey(type.Value))
                if (UnHandledStates[type.Value].Contains(info.NewState))
                {
                    //Debug.Log("Animation not handled... Staying on current animation");
                    continue;
                }
            
            //Get StateComponent
            StateComponent state = entityManager.GetComponentData<StateComponent>(info.Entity);
            
            //Get AnimationComponent
            AnimationData animation = entityManager.GetComponentData<AnimationData>(info.Entity);
            
            //Make sure animation exists for this type/state
            if (!AnimationHolder.Animations.ContainsKey(type.Value) ||
                !AnimationHolder.Animations[type.Value].ContainsKey(info.NewState))
                continue;
            
            //Set new AnimationState
            state.CurrentAnimationState = info.NewState;
            entityManager.SetComponentData(info.Entity, state);

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
}