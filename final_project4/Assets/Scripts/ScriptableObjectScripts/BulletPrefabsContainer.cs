using System;
using System.Collections.Generic;
using Enums;
using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Bullets/Bullet Prefab Container", fileName = "new BulletPrefabContainer")]
public class BulletPrefabsContainer : ScriptableObject
{
    public List<BulletLink> Prefabs;

    [Serializable]
    public struct BulletLink
    {
        public ProjectileType Type;
        public GameObject Prefab;
        public PhysicsShapeAuthoring Filter;
    }
}
