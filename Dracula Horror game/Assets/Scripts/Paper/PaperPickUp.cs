using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperPickUp : MonoBehaviour
{
    public PaperSpawner spawner {get; set;}

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("PickUp") && other.CompareTag("Player"))
        {
            spawner.Papers.Remove(gameObject);

            if (spawner.Papers.Count <= 0)
            {
                spawner.Win();
            }

            Destroy(transform.parent.gameObject);
        }
    }
}
