using Enums;
using Unity.Entities;
using Unity.Mathematics;
[GenerateAuthoringComponent]
public struct PathFollowComponent : IComponentData
{
    public int2 PositionToGo;
    public int pathIndex;
    public EnemyState EnemyState;
    public float timeWonderingCounter;      
}