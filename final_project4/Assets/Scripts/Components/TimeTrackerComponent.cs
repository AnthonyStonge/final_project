using System;
using Unity.Entities;

[Serializable]
public struct TimeTrackerComponent : IComponentData
{
    public float Current;
    public float ResetValue;
    public bool Available => Current <= 0;

    public TimeTrackerComponent(float resetValue)
    {
        ResetValue = resetValue;
        Current = ResetValue;
    }
    
    public void Reset()
    {
        Current = ResetValue;
    }
}