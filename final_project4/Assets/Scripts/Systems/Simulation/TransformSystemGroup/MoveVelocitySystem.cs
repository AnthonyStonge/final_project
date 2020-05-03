using Enums;
using EventStruct;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Physics;
using float2 = Unity.Mathematics.float2;

[DisableAutoCreation]
public class MoveVelocitySystem : SystemBase
{
    private EntityCommandBufferSystem entityCommandBuffer;
    private NativeQueue<StateInfo> stateEvents;

    protected override void OnCreate()
    {
        entityCommandBuffer = World.GetExistingSystem<EndInitializationEntityCommandBufferSystem>();
        stateEvents = new NativeQueue<StateInfo>(Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        stateEvents.Dispose();
    }

    protected override void OnUpdate()
    {
        //Create parallel writer
        NativeQueue<StateInfo>.ParallelWriter events = stateEvents.AsParallelWriter();

        float dt = Time.DeltaTime;

        JobHandle job = Entities.ForEach((Entity e, ref PhysicsVelocity velocity, in DirectionData direction, in SpeedData speed) =>
        {
            velocity.Linear.xz = direction.Value * speed.Value * dt;
            velocity.Linear.y = 0;
            velocity.Angular.xz = 0;
            
            //If inputs to move, change state
            
            if (!direction.Value.Equals(float2.zero))
            {
                events.Enqueue(new StateInfo
                {
                    Entity = e,
                    Action = StateInfo.ActionType.TryChange,
                    DesiredState = State.Running
                });
            }
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