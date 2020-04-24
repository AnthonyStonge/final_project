using System;
using System.Collections.Generic;
using Enums;
using EventStruct;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Scriptable/Sound/Weapon")]
public class WeapongSoundScriptable : ScriptableObject
{
    public WeaponType WeaponType;
    public List<SoundEnum> SFXList;
}

[Serializable]
public struct SoundEnum
{
    public WeaponInfo.WeaponEventType eventType;
    public AudioClip sound;
}
