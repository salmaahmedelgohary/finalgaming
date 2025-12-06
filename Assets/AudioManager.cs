using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioClip overworldMusic;
    public AudioClip caveMusic;
    public AudioClip[] variousSFX;
   // void Awake()
   // {
        //make sure the entire game only has one Audio Manager throughout
      //  if (Instance == null)
       // {
        //    Instance = this;
         //   DontDestroyOnLoad(gameObject);
      //  }
       // else Destroy(gameObject);
    //}

    void Start()
    {
        //background music clip is assigned, and volume starts off being zero.
        musicSource.clip = overworldMusic;
        musicSource.Play();
    }

    //Public in case another object needs to call for a specific sound effect to begin playing  
    public void PlayMusicSFX(AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    //Public in case another object needs to call for a specific soundtrack to begin playing  
    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
    //Function takes a bunch of sound clips as parameters  
    public void PlayRandomSFX(params AudioClip[] clips)
    {
        //assign the incoming array of items to our local arrayList variable called 'variousSFX'  
        variousSFX = clips;

        //randomly select a sound clip from the arrayList, then play that clip  
        int index = Random.Range(0, variousSFX.Length);
        sfxSource.PlayOneShot(variousSFX[index]);
    }
}
