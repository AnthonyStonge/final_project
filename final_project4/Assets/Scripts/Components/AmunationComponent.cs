using Enums;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct AmunationComponent : IComponentData
{
    public WeaponType TypeAmunation;
    public int AmunationQuantity;
}
