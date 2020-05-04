using System;
using Enums;
using EventStruct;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Type = Enums.Type;

[DisableAutoCreation]
[AlwaysUpdateSystem]
[UpdateAfter(typeof(StateMovingSystem))]
public class StateAttackingSystem : SystemBase
{
    private EndInitializationEntityCommandBufferSystem entityCommandBuffer;
    private NativeQueue<StateInfo> stateEvents = new NativeQueue<StateInfo>(Allocator.Persistent);

    protected override void OnCreate()
    {
        entityCommandBuffer = World.GetExistingSystem<EndInitializationEntityCommandBufferSystem>();
    }

    protected override void OnDestroy()
    {
        stateEvents.Dispose();
    }

    protected override void OnUpdate()
    {
        //Player state
        Entity player = GameVariables.Player.Entity;
        InputComponent playerInputs = GetComponent<InputComponent>(player);
        GunComponent gun = GetComponent<GunComponent>(
            GameVariables.Player.PlayerWeaponEntities[GameVariables.Player.CurrentWeaponHeld]);

        //Make sure player can attack (Weapon not on cooldown)
        if (gun.SwapTimer <= 0 && playerInputs.Shoot)
        {
            //Add StateEvent
            EventsHolder.StateEvents.Add(new StateInfo
            {
                Entity = player,
                DesiredState = State.Attacking,
                Action = StateInfo.ActionType.TryChange
            });
        }

        //TODO REMOVE BECAUSE I THINK ITS ALREADY DONE SOMEWHERE ELSE...
        //TODO DONT RUN QUERY IF NO ENEMIES EXISTS
        //Create parallel writer
        NativeQueue<StateInfo>.ParallelWriter events = stateEvents.AsParallelWriter();
        
        Translation playerPos = GetComponent<Translation>(player);
        //Act on Enemies
        JobHandle job = Entities.WithAll<EnemyTag>().ForEach(
            (Entity e, in TypeData type, in StateComponent state, in Translation currentPosition, in AttackRangeComponent range) =>
            {
                //Is distance small enough to Attack
                if (math.distancesq(currentPosition.Value, playerPos.Value) > range.Distance * range.Distance)
                    return;

                StateInfo.ActionType actionType = StateInfo.ActionType.TryChange;
                
                switch (type.Value)
                {
                    case Type.Pig:
                        break;
                    case Type.Rat:
                        break;
                    case Type.Chicken:
                        break;
                    case Type.Gorilla:
                        actionType = StateInfo.ActionType.TryChangeAndLock;
                        break;
                }
                //Add StateEvent
                events.Enqueue(new StateInfo
                {
                    Entity = e,
                    DesiredState = State.Attacking,
                    Action = actionType
                });
            }).ScheduleParallel(Dependency);
        
        //Create job
        JobHandle emptyEventQueueJob = new EmptyEventQueueJob
        {
            EventsQueue = stateEvents
        }.Schedule(job);

        //Link all jobs
        Dependency = JobHandle.CombineDependencies(job, emptyEventQueueJob);
        entityCommandBuffer.AddJobHandleForProducer(Dependency);
    }
    
    struct EmptyEventQueueJob : IJob
    {
        public NativeQueue<StateInfo> EventsQueue;

        public void Execute()
        {
            while (EventsQueue.TryDequeue(out StateInfo info))
            {
                EventsHolder.StateEvents.Add(info);
            }
        }
    }
}