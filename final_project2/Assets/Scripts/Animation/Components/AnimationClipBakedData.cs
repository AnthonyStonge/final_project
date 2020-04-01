using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public struct AnimationClipBakedData : IComponentData
{
    public float TextureOffset;
    public float TextureRange;
    public float OnePixelOffset;
    public int TextureWidth;

    public float AnimationLength;
    public bool Looping;
}
