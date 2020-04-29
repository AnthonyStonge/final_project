using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
[GenerateAuthoringComponent]
public struct PathFollowComponent : IComponentData
{
    public int pathIndex;
    public float timeBetweenCheck;
    public int2 PositionToGo;
}
