using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
[GenerateAuthoringComponent]
public struct PathFollow : IComponentData
{
    public int pathIndex;
    public float timeBetweenCheck;
}
