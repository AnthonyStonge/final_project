using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
[GenerateAuthoringComponent]
public struct EnnemyComponent : IComponentData
{
    public int attackDistance;
    public bool inRange;
}
