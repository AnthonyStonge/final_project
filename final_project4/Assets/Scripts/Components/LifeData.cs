using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct LifeData : IComponentData
{
    public Range Value;
}
