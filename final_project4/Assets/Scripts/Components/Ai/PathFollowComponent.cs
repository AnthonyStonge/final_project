using Enums;
using Unity.Entities;
using Unity.Mathematics;
[GenerateAuthoringComponent]
public struct PathFollowComponent : IComponentData
{
    public int2 PositionToGo;
    public float3 player;
    public int pathIndex;
    public EnemyState EnemyState;
    public float timeWonderingCounter;
    public bool BeginWalk;
}