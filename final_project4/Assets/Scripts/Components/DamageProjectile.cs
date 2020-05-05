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
    public ProjectileType Type;
    public float Speed;
    [Tooltip("Should be half of the scale?")]
    public float Radius;
}