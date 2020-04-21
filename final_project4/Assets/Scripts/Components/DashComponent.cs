using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct DashComponent : IComponentData
{
    [Header("Variables")]
    public float Distance;
    public float ResetDashTimer;
    public float ResetDashSkill;

    [HideInInspector]
    public float DashSkillTimer;
    [HideInInspector]
    public float DashTimer;
    [HideInInspector]
    public float2 InputDuringDash;
    [HideInInspector]
    public quaternion TargetDuringDash;

}
