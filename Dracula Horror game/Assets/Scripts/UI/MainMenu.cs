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
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        fadePanel.SetActive(false);
        creditsPanel.SetActive(false);
        if (AudioManager.instance != null)
        {
            AudioManager.instance.StopPlaying("Chase");
            AudioManager.instance.Play("MainMenu");
            AudioManager.instance.CurrSound.source.volume = 1;
        }        
    }

    public void StartGame()
    {        
        StartCoroutine(GameManager.instance.LoadAsync(1));
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
