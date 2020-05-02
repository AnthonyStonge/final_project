using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[DisableAutoCreation]
public class GlobalEventListenerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        //Look for player hp
        if(EntityManager.GetComponentData<LifeComponent>(GameVariables.Player.Entity).Life.Value <= 0)
            GlobalEvents.PlayerEvents.OnPlayerDie();
    }
}
