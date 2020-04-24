using System;
using System.Collections.Generic;
using Enums;
using EventStruct;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Scriptable/VFX/Bullet")]
public class BulletVFXScriptable : ScriptableObject
{
    public WeaponType WeaponType;
    // public List<> SFXList;
}

// [Serializable]
// public struct SoundEnum
// {
//     public WeaponInfo.WeaponEventType eventType;
//     public AudioClip sound;
// }
