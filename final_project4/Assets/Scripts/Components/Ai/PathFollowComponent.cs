using System.Collections;
using System.Collections.Generic;
using Enums;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
public struct PathFollowComponent : IComponentData
{
    public int pathIndex;
    public float timeBetweenCheck;
    public int2 PositionToGo;
    public bool EnemyReachedTarget;
    public EnnemyState ennemyState;
    public bool IsWalking;
    public float timeWonderingCounter;
}
