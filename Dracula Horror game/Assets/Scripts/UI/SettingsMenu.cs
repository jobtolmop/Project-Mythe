using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private Dropdown dropdownQuality;
    [SerializeField] private Dropdown dropdownShadows;
    [SerializeField] private Toggle toggleFullscreen;
    [SerializeField] private Toggle toggleVsync;
    [SerializeField] private Slider sliderShadow;
    [SerializeField] private Slider sliderFog;
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderFX;
    [SerializeField] private Slider sliderFootsteps;
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer fxMixer;
    [SerializeField] private AudioMixer footstepsMixer;

    private Resolution[] resolutions;

    private int currRes;

    void Start()
    {
        dropdown.ClearOptions();

        resolutions = Screen.resolutions;

        SettingsStartValues();

        List<string> options = new List<string>();

        currRes = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currRes = i;
            }
        }

        dropdown.AddOptions(options);
        dropdown.value = currRes;
        dropdown.RefreshShownValue();
    }

    private void SettingsStartValues()
    {
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            Screen.SetResolution(resolutions[PlayerPrefs.GetInt("ResolutionIndex")].width, resolutions[PlayerPrefs.GetInt("ResolutionIndex")].height, PlayerPrefs.GetInt("Fullscreen", 1) != 0);
        }
        else
        {
            Screen.fullScreen = PlayerPrefs.GetInt("Fullscreen", 1) != 0;
        }

        QualitySettings.vSyncCount = PlayerPrefs.GetInt("Vsync", 1);
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", 5));
        QualitySettings.shadowResolution = (ShadowResolution)PlayerPrefs.GetInt("ShadowRes", 3);
        dropdownQuality.value = PlayerPrefs.GetInt("Quality", 5);
        dropdownQuality.RefreshShownValue();
        dropdownShadows.value = PlayerPrefs.GetInt("ShadowRes", 3);
        dropdownShadows.RefreshShownValue();
        toggleFullscreen.isOn = PlayerPrefs.GetInt("Fullscreen", 1) != 0;
        toggleVsync.isOn = PlayerPrefs.GetInt("Vsync", 1) != 0;
        sliderShadow.value = PlayerPrefs.GetFloat("ShadowDis", 150);
        QualitySettings.shadowDistance = PlayerPrefs.GetFloat("ShadowDis", 150);
        sliderFog.value = PlayerPrefs.GetFloat("Fog", 0.01f) * 100;
        RenderSettings.fogDensity = PlayerPrefs.GetFloat("Fog", 0.01f);
        sliderMusic.value = PlayerPrefs.GetFloat("musicVolume", 0);
        musicMixer.SetFloat("volume", PlayerPrefs.GetFloat("musicVolume", 0));
        sliderFX.value = PlayerPrefs.GetFloat("fxVolume", 0);
        fxMixer.SetFloat("volume", PlayerPrefs.GetFloat("fxVolume", 0));
        sliderFootsteps.value = PlayerPrefs.GetFloat("footstepsVolume", 5);
        footstepsMixer.SetFloat("volume", PlayerPrefs.GetFloat("footstepsVolume", 5));
    }

    public void ResetToDefault()
    {
        //Quality
        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        PlayerPrefs.SetInt("ResolutionIndex", currRes);
        PlayerPrefs.SetInt("Vsync", 1);
        QualitySettings.vSyncCount = 1;
        QualitySettings.SetQualityLevel(5);
        PlayerPrefs.SetInt("Quality", 5);
        dropdownQuality.value = PlayerPrefs.GetInt("Quality", 5);
        dropdownQuality.RefreshShownValue();
        dropdown.value = currRes;
        dropdown.RefreshShownValue();
        PlayerPrefs.SetInt("Fullscreen", 1);
        toggleFullscreen.isOn = true;
        PlayerPrefs.SetInt("Vsync", 1);
        toggleVsync.isOn = true;

        //Lighting
        PlayerPrefs.SetInt("ShadowRes", 3);
        QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
        dropdownShadows.value = 3;
        dropdownShadows.RefreshShownValue();
        PlayerPrefs.SetFloat("ShadowDis", 150);
        sliderShadow.value = 150;
        QualitySettings.shadowDistance = 150;
        PlayerPrefs.SetFloat("Fog", 0.01f);
        sliderFog.value = 1;
        RenderSettings.fogDensity = 0.01f;

        //Audio
        PlayerPrefs.SetFloat("musicVolume", 0);
        sliderMusic.value = 0;
        musicMixer.SetFloat("volume", 0);
        PlayerPrefs.SetFloat("fxVolume", 0);
        sliderFX.value = 0;
        fxMixer.SetFloat("volume", 0);
        PlayerPrefs.SetFloat("footstepsVolume", 5);
        sliderFootsteps.value = 5;
        footstepsMixer.SetFloat("volume", 5);
    }

    //Graphics settings
    public void SetResolution(int i)
    {
        PlayerPrefs.SetInt("ResolutionIndex", i);
        Screen.SetResolution(resolutions[i].width, resolutions[i].height, Screen.fullScreen);
    }

    public void SetQualitySettings(int i)
    {
        PlayerPrefs.SetInt("Quality", i);
        QualitySettings.SetQualityLevel(i);
        SettingsStartValues();

    }

    public void SetFullscreen(bool toggle)
    {
        PlayerPrefs.SetInt("Fullscreen", toggle ? 1 : 0);
        Screen.fullScreen = toggle;
    }

    public void SetVsync(bool toggle)
    {
        PlayerPrefs.SetInt("Vsync", toggle ? 1 : 0);
        QualitySettings.vSyncCount = toggle ? 1 : 0;
    }

    //Lighting settings
    public void SetShadowRes(int i)
    {
        PlayerPrefs.SetInt("ShadowRes", i);
        QualitySettings.shadowResolution = (ShadowResolution)i;
    }

    public void SetShadowDistance(float i)
    {
        PlayerPrefs.SetFloat("ShadowDis", i);
        QualitySettings.shadowDistance = i;
    }

    public void SetFogDensity(float i)
    {
        i /= 100;
        PlayerPrefs.SetFloat("Fog", i);
        RenderSettings.fogDensity = i;
    }

    //Audio settings
    public void SetMusicVolume(float i)
    {
        PlayerPrefs.SetFloat("musicVolume", i);
        musicMixer.SetFloat("volume", i);
    }

    public void SetFxVolume(float i)
    {
        PlayerPrefs.SetFloat("fxVolume", i);
        fxMixer.SetFloat("volume", i);
    }

    public void SetFootstepsVolume(float i)
    {
        PlayerPrefs.SetFloat("footstepsVolume", i);
        footstepsMixer.SetFloat("volume", i);
    }
}
