using System;
using System.Collections.Generic;
using Enums;
using EventStruct;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Sounds/Weapon", fileName = "New Weapon")]
public class WeaponSoundsScriptableObject : ScriptableObject
{
    public WeaponType WeaponType;
    public List<Sound> Sounds;

    [Serializable]
    public struct Sound
    {
        public WeaponInfo.WeaponEventType EventType;
        public AudioClip Clip;
    }
}
