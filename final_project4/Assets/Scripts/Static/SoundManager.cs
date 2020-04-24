using System.Collections;
using System.Collections.Generic;
using EventStruct;
using UnityEngine;

public static class SoundManager
{
    public static void Initialize()
    {
        
    }

    public static void PlaySound(int clipId)
    {
        //Get AudioSource for this clip
        AudioSource source = SoundHolder.AudioSources[SoundHolder.SoundsToAudioSource[clipId]];

        //Get AudioClip
        AudioClip clip = SoundHolder.Sounds[clipId];
        
        //Set and Play
        source.clip = clip;
        source.Play();
    }
}
