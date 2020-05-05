using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Type = Enums.Type;
[Serializable]
public struct EnemySpawnerInfo
{
    public int2 spawnerPos;
    public int NbEnnemyMax;
    public Type EnemyType;
    public int currentEnnemySpawn;
    public int2 TimeRangeBetweenSpawn;
    public int2 EnemySpawnDirection;
    public int Distance;
    [HideInInspector]
    public float currentTime;
}
