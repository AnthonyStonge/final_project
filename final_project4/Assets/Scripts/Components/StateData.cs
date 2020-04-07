using System;
using Unity.Entities;

[Serializable]
public struct StateData : IComponentData
{
    public StateActions Value;
}

//TODO CHANGE THAT POOR NAME LOL
public enum StateActions
{
    IDLE,
    MOVING,
    ATTACKING,
    DYING,
    DASHING
}
