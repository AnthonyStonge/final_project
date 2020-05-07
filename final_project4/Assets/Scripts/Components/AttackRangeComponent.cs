using System;
using Unity.Entities;

[Serializable]
[GenerateAuthoringComponent]
public struct AttackRangeComponent : IComponentData
{
    //Act as the range to attack
    public float AttackDDistance;
    public float AgroDistance;
    public float FleeDistance;
    public bool CanAttack;
    public bool IsInRange;
}
