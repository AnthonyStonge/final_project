using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct ForwardData : IComponentData
{
    public float3 Value;
}
