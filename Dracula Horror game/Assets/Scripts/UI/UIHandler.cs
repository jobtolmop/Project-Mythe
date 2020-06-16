using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseParent;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject collectionPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject collectionButton;
    [SerializeField] private GameObject backButton;
    [SerializeField] private GameObject qualityButton;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject paperPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject deathPanel;
    [SerializeField] private GameObject fadeInPanel;
    [SerializeField] private GameObject arrows;
    [SerializeField] private Transform dotImage;
    [SerializeField] private Text fpsText;

    public Transform Dot { get { return dotImage; } }
    public GameObject WinPanel { get { return winPanel; } } 

    private PaperSpawner paperSpawner;
    [SerializeField] private Image paper;
    [SerializeField] private Sprite controls;

    private int currId = 0;

    private List<int> ids = new List<int>();
    public List<int> Ids { get { return ids; } }

    private float timer = 0;

    private void Start()
    {        
        pauseParent.SetActive(false);
        paperSpawner = FindObjectOfType<PaperSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            fpsText.gameObject.SetActive(!fpsText.gameObject.activeSelf);
        }

        if (fpsText.gameObject.activeSelf)
        {
            timer += Time.unscaledDeltaTime;

            if (timer > 0.5f)
            {
                int fps = (int)(1f / Time.unscaledDeltaTime);
                fpsText.text = "FPS: " + fps;
                timer = 0;
            }
        }        
        PauseGameCheck();

        //QuitGameCheck();

        PageScroll();
    }

    private void PauseGameCheck()
    {
        if (Input.GetButtonDown("Cancel") && !winPanel.activeSelf && !deathPanel.activeSelf)
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
                    pauseParent.SetActive(true);
                    pausePanel.SetActive(true);
                    collectionPanel.SetActive(false);
                    settingsPanel.SetActive(false);
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(collectionButton.gameObject);
                    pausePanel.GetComponentInChildren<Text>().text = Mathf.Abs(paperSpawner.Papers.Count - paperSpawner.PapersToSpawn) + "/" + paperSpawner.PapersToSpawn;
                    Time.timeScale = 0f;
                }
                else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    pauseParent.SetActive(false);
                    Time.timeScale = 1f;
                }
            }
        }
    }

    public void ToggleCollectionPanel(bool toggle)
    {
        collectionPanel.SetActive(toggle);
        pausePanel.SetActive(!toggle);

        if (toggle)
        {
            collectionPanel.GetComponentInChildren<Text>().text = Mathf.Abs(paperSpawner.Papers.Count - paperSpawner.PapersToSpawn) + "/" + paperSpawner.PapersToSpawn;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(backButton);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(collectionButton);
        }        
    }

    public void ToggleSettingsPanel(bool toggle)
    {
        settingsPanel.SetActive(toggle);
        pausePanel.SetActive(!toggle);        

        if (toggle)
        {
            EventSystem.current.SetSelectedGameObject(null);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(settingsButton);
        }        
    }

    private void PageScroll()
    {
        int nextPaper = 0;

        if (ids.Count == 0 || !collectionPanel.activeSelf)
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
                nextPaper = ids[0] - 1;
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

        if (currId == 0)
        {
            arrows.transform.GetChild(0).gameObject.SetActive(false);
            arrows.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if(currId == ids.Count - 1)
        {
            arrows.transform.GetChild(0).gameObject.SetActive(true);
            arrows.transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            arrows.transform.GetChild(0).gameObject.SetActive(true);
            arrows.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (Input.GetButtonDown("Right"))
        {
            if (currId < ids.Count - 1)
            {
                currId++;               
            }            
        }
        else if(Input.GetButtonDown("Left"))
        {
            if (currId > 0)
            {
                currId--;          
            }
        }

        nextPaper = ids[currId] - 1;

        if (nextPaper < 0)
        {
            paper.sprite = controls;
        }
        else
        {
            paper.sprite = paperSpawner.Sprites[nextPaper];
        }
    }

    public void ExitToMainMenu()
    {
        fadeInPanel.SetActive(true);
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
