using System;
using Unity.Entities;

[Serializable]
public struct DashComponent : IComponentData
{
    public TimeTrackerComponent Timer;
    public float Distance;
}
