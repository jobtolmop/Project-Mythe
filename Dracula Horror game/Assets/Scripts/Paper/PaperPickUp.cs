using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperPickUp : MonoBehaviour
{
    [SerializeField] private Sprite paperSprite;
    [SerializeField] private UIHandler ui;

    public PaperSpawner spawner {get; set;}
    public Sprite PaperSprite { get { return paperSprite; } set { paperSprite = value; } }

    public int Id { get; set; } = 0;

    [SerializeField] private AudioSource sfx;
    private bool pickedUp = false;
    [SerializeField] private bool destroy = true;
    [SerializeField] [TextArea] private string textToDisplay = "";

    private void Start()
    {
        textToDisplay = textToDisplay.Replace("\new", "\n");
        ui = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIHandler>();
    }

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
        spawner.PaperPanel.transform.GetChild(1).GetComponent<Text>().text = textToDisplay;

        if (destroy)
        {
            ui.Ids.Add(Id);
            ui.Ids.Sort(SortById);
        }        

        Time.timeScale = 0;

        if (!sfx.isPlaying)
        {
            sfx.Play();

            StartCoroutine("WaitForSFX");
        }            

        if (spawner.Papers.Count <= 0)
        {
            spawner.Win();
        }
    }

    private IEnumerator WaitForSFX()
    {
        while(sfx.isPlaying)
        {
            yield return null;
        }

        if (destroy)
        {
            Destroy(gameObject);
        }
        else
        {
            pickedUp = false;
            spawner = null;
        }        
    }

    static int SortById(int p1, int p2)
    {
        return p1.CompareTo(p2);
    }
}
