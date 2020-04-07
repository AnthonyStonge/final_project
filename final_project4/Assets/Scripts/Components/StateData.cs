using System;
using Unity.Entities;

[Serializable]
public struct StateData : IComponentData
{
    public StateActions Value;
}

public enum StateActions
{
    IDLE,
    MOVING,
    ATTACKING,
    DYING,
    DASHING
}
