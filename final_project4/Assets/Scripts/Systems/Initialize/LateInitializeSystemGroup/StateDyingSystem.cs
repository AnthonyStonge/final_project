using Enums;
using EventStruct;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

[DisableAutoCreation]
[UpdateAfter(typeof(StateAttackingSystem))]
[UpdateAfter(typeof(UpdatePlayerStateSystem))]
public class StateDyingSystem : SystemBase
{
    private NativeQueue<StateInfo> stateEvents;
    private EndInitializationEntityCommandBufferSystem entityCommandBuffer;
    
    protected override void OnCreate()
    {
        stateEvents = new NativeQueue<StateInfo>(Allocator.Persistent);
        entityCommandBuffer = World.GetExistingSystem<EndInitializationEntityCommandBufferSystem>();
    }

    protected override void OnDestroy()
    {
        stateEvents.Dispose();
    }

    protected override void OnUpdate()
    {
        //Create parallel writer
        NativeQueue<StateInfo>.ParallelWriter events = stateEvents.AsParallelWriter();
        
        ////Act on all entities with HealthData.
        JobHandle job = Entities.WithAll<StateComponent>().ForEach((Entity e, ref LifeComponent health) =>
        {
            //If health <= 0 -> set state to dying
            if (health.IsDead())
                events.Enqueue(new StateInfo
                {
                    Entity = e,
                    DesiredState = State.Dying,
                    Action = StateInfo.ActionType.TryChangeAndLock
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