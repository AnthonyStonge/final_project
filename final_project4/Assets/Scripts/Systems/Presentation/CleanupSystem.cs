using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class CleanupSystem : SystemBase
{
    protected override void OnUpdate()
    {
        EventsHolder.BulletsEvents.Clear();    //TODO DO SOMEWHERE ELSE
    }
}
