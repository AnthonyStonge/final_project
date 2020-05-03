using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
[GenerateAuthoringComponent]
public struct PathFindingComponent : IComponentData
{
    public int2 startPos;
    public int2 endPos;
    public int findPath;
}
