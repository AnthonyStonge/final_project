using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Static.Events
{
    public static class SoundEvents
    {
        public static void PlaySound(AudioSource source, AudioClip clip)
        {
            source.clip = clip;
            source.Play();
        }
    }
}
