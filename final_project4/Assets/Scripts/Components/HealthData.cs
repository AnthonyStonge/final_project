using System;
using Unity.Entities;

[Serializable]
public struct HealthData : IComponentData
{
    public short Value;
}