using System.Collections;
using System.Collections.Generic;
using EventStruct;
using UnityEngine;

public static class SoundManager
{
    public static void PlaySound(int clipId)
    {
        //Get AudioSource for this clip
        Source source = SoundHolder.AudioSources[SoundHolder.SoundsToAudioSource[clipId]];

        //Get AudioClip
        AudioClip clip = SoundHolder.Sounds[clipId].AudioClip;

        //Set and Play
        if (source.IsPlayOneShotOnly)
            source.AudioSource.PlayOneShot(clip);
        else
        {
            source.AudioSource.clip = clip;
            source.AudioSource.Play();
        }
    }

    public static void DecrementNotAvailableSounds(float deltaTime)
    {
        foreach (Clip clip in SoundHolder.Sounds.Values)
        {
            if (!clip.IsAvailable)
                clip.Timer -= deltaTime;
        }
    }
}