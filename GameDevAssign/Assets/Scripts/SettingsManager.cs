using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{

    public Slider volumeSlider; // Reference to the UI Slider for background music volume control
    public Slider sfxSlider; // Reference to the UI Slider for sound effects volume control

    private void Start()
    {
        // Load the saved volume settings
        float savedMusicVolume = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
        float savedSfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        volumeSlider.value = savedMusicVolume;
        sfxSlider.value = savedSfxVolume;

        // Add listeners to the sliders to adjust the volume in AudioManager
        volumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void returnMainScene()
    {
        SceneManager.LoadScene("Main Screen");
    }

    private void SetMusicVolume(float volume)
    {
        AudioManager.Instance.SetMusicVolume(volume);
    }

    private void SetSFXVolume(float volume)
    {
        AudioManager.Instance.SetSFXVolume(volume);
    }


}
