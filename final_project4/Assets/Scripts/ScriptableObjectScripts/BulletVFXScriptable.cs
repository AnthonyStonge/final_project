﻿using System;
using Enums;
using UnityEngine;
using UnityEngine.VFX;

[Serializable]
[CreateAssetMenu(menuName = "Scriptable/VFX/Bullet")]
public class BulletVFXScriptable : ScriptableObject
{
    public ProjectileType WeaponType;
    public VisualEffect VFX;
}
/*
[Serializable]
public struct VFXStruct
{
    public BulletInfo.BulletCollisionType EventType;
    public VisualEffect VisualEffect;
}
*/