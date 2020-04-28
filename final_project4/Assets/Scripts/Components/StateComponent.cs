using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct StateComponent : IComponentData
{
    public bool StateLocked;
    
    public State CurrentState;
    [HideInInspector] public State DesiredState;
    [HideInInspector] public bool ShouldStateBeLocked;
}
