using Enums;
using EventStruct;
using Unity.Collections;
using Unity.Entities;
using AnimationInfo = EventStruct.AnimationInfo;

public struct DynamicAnimator : IBufferElementData
{
    public State State;
}

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
        NativeQueue<AnimationInfo>.ParallelWriter animationEvents = EventsHolder.AnimationEvents.AsParallelWriter();

        ComponentDataContainer<AnimationData> animatedEntities = new ComponentDataContainer<AnimationData>
        {
            Components = GetComponentDataFromEntity<AnimationData>()
        };
        
        ReadOnlyList<StateInfo> stateEvents = new ReadOnlyList<StateInfo>()
        {
            List = EventsHolder.StateEvents
        };
        //Should work, but might be slow because lots of foreach lolololololololol
        Entities.ForEach((Entity e, DynamicBuffer<DynamicAnimator> dynamicAnimator, ref StateComponent component, in TypeData type) =>
        {
            NativeList<StateInfo> events = new NativeList<StateInfo>(Allocator.Temp);

            //Retrieve all event corresponding to this entity
            for (var index = 0; index < stateEvents.List.Length; index++)
            {
                var info = stateEvents.List[index];
                if (info.Entity == e)
                    events.Add(info);
            }
            //Return if no events with this entity
            if (events.Length <= 0)
                return;

            // bool stateChanged = false;

            //Look for an unlock event
            for (var index = 0; index < events.Length; index++)
            {
                if (events[index].Action == StateInfo.ActionType.Unlock)
                {
                    UnLock(ref component);
                    // stateChanged = true;
                }
            }

            //Retrieve all states desired and choose the most important state
            State stateToChangeTo = 0;
            State animationStateToChangeTo = 0;
            bool shouldStateMachineLock = false;
            bool tryChangeEvent = false;
            
            for (var index = 0; index < events.Length; index++)
            {
                var info = events[index];
                if (info.Action == StateInfo.ActionType.TryChange)
                {
                    tryChangeEvent = true;
                    if (info.DesiredState > stateToChangeTo)
                    {
                        stateToChangeTo = info.DesiredState;
                        shouldStateMachineLock = false;
                    }
                    if (info.DesiredState > animationStateToChangeTo)
                       if (Contains(ref dynamicAnimator, info.DesiredState))
                                animationStateToChangeTo = info.DesiredState;
                }
                else if (info.Action == StateInfo.ActionType.TryChangeAndLock)
                {
                    tryChangeEvent = true;
                    if (info.DesiredState > stateToChangeTo)
                    {
                        stateToChangeTo = info.DesiredState;
                        shouldStateMachineLock = true;
                    }
                    if (info.DesiredState > animationStateToChangeTo)
                        if (Contains(ref dynamicAnimator, info.DesiredState))
                                animationStateToChangeTo = info.DesiredState;
                }
            }
            //Change state
            if (tryChangeEvent)
            {
                if (component.CurrentState != stateToChangeTo)
                    TryChangeState(ref component, stateToChangeTo, shouldStateMachineLock);
            }

            //Create animation event (only if state changed)
            if (animatedEntities.Components.HasComponent(e))
            {
                //Make sure animation desired isnt already playing
                if (component.CurrentAnimationState != animationStateToChangeTo)
                {
                    component.CurrentAnimationState = animationStateToChangeTo;
                    
                    animationEvents.Enqueue(new AnimationInfo
                    {
                        Entity = e,
                        Type = AnimationInfo.EventType.OnSwapAnimation,
                        NewState = animationStateToChangeTo
                    });
                }
            }
            events.Dispose();
            
        }).ScheduleParallel(Dependency).Complete();

    }

    private static bool Contains(ref DynamicBuffer<DynamicAnimator> dynamicAnimators, State state)
    {
        for (int i = 0; i < dynamicAnimators.Length; i++)
        {
            if (dynamicAnimators[i].State == state) return true;
        }
        return false;
    }
    private static void TryChangeState(ref StateComponent component, State desiredState, bool shouldLock)
    {
        // bool stateChanged = false;
        if (!component.StateLocked)
        {
            //Debug.Log("Changing state to: " + desiredState);
            ChangeState(ref component, desiredState, shouldLock);
        }

        //Always set desired values
        component.DesiredState = desiredState;
        component.ShouldStateBeLocked = shouldLock;

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