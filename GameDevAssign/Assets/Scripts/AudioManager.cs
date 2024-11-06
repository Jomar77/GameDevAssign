using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource backgroundMusicSource; // Reference to the AudioSource for background music
    public List<AudioSource> sfxSourceList;

    private AudioSource sfxSource;// Reference to the AudioSource for sound effects

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate instance if it exists
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    private void Start()
    {
        // Load the saved volume settings
        float savedMusicVolume = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
        float savedSfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        backgroundMusicSource.volume = savedMusicVolume;

        sfxSource = PlayRandomAudioSource();
        sfxSource.volume = savedSfxVolume;

        // Start playing the audio if it isn't already
        if (!backgroundMusicSource.isPlaying)
        {
            backgroundMusicSource.loop = true; // Loop the music
            backgroundMusicSource.Play(); // Start playing the music
        }
    }


    public AudioSource PlayRandomAudioSource()
    {

        if (sfxSourceList.Count == 0) return null; // Ensure the list is not empty

        int randomIndex = Random.Range(0, sfxSourceList.Count); // Get a random index
        AudioSource selectedSource = sfxSourceList[randomIndex]; // Select the AudioSource

        return selectedSource;
    }

    public void SetMusicVolume(float volume)
    {
        backgroundMusicSource.volume = volume;
        PlayerPrefs.SetFloat("BackgroundVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public AudioSource GetSFXVolume()
    {
        return sfxSource;
    }


    public AudioClip PlayRandomSFX()
    {
        if (sfxSourceList.Count == 0) return null; // Ensure the list is not empty

        int randomIndex = Random.Range(0, sfxSourceList.Count); // Get a random index
        AudioClip selectedClip = sfxSourceList[randomIndex].clip; // Select the AudioClip from the AudioSource
        PlaySFX(selectedClip); // Play the selected sound effect
        return selectedClip;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxSource.volume); // Use the current volume setting
        }// Play a sound effect
    }
}
