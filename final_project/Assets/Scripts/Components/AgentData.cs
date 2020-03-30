using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;
using System.Runtime.InteropServices;

public struct AgentData : IComponentData
{
    public float3 position;
    public float3 velocity;

    public bool active;
    public int IsFriendly;
}

public class AgentComponent : ComponentDataProxy<AgentData>
{
    
}
