using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics.Authoring;
using UnityEngine;

[GenerateAuthoringComponent]
public struct BulletCollider : IComponentData
{
    public PhysicsCategoryTags BelongsTo;
    public PhysicsCategoryTags CollidesWith;
    public int GroupIndex;
}
