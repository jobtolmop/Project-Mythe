using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomRoomGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> rooms = new List<GameObject>();
    [SerializeField] private List<GameObject> placedRooms = new List<GameObject>();
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private GameObject endRoom;

    // Start is called before the first frame update
    void Start()
    {
        int count = rooms.Count;

        for (int i = 0; i < count; i++)
        {
            int rand = 0;

            if (rooms.Count > 1)
            {
                rand = Random.Range(0, rooms.Count);
            }
            
            GameObject room = Instantiate(rooms[rand], spawnLocation, false);

            //room.transform.localRotation = spawnLocation.localRotation;

            room.transform.SetParent(null);
            placedRooms.Add(room);
            spawnLocation = room.transform.GetChild(1);
            rooms.Remove(rooms[rand]);
        }

        GameObject roomEnd = Instantiate(endRoom, spawnLocation, false);
        roomEnd.transform.SetParent(null);

        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
