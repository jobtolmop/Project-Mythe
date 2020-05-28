using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperSpawner : MonoBehaviour
{
    [SerializeField] private GameObject paperPref;
    [SerializeField] private Transform door;
    
    [SerializeField] private int papersToSpawn = 10;

    private List<GameObject> papers = new List<GameObject>();
    private List<GameObject> spawnLocations = new List<GameObject>();

    public List<GameObject> Papers { get { return papers; } }

    // Start is called before the first frame update
    void Start()
    {
        spawnLocations.AddRange(GameObject.FindGameObjectsWithTag("PaperSpawn"));

        for (int i = 0; i < papersToSpawn; i++)
        {
            int rand = Random.Range(0, spawnLocations.Count);

            GameObject paper = Instantiate(paperPref, spawnLocations[rand].transform.position, Quaternion.identity);
            paper.GetComponentInChildren<PaperPickUp>().spawner = GetComponent<PaperSpawner>();
            papers.Add(paper);
            spawnLocations.RemoveAt(rand);
        }        
    }

    public void Win()
    {
        Debug.Log("YOU WIN!!!");
        //door.GetComponent<Animator>().speed = 1;
    }
}
