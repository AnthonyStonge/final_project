using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct StateData : IComponentData
{
    public StateActions Value;
}

public enum StateActions
{
    IDLE,
    MOVING,
    ATTACKING,
    RELOADING,
    DYING,
    DASHING
}
