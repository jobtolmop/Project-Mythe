using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGame : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

    private void Start()
    {
        winPanel = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(5).gameObject;
        winPanel.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("Enemy").SetActive(false);
            other.GetComponent<PlayerMovement>().enabled = false;
            Time.timeScale = 0;
            winPanel.SetActive(true);
            StartCoroutine("QuitGame");
        }
    }

    private IEnumerator QuitGame()
    {
        yield return new WaitForSeconds(10);
        Application.Quit();
    }
}
