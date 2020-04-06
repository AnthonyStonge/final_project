using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct TargetData : IComponentData
{
    public float3 Value;
}
