using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject paperPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject arrows;
    [SerializeField] private Transform dotImage;

    public Transform Dot { get { return dotImage; } }

    private PaperSpawner paperSpawner;
    [SerializeField] private Image paper;
    [SerializeField] private Sprite controls;

    private int currId = 0;

    private List<int> ids = new List<int>();
    public List<int> Ids { get { return ids; } }

    private float timer = 0;

    private void Start()
    {        
        pausePanel.SetActive(false);
        QualitySettings.SetQualityLevel(5);
        paperSpawner = FindObjectOfType<PaperSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        PauseGameCheck();

        //QuitGameCheck();

        PageScroll();

        GraphicsCheck();
    }

    private void PauseGameCheck()
    {
        if (Input.GetButtonDown("Cancel") && !winPanel.activeSelf)
        {
            if (paperPanel.activeSelf)
            {
                paperPanel.SetActive(false);
                Time.timeScale = 1;
            }
            else
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
        }
    }

    private void QuitGameCheck()
    {
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

    }

    private void PageScroll()
    {
        if (ids.Count == 0 || !pausePanel.activeSelf)
        {
            paper.gameObject.SetActive(false);
            arrows.SetActive(false);
            return;
        }
        if (ids.Count == 1)
        {
            if (ids[0] == 0)
            {
                paper.gameObject.SetActive(true);
                arrows.SetActive(false);
                return;
            }
            else
            {
                int nextPaper = ids[0] - 1;
                paper.sprite = paperSpawner.Sprites[nextPaper];
                paper.gameObject.SetActive(true);
                arrows.SetActive(false);
                return;
            }
        }

        if (ids[0] != 0 && currId == 0)
        {
            paper.sprite = paperSpawner.Sprites[ids[0]];
        }

        paper.gameObject.SetActive(true);
        arrows.SetActive(true);


        if (Input.GetButtonDown("Right"))
        {
            if (currId < ids.Count - 1)
            {
                currId++;
                int nextPaper = ids[currId] - 1;
                if (nextPaper < 0)
                {
                    paper.sprite = controls;
                }
                else
                {
                    paper.sprite = paperSpawner.Sprites[nextPaper];
                }
            }            
        }
        else if(Input.GetButtonDown("Left"))
        {
            if (currId > 0)
            {
                currId--;
                int nextPaper = ids[currId] - 1;
                if (nextPaper < 0)
                {
                    paper.sprite = controls;
                }
                else
                {
                    paper.sprite = paperSpawner.Sprites[nextPaper];
                }                
            }
        }
    }

    private void GraphicsCheck()
    {
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
