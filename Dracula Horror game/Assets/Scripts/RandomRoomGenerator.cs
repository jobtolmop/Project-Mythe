using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomRoomGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> rooms = new List<GameObject>();
    [SerializeField] private List<GameObject> placedRooms = new List<GameObject>();
    [SerializeField] private Transform spawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        int count = rooms.Count;

        for (int i = 0; i < 8; i++)
        {
            int rand = 0;

            if (rooms.Count > 1)
            {
                rand = Random.Range(0, rooms.Count);
            }
            
            GameObject room = Instantiate(rooms[rand], spawnLocation, false);

            if (room.transform.GetChild(0).childCount == 0)
            {
                room.transform.localPosition -= room.transform.GetChild(0).localPosition;                
            }

            room.transform.SetParent(null);
            placedRooms.Add(room);            
            spawnLocation = room.transform.GetChild(1).GetChild(0);
            //rooms.Remove(rooms[rand]);
        }

        GetComponent<NavMeshSurface>().BuildNavMesh();
    }
}
