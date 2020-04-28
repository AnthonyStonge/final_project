using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct TypeData : IComponentData
{
    public Type Value;
}
