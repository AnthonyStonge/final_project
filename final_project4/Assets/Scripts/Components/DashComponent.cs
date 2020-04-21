using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct DashComponent : IComponentData
{
    public float Speed;
    
    [ReadOnly] public float DashTime;
    [ReadOnly] public float CooldownTime;

    [HideInInspector]
    public float CurrentCooldownTime;
    [HideInInspector]
    public float CurrentDashTime;
    [HideInInspector]
    public float2 InputDuringDash;
    [HideInInspector]
    public quaternion TargetDuringDash;

}
