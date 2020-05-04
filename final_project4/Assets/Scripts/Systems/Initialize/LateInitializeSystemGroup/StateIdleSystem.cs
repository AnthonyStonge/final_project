using Enums;
using EventStruct;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

[DisableAutoCreation]
public class StateIdleSystem : SystemBase
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

        //Set all entities with State to Idle
        JobHandle job = Entities.WithAll<StateComponent>().ForEach((Entity e) =>
        {
            //Reset all states to Idle until proven opposite
            events.Enqueue(new StateInfo
            {
                Entity = e,
                DesiredState = State.Idle,
                Action = StateInfo.ActionType.TryChange
            });
        }).ScheduleParallel(Dependency);
        job.Complete();

        while (stateEvents.TryDequeue(out StateInfo info))
        {
            EventsHolder.StateEvents.Add(info);
        }
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
