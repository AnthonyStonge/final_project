using Enums;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct AmmunitionComponent : IComponentData
{
    public WeaponType TypeAmmunition;
    public int AmmunitionQuantity;
}
