using Enums;
using Unity.Entities;
using Unity.Mathematics;
[GenerateAuthoringComponent]
public struct PathFollowComponent : IComponentData
{
    public float2 WonderingPosition;
    public float3 PlayerPosition;
    public float2 BackPosition;
    public int pathIndex;
    public EnemyState EnemyState;
    public float timeWonderingCounter;
    public bool BeginWalk;
}