using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;

public struct InteractableComponent : IComponentData
{
    public InteractableType Type;
    public InteractableObjectType ObjectType;
}
