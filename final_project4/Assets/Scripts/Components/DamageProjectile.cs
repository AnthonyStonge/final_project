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
    public BulletType Type;
    public float Speed;
    public float Radius;
}

[Serializable]
public struct DamageArea : IComponentData
{
    public float Damage;
    public float Radius;
    public uint CollisionFilter;

    public bool SingleUse;
    [NonSerialized]
    public bool WasUsed;
}
