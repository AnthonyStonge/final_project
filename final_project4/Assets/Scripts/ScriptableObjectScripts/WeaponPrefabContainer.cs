using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Weapon/Weapon Prefab Container", fileName = "new WeaponPrefabContainer")]
public class WeaponPrefabContainer : ScriptableObject
{
    public List<PrefabLink> Prefabs;
    
    [Serializable]
    public struct PrefabLink
    {
        public WeaponType Type;
        public GameObject Prefab;
    }
}
