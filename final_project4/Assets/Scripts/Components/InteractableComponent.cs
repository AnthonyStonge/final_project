using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using UnityEngine;

public class InteractableComponent : IComponentData
{
    public InteractableType Type;
}
