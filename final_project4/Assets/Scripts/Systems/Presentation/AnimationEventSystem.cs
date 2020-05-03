using System;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
using AnimationInfo = EventStruct.AnimationInfo;
using Type = Enums.Type;

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
            switch (info.Type)
            {
                case AnimationInfo.EventType.OnSwapAnimation:
                    SwapAnimation(info);
                    break;
                case AnimationInfo.EventType.OnAnimationStart:
                    break;
                case AnimationInfo.EventType.OnAnimationEnd:
                    OnAnimationEnd(info);
                    break;
            }
        }
    }

    private void SwapAnimation(AnimationInfo info)
    {
        //Get Type of Entity
        TypeData type = entityManager.GetComponentData<TypeData>(info.Entity);

        //TODO CAN REMOVE UNDER IN THEORIE
        //Make sure animation exists for this type/state
        if (!AnimationHolder.Animations.ContainsKey(type.Value) ||
            !AnimationHolder.Animations[type.Value].ContainsKey(info.NewState))
            return;

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

    private void OnAnimationEnd(AnimationInfo info)
    {
        //Look if end of state Dying
        if (info.NewState != State.Dying)
            return;

        if (info.Entity == GameVariables.Player.Entity)
        {
            GlobalEvents.PlayerEvents.OnPlayerDie();
            return;
        }
        
        //Destroy entity
        entityManager.DestroyEntity(info.Entity);
    }
}