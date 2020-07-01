using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinGame : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private PaperSpawner spawner;

    private void Start()
    {
        winPanel = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIHandler>().WinPanel;
        spawner = FindObjectOfType<PaperSpawner>();
        winPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Time.timeScale = 0;
            //GameObject.FindGameObjectWithTag("Enemy").SetActive(false);
            //other.GetComponent<PlayerMovement>().enabled = false;            
            winPanel.SetActive(true);

            if (spawner.WonGame)
            {
                winPanel.transform.GetChild(1).GetComponent<Text>().text = "You uncovered the truth and you told the people in your village about what had happened with you and the people who went missing in this mansion. When you showed the documents alot of people believed your story so the village is planning on an attack to kill that monster! You decide to not go there as well, because you don't want to see that place ever again...";
            }

            StartCoroutine("ExitToMainMenu");
        }
    }

    private IEnumerator ExitToMainMenu()
    {
        yield return new WaitForSecondsRealtime(15);
        StartCoroutine(GameManager.instance.LoadAsync(0));
    }
}
