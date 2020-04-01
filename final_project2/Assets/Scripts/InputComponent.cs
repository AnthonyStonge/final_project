using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct InputComponent : IComponentData
{
    public float2 move;
    public bool dash;
    public bool interact;
    public int inventory;
    public bool pause;
}
