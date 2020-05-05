using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
[CreateAssetMenu(menuName = "Grid/CreateGrid")]
public class ScriptableGrid : ScriptableObject
{
    public int2 gridSize;
    public float3 nodeSize;
    public List<int> indexNoWalkable = new List<int>();
    public List<int> enemyPreSpawnerIndex = new List<int>();
    public List<int> enemySpawnerIndex = new List<int>();
    public List<EnemyPreSpawnerInfo> enemyPreSpawner = new List<EnemyPreSpawnerInfo>();
    public List<EnemySpawnerInfo> enemySpawner = new List<EnemySpawnerInfo>();
}
