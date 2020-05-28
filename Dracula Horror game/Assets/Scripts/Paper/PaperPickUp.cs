using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperPickUp : MonoBehaviour
{
    [SerializeField] private Sprite paperSprite;
    public PaperSpawner spawner {get; set;}
    public Sprite PaperSprite { get { return paperSprite; } set { paperSprite = value; } }

    [SerializeField] private AudioSource sfx;
    private bool pickedUp = false;

    public void PickUpPage()
    {
        if (pickedUp)
        {
            return;
        }

        pickedUp = true;
            
        //Controls page dont mess with the spawner
        if (spawner != null)
        {
            spawner.Papers.Remove(gameObject);
            spawner.PlayerPanel.SetActive(false);
            spawner.PlayerPanel.SetActive(true);
            spawner.PlayerPanel.GetComponentInChildren<Text>().text = Mathf.Abs(spawner.Papers.Count - spawner.PapersToSpawn) + "/" + spawner.PapersToSpawn;
        }
        else
        {
            spawner = FindObjectOfType<PaperSpawner>();
        }
           
        spawner.PaperPanel.SetActive(true);
        spawner.PaperPanel.transform.GetChild(0).GetComponent<Image>().sprite = paperSprite;
        Time.timeScale = 0;

        if (!sfx.isPlaying)
        {
            sfx.Play();
            StartCoroutine("WaitForSFX");
        }            
        if (spawner.Papers.Count <= 0)
        {
            spawner.Win();
            AudioManager.instance.Play("Door_Open");
        }
    }

    private IEnumerator WaitForSFX()
    {
        while(sfx.isPlaying)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
