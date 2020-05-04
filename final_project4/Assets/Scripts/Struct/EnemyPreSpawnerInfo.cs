using System;
using Unity.Mathematics;
using Type = Enums.Type;
[Serializable]
public struct EnemyPreSpawnerInfo
{
    public int2 spawnerPos;
    public Type EnemyType;

}
