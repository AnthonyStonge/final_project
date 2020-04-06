using System;
using Unity.Entities;

[Serializable]
public struct StateData : IComponentData
{
    public FellowActions Value;
}

//TODO CHANGE THAT POOR NAME LOL
public enum FellowActions
{
    IDLE,
    MOVING,
    ATTACKING,
    DYING,
    DASHING
}
