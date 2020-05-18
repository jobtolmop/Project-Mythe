using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private float timer = 0;

    private void Start()
    {
        pausePanel.SetActive(false);
        QualitySettings.SetQualityLevel(5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (Time.timeScale > 0)
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        if (Input.GetButton("Cancel"))
        {
            timer += Time.fixedUnscaledDeltaTime;

            if (timer > 10)
            {
                Application.Quit();
            }
        }
        else
        {
            timer = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            QualitySettings.IncreaseLevel();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            QualitySettings.DecreaseLevel();
        }
    }
}
