using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Why static? Because the whole game will have only one instance of AudioManager
    public static AudioManager Instance;

    public AudioSource musicSource; // source that plays background music
    public AudioSource sfxSource; // source that plays sound effects

    public AudioClip overworldMusic; // audio clip of background music for level 1
    public AudioClip caveMusic; // audio clip of background music for level 2

    public AudioClip[] variousSFX; // array of sound effects clips to keep things varied

    void Awake()
    {
        // make sure the entire game only has one AudioManager throughout
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // background music clip is assigned, and playback begins
        if (musicSource != null && overworldMusic != null)
        {
            musicSource.clip = overworldMusic;
            musicSource.Play();
        }
    }

    // Function takes a bunch of sound clips as parameters
    public void PlayRandomSFX(params AudioClip[] clips)
    {
        // assign the incoming array of items to our local array variable called 'variousSFX'
        variousSFX = clips;

        if (variousSFX == null || variousSFX.Length == 0 || sfxSource == null) return;

        // randomly select a sound clip from the array, then play that clip
        int index = Random.Range(0, variousSFX.Length);
        sfxSource.PlayOneShot(variousSFX[index]);
    }

    // Public in case another object needs to call for a specific sound effect to begin playing
    public void PlayMusicSFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null) return;
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    // Public in case another object needs to call for a specific soundtrack to begin playing
    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;
        musicSource.clip = clip;
        musicSource.Play();
    }
}
