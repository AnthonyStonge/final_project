using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct DamageProjectile : IComponentData
{
    public float3 PreviousPosition;
    public ProjectileType Type;
    public float Speed;
    public float Radius;
}