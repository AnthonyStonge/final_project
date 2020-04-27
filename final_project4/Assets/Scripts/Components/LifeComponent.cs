using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
[GenerateAuthoringComponent]
public struct LifeComponent : IComponentData
{
    public int MaxLife;
    public int CurrentLife;
}
