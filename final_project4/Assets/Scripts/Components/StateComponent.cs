using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;

public struct StateComponent : IComponentData
{
    public bool StateLocked;
    
    public State CurrentState;
    public State DesiredState;
}
