using System.Collections;
using System.Collections.Generic;
using Enums;
using EventStruct;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class StateReloadingSystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entity player = GameVariables.Player.Entity;
        
        //Get Player Components
        InputComponent inputs = EntityManager.GetComponentData<InputComponent>(player);

        if(inputs.Reload)
            EventsHolder.StateEvents.Add(new StateInfo
            {
                Entity = player,
                DesiredState = State.Reloading,
                Action = StateInfo.ActionType.TryChange
            });
    }
}
