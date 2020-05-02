using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MonoAudioManager : MonoBehaviour
{
    public AudioMixer MainAudioMixer;
    public AudioSource SoundtrackSource;
    public AudioSource AmbienceSource;
    public AudioClip Song1;
    public AudioClip Song2;
    public AudioClip Song3;
    public AudioClip Song4;
    public AudioClip Song5;
    public AudioClip Song6;

    private int currentSongPlaying = 1;
    
    void Start()
    {
        SoundtrackSource.Play();
    }

    private bool changeNow = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            changeNow = true;
        }
        
        if (!SoundtrackSource.isPlaying || changeNow)
        {
            changeNow = false;
            currentSongPlaying++;
            if (currentSongPlaying == 7)
                currentSongPlaying = 1;
            switch (currentSongPlaying)
            {
                case 1:
                    SoundtrackSource.clip = Song1;
                    break;
                case 2:
                    SoundtrackSource.clip = Song2;
                    break;
                case 3:
                    SoundtrackSource.clip = Song3;
                    break;
                case 4:
                    SoundtrackSource.clip = Song4;
                    break;
                case 5:
                    SoundtrackSource.clip = Song5;
                    break;
                case 6:
                    SoundtrackSource.clip = Song6;
                    break;
            }
            SoundtrackSource.Play();
        }
    }
}
