using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRoomGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> rooms = new List<GameObject>();
    [SerializeField] private Transform spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < rooms.Count; i++)
        {
            GameObject room = Instantiate(rooms[i], spawnLocation.position - rooms[i].transform.GetChild(0).localPosition, Quaternion.identity);
            spawnLocation = room.transform.GetChild(1).GetChild(0);
        }        
    }
}
