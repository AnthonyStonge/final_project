using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct InputsComponent : IComponentData
{
    public float3 direction;
    public bool dash;
    public bool interact;
    public bool cancel;
    public int inventory;
}
