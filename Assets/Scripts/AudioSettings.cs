using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider fxSlider;
    [SerializeField] private Slider masterSlider;

    private void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
            LoadVolume();
        else
        {
         SetMusicVolume();
         SetFXVolume();
         SetMasterVolume();
        }
        
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        myMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetFXVolume()
    {
        float volume = fxSlider.value;
        myMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("fxVolume", volume);
    }
    public void SetMasterVolume()
    {
        float volume = masterSlider.value;
        myMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        fxSlider.value = PlayerPrefs.GetFloat("fxVolume");
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        SetFXVolume();
        SetMasterVolume();
        SetMusicVolume();
    }
}
