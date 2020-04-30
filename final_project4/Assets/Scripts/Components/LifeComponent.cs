using Unity.Entities;

[GenerateAuthoringComponent]
public struct LifeComponent : IComponentData
{
    public Range Life;
}
