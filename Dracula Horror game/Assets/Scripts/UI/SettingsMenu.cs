using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private Dropdown dropdownQuality;
    [SerializeField] private Toggle toggle;

    private Resolution[] resolutions;
    
    void Start()
    {
        dropdown.ClearOptions();

        resolutions = Screen.resolutions;

        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            Screen.SetResolution(resolutions[PlayerPrefs.GetInt("ResolutionIndex")].width, resolutions[PlayerPrefs.GetInt("ResolutionIndex")].height, PlayerPrefs.GetInt("Fullscreen", 1) != 0);
        }
        else
        {
            Screen.fullScreen = PlayerPrefs.GetInt("Fullscreen", 1) != 0;
        }

        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality", 5));
        dropdownQuality.value = PlayerPrefs.GetInt("Quality", 5);
        toggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) != 0;

        List<string> options = new List<string>();

        int currRes = 0;

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
        dropdown.RefreshShownValue();
    }

    public void SetResolution(int i)
    {
        PlayerPrefs.SetInt("ResolutionIndex", i);
        Screen.SetResolution(resolutions[i].width, resolutions[i].height, Screen.fullScreen);
    }

    public void SetQualitySettings(int i)
    {
        PlayerPrefs.SetInt("Quality", i);
        QualitySettings.SetQualityLevel(i);
    }

    public void SetFullscreen(bool toggle)
    {
        PlayerPrefs.SetInt("Fullscreen", toggle ? 1 : 0);
        Screen.fullScreen = toggle;
    }
}
