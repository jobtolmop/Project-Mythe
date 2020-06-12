using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject fadePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject playButton;

    private void Start()
    {
        AudioManager.instance.StopPlaying("Chase");
        fadePanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void StartGame()
    {        
        SceneManager.LoadSceneAsync("SampleScene");
        fadePanel.SetActive(true);
    }

    public void ShowCredits()
    {
        EventSystem.current.SetSelectedGameObject(null);
        creditsPanel.SetActive(true);        
    }

    public void ToggleSettings(bool toggle)
    {
        settingsPanel.SetActive(toggle);

        if (!toggle)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(playButton);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    } 
}
