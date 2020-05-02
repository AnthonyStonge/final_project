using System;
using Enums;
using Unity.Entities;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct StateComponent : IComponentData
{
    public bool StateLocked;
    
    public State CurrentState;
    [HideInInspector] public State CurrentAnimationState;
    
    [HideInInspector] public State DesiredState;
    [HideInInspector] public bool ShouldStateBeLocked;
}
