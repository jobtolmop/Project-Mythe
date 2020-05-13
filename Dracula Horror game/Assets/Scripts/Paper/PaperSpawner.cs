using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperSpawner : MonoBehaviour
{
    [SerializeField] private GameObject paperPref;
    [SerializeField] private Transform door;

    [SerializeField] private float distanceBetweenPapers = 40;
    [SerializeField] private int papersToSpawn = 10;

    private List<GameObject> papers = new List<GameObject>();
    private List<Transform> spawnLocations = new List<Transform>();

    public List<GameObject> Papers { get { return papers; } }

    private bool won = false;

    // Start is called before the first frame update
    void Start()
    {
        door.GetComponent<Animator>().speed = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnLocations.Add(transform.GetChild(i));
        }

        for (int i = 0; i < papersToSpawn; i++)
        {
            int rand = Random.Range(0, spawnLocations.Count);

            GameObject paper = Instantiate(paperPref, spawnLocations[rand].position, Quaternion.identity);
            paper.GetComponentInChildren<PaperPickUp>().spawner = GetComponent<PaperSpawner>();
            papers.Add(paper);
            spawnLocations.RemoveAt(rand);
        }        
    }

    public void Win()
    {
        Debug.Log("YOU WIN!!!");
        won = true;
        door.GetComponent<Animator>().speed = 1;
    }
}
