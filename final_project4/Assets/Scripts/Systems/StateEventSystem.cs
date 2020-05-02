using Enums;
using EventStruct;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using AnimationInfo = EventStruct.AnimationInfo;

[DisableAutoCreation]
[UpdateBefore(typeof(AnimationEventSystem))]
public class StateEventSystem : SystemBase
{
    private NativeQueue<AnimationInfo> statesChanged;

    protected override void OnCreate()
    {
        statesChanged = new NativeQueue<AnimationInfo>(Allocator.Persistent);
    }

    protected override void OnDestroy()
    {
        statesChanged.Dispose();
    }

    protected override void OnUpdate()
    {
        //Debug.Log("On StateEvent Update");

        //Create parallel writer
        NativeQueue<AnimationInfo>.ParallelWriter animationEvents = statesChanged.AsParallelWriter();

        ComponentDataContainer<AnimationData> animatedEntities = new ComponentDataContainer<AnimationData>
        {
            Components = GetComponentDataFromEntity<AnimationData>()
        };

        //Should work, but might be slow because lots of foreach lolololololololol
        JobHandle job = Entities.WithoutBurst().ForEach((Entity e, ref StateComponent component) =>
        {
            NativeList<StateInfo> events = new NativeList<StateInfo>(Allocator.Temp);

            //Retrieve all event corresponding to this entity
            foreach (StateInfo info in EventsHolder.StateEvents)
            {
                if (info.Entity == e)
                    events.Add(info);
            }

            //Return if no events with this entity
            if (events.Length <= 0)
                return;

            bool stateChanged = false;

            //Look for an unlock event
            foreach (StateInfo info in events)
            {
                if (info.Action == StateInfo.ActionType.Unlock)
                {
                    if (UnLock(ref component))
                        stateChanged = true;
                }
            }

            //Retrieve all states desired and choose the most important state
            State stateToChangeTo = 0;
            bool shouldStateMachineLock = false;
            bool tryChangeEvent = false;

            foreach (StateInfo info in events)
            {
                if (info.Action == StateInfo.ActionType.TryChange)
                {
                    tryChangeEvent = true;

                    if (info.DesiredState > stateToChangeTo)
                    {
                        stateToChangeTo = info.DesiredState;
                        shouldStateMachineLock = false;
                    }
                }
                else if (info.Action == StateInfo.ActionType.TryChangeAndLock)
                {
                    tryChangeEvent = true;

                    if (info.DesiredState > stateToChangeTo)
                    {
                        stateToChangeTo = info.DesiredState;
                        shouldStateMachineLock = true;
                    }
                }
            }

            //Change state
            if (tryChangeEvent)
            {
                if (component.CurrentState != stateToChangeTo)
                    stateChanged = TryChangeState(ref component, stateToChangeTo, shouldStateMachineLock);
            }

            //Create animation event (only if state changed)
            if (stateChanged && animatedEntities.Components.HasComponent(e))
            {
                animationEvents.Enqueue(new AnimationInfo
                {
                    Entity = e,
                    NewState = component.CurrentState
                });
            }

            events.Dispose();
        }).ScheduleParallel(Dependency);

        job.Complete();

        //Empty queue in here because AnimationEvent come right after this system
        while (statesChanged.TryDequeue(out AnimationInfo info))
        {
            EventsHolder.AnimationEvents.Add(info);
        }
    }

    private static bool TryChangeState(ref StateComponent component, State desiredState, bool shouldLock)
    {
        bool stateChanged = false;
        if (!component.StateLocked)
        {
            //Debug.Log("Changing state to: " + desiredState);
            stateChanged = true;
            ChangeState(ref component, desiredState, shouldLock);
        }

        //Always set desired values
        component.DesiredState = desiredState;
        component.ShouldStateBeLocked = shouldLock;

        return stateChanged;
    }

    private static bool UnLock(ref StateComponent component)
    {
        //Unlock state machine
        component.StateLocked = false;

        //Set State to DesiredOne
        if (component.CurrentState == component.DesiredState)
            return false;

        //Change state (if not the same)
        ChangeState(ref component, component.DesiredState, component.ShouldStateBeLocked);
        return true;
    }

    private static void ChangeState(ref StateComponent component, State state, bool shouldLock)
    {
        component.CurrentState = state;
        component.StateLocked = shouldLock;
    }
}