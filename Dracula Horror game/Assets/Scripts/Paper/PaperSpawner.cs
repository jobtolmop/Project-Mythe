using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperSpawner : MonoBehaviour
{
    [SerializeField] private GameObject paperPref;
    [SerializeField] private Transform door;
    
    [SerializeField] private int papersToSpawn = 10;
    public int PapersToSpawn { get { return papersToSpawn; } }

    private List<GameObject> papers = new List<GameObject>();
    private List<GameObject> spawnLocations = new List<GameObject>();
    [SerializeField] private Texture[] textures;
    [SerializeField] private Sprite[] sprites;
    public Sprite[] Sprites { get { return sprites; } }
    [SerializeField] private Material mat;

    public List<GameObject> Papers { get { return papers; } }

    [SerializeField] private GameObject paperPanel;
    public GameObject PaperPanel { get { return paperPanel; } }

    [SerializeField] private GameObject playerPanel;
    public GameObject PlayerPanel { get { return playerPanel; } }

    public bool WonGame { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        //door = GameObject.FindGameObjectWithTag("EndDoor").transform;
        //door.GetComponent<Animator>().speed = 0;
        spawnLocations.AddRange(GameObject.FindGameObjectsWithTag("PaperSpawn"));

        for (int i = 0; i < papersToSpawn; i++)
        {
            int rand = Random.Range(0, spawnLocations.Count);

            GameObject paper = Instantiate(paperPref, spawnLocations[rand].transform.position, Quaternion.identity);
            Material newMat = new Material(mat);
            newMat.mainTexture = textures[i];
            paper.GetComponent<MeshRenderer>().material = newMat;
            PaperPickUp pickUp = paper.GetComponentInChildren<PaperPickUp>();
            pickUp.spawner = GetComponent<PaperSpawner>();
            pickUp.PaperSprite = sprites[i];
            pickUp.Id = i + 1;
            papers.Add(paper);
            spawnLocations.RemoveAt(rand);
        }        
    }

    public void Win()
    {
        Debug.Log("YOU WIN!!!");
        WonGame = true;
    }
}
