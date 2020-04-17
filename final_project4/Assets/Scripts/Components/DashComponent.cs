using System;
using Unity.Entities;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct DashComponent : IComponentData
{
    [Header("Variables")]
    public float Distance;
    public float ResetDashTimer;

    [HideInInspector] public float DashTimer;
}
