using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject content;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject mainMenu;
    
    [SerializeField]
    private Slider effectsVolumeSlider;
    [SerializeField]
    private Slider musicVolumeSlider;
    
    [SerializeField] 
    private AudioMixer audioMixer;

    private void Start()
    {
        OnEffectVolumeChange(effectsVolumeSlider.value);
        OnMusicVolumeChange(musicVolumeSlider.value);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SetPause(!content.activeSelf);
            settings.SetActive(false);
        }
    }

    private void SetPause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
        content.SetActive(pause);
        mainMenu.SetActive(pause);
    }
    
    public void OnResumeButtonClick()
    {
        SetPause(false);
    }
    
    public void OnSettingsButtonClick()
    {
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }
    
    
    public void OnEffectVolumeChange(float value)
    {
        Debug.Log(dBA(value));
        audioMixer.SetFloat("EffectsVolume", dBA(value));
    }
    
    public void OnMusicVolumeChange(float value)
    {
        audioMixer.SetFloat("MusicVolume", dBA(value));
    }
    
    private float dBA(float volume)
    {
        return -80f + 100F * volume;
    }
}
