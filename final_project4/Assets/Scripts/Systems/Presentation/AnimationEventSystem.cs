using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using AnimationInfo = EventStruct.AnimationInfo;
using Random = System.Random;
using Type = Enums.Type;

[DisableAutoCreation]
[UpdateAfter(typeof(StateEventSystem))]
public class AnimationEventSystem : SystemBase
{
    private EntityManager entityManager;

    public Dictionary<Type, HashSet<State>> UnHandledStates = new Dictionary<Type, HashSet<State>>();

    private Random rnd; 
    
    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        rnd = new Random(1234);
        
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
                    SwapAnimation(ref info);
                    break;
                case AnimationInfo.EventType.OnAnimationStart:
                    break;
                case AnimationInfo.EventType.OnAnimationEnd:
                    OnAnimationEnd(ref info);
                    break;
            }
        }
    }

    private void SwapAnimation(ref AnimationInfo info)
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
        entityManager.SetSharedComponentData(info.Entity, AnimationHolder.Animations[type.Value][info.NewState].Frames[animation.MeshIndexAt]);

        //Set new Refresh Group
        entityManager.SetComponentData(info.Entity, animation);
        entityManager.SetSharedComponentData(info.Entity, new AnimationBatch
        {
            BatchId = AnimationSystem.BatchIdToUpdate
        });
    }

    private void OnAnimationEnd(ref AnimationInfo info)
    {
        //Look if end of state Dying
        if (info.NewState != State.Dying)
            return;
        
        if (info.Entity == GameVariables.Player.Entity)
        {
            //GlobalEvents.PlayerEvents.OnPlayerDie();
            return;
        }

        if (rnd.Next(20) == 10)
        {
            var trans = entityManager.GetComponentData<Translation>(info.Entity);
            
            //Player only has a pistol
            if (GameVariables.Player.PlayerCurrentWeapons.Count <= 1) return;
            
            int index = rnd.Next(1, GameVariables.Player.PlayerCurrentWeapons.Count);
            WeaponType weaponType = GameVariables.Player.PlayerCurrentWeapons[index];
            DropSystem.DropAmmunition(entityManager, trans.Value, weaponType);
        }
        
        //Destroy entity
        entityManager.DestroyEntity(info.Entity);
    }
}