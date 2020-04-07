using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct InputComponent : IComponentData
{
    public float2 Move;
    public bool Reload;
    public bool Dash;
    public bool Interact;
    public int Inventory;
    public bool Cancel;
    public float3 Mouse;
}
