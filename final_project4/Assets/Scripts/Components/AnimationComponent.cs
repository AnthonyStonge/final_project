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
    public ushort FrameRate;

    [HideInInspector] public ushort MeshIndexAt;
    [HideInInspector] public float Timer;
    [HideInInspector] public float TimeBetweenFrame => 1 / FrameRate;    //TODO Dont do a division every frame...
}
