using Enums;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct AmmunitionComponent : IComponentData
{
    public WeaponType TypeAmunation;
    public int AmunationQuantity;
}
