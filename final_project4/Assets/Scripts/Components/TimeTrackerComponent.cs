using System;
using Unity.Entities;

[Serializable]
public struct TimeTrackerComponent : IComponentData
{
    public float Current;
    public readonly float ResetValue;
    public bool Available => Current < 0;

    public void Reset()
    {
        this.Current = ResetValue;
    }
}