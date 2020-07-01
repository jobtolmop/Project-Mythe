using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private bool playCutscene = true;
    public bool PlayCutscene { get { return playCutscene; } set { playCutscene = value; } }
    public bool ReadyLoading { get; set; } = false;

    public UIHandler UI { get; set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadAsync(int scene)
    {
        if (UI != null)
        {
            UI.FadeInPanel.SetActive(true);
        }

        while (!ReadyLoading)
        {
            yield return null;
        }

        ReadyLoading = false;

        //Dont go to the load scene if going back to the main menu
        if (scene != 0)
        {
            AsyncOperation ao = SceneManager.LoadSceneAsync("LoadingScene");

            while (!ao.isDone)
            {
                Debug.Log("Loading loading screen...");
                yield return null;

                if (ao.progress >= 0.9f)
                {
                    break;
                }
            }

            //Starting new coroutine because yield retun null doesnt work anymore because of the scene loading in this function
            StartCoroutine(LoadActualScene(scene));
        }        
        else
        {
            SceneManager.LoadSceneAsync(scene);
        }
    }

    private IEnumerator LoadActualScene(int scene)
    {
        yield return new WaitForSeconds(5);

        UI.LoadingText.gameObject.SetActive(true);
        //Now load the scene that was supposed to load
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {            
            if (operation.progress >= 0.9f)
            {
                UI.LoadingText.text = "Press Enter to start the game";

                if (Input.GetButtonDown("Submit"))
                {
                    AudioManager.instance.FadeOutRate = 0.005f;
                    StartCoroutine(AudioManager.instance.StartFadeOut());

                    if (UI != null)
                    {
                        UI.FadeInPanel.SetActive(true);
                    }

                    while (!ReadyLoading)
                    {
                        yield return null;
                    }

                    operation.allowSceneActivation = true;
                    break;
                }
            }
            else
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);

                UI.LoadingText.text = "Loading... " + (progress * 100).ToString("F0") + "%";
            }

            yield return null;
        }
    }
}
