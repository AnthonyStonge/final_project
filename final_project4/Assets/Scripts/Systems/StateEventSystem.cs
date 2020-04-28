using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enums;
using EventStruct;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class StateEventSystem : SystemBase
{
    private EntityManager entityManager;

    protected override void OnCreate()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    protected override void OnUpdate()
    {
        Debug.Log("On StateEvent Update");

        //Should work, but might be slow because lots of foreach lolololololololol
        Entities.WithoutBurst().ForEach((Entity e, ref StateComponent component) =>
        {
            NativeList<StateInfo> events = new NativeList<StateInfo>(Allocator.Temp);

            //Retrieve all event corresponding to this entity
            foreach (StateInfo info in EventsHolder.StateEvents)
            {
                if (info.Entity == e)
                    events.Add(info);
            }

            //Act on each events

            //Look for an unlock event
            foreach (StateInfo info in events)
            {
                if (info.Action == StateInfo.ActionType.Unlock)
                {
                    //Unlock state machine
                    component.StateLocked = false;

                    //Set State to DesiredOne
                    component.CurrentState = component.DesiredState;
                }
            }

            //Might have multiple TryChange Events.
            NativeList<State> statesDesired = new NativeList<State>(Allocator.Temp);

            //Retrieve all states desired
            foreach (StateInfo info in events)
            {
                if (info.Action == StateInfo.ActionType.TryChange ||
                    info.Action == StateInfo.ActionType.TryChangeAndLock)
                    statesDesired.Add(info.DesiredState);
            }

            //Must make sure to treat State in least to most important order
            if (statesDesired.Length <= 0)
                return;

            if (statesDesired.Length == 1)
            {
                TryChangeState(ref component, statesDesired[0]);
                return;
            }

            //Choose most important state to change too
            //TODO IMPLEMENT ORDER IN STATES
            State stateToChangeTo = statesDesired[0];

            //Change state
            TryChangeState(ref component, stateToChangeTo);

            //Look if changing to this state implicates to lock the StateMachine
            foreach (StateInfo info in events)
            {
                if (info.DesiredState == stateToChangeTo)
                    if (info.Action == StateInfo.ActionType.TryChangeAndLock)
                        component.StateLocked = true;
            }

            events.Dispose();
        }).ScheduleParallel();
    }

    private static void TryChangeState(ref StateComponent component, State desiredState)
    {
        if (!component.StateLocked)
        {
            Debug.Log("Changing state to: " + desiredState);
            component.CurrentState = desiredState;
        }

        component.DesiredState = desiredState;
    }
}