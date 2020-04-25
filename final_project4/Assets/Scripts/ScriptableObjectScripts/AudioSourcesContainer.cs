using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Sounds/Audio Source Container", fileName = "new AudioSourceContainer")]
public class AudioSourcesContainer : ScriptableObject
{
    public List<AudioSourceLink> AudioSources;

    [Serializable]
    public struct AudioSourceLink
    {
        public AudioSourceType Type;
        public AudioSource Source;
        public bool IsPlayOneShotOnly;
    }
}