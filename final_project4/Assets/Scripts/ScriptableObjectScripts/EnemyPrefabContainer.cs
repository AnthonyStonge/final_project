using System;
using System.Collections.Generic;
using UnityEngine;
using Type = Enums.Type;

[CreateAssetMenu(menuName = "ScriptableObjects/Enemy/Enemy Prefab container", fileName = "new EnemyPrefabContainer")]
public class EnemyPrefabContainer : ScriptableObject
{
    public List<EnemyPrefab> Prefabs;
    
    [Serializable]
    public struct EnemyPrefab
    {
        public Type Type;
        public GameObject Prefab;
    }
}
