using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;


public struct AnimationData : IComponentData
{
    public short MeshIndexAt;
    public bool Decrement;
    public short MaxIndex;


    public void ChangeFrame()
    {
        if (Decrement)
        {
            MeshIndexAt--;
            if (MeshIndexAt < 0) MeshIndexAt = MaxIndex;
        }
        else
        {
            MeshIndexAt++;
            if (MeshIndexAt > MaxIndex) MeshIndexAt = 0;
        }
    }
}
