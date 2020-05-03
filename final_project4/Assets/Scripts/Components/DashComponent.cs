using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct DashComponent : IComponentData
{
    [HideInInspector] public float2 Direction;
    
    public float Speed;
    
    [ReadOnly] public float DashTime;
    [ReadOnly] public float CooldownTime;

    [HideInInspector] public float CurrentCooldownTime;
    [HideInInspector] public float CurrentDashTime;

    [HideInInspector] public bool WasDashingPreviousFrame;
    public bool IsAvailable => CurrentCooldownTime <= 0;
    public bool IsDashing => CurrentDashTime > 0;


    public void OnDash()
    {
        //Reset timers
        CurrentCooldownTime = CooldownTime;
        CurrentDashTime = DashTime;
        WasDashingPreviousFrame = true;
    }

}
