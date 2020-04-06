using System;
using Unity.Entities;

[Serializable]
public struct TimeTrackerComponent : IComponentData
{
    public float Current;
    public float ResetValue;
}
