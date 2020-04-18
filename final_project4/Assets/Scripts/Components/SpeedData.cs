using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct SpeedData : IComponentData
{
    public float Value;
}