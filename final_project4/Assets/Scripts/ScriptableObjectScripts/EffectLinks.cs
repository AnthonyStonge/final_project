using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using EventStruct;
using UnityEngine;
using UnityEngine.VFX;
[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Effect Links", fileName = "new EffectLinks")]

public class EffectLinks : ScriptableObject
{
    [Header("Effect")]
    public VisualEffect Effect;
    public int MaxAmount = 2048;
    [Header("Types to link the effect to")]
    public List<WeaponLinks> Weapons;

    public List<BulletLinks> Bullets;
    
    [Serializable]
    public struct WeaponLinks
    {
        public WeaponType WeaponType;
        public WeaponInfo.WeaponEventType EventType;
    }
    [Serializable]
    public struct BulletLinks
    {
        public ProjectileType BulletType;
        public BulletInfo.BulletCollisionType CollisionType;
    }
}
