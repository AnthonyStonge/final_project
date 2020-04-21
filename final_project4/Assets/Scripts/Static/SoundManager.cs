using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public static AudioSource audioSource;
    
    public static void Initialize()
    {
        //TODO LOAD AUDIO SOURCE SOMEWHERE
    }

    public static void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
    
    public static void PlaySoundLoop()
    {
        //TODO PLAY SOUND, WHEN FINISH, RESTART IT (KEEP HAND ON THE SOUND)
    }
}
