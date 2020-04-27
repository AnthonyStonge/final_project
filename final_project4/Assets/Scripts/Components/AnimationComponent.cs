using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
[GenerateAuthoringComponent]
public struct AnimationComponent : IComponentData
{
    public Animation.AnimationType AnimationType;

    [HideInInspector] public ushort MeshIndexAt;
}
