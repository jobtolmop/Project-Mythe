using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrush : MonoBehaviour
{
    public bool PlayerAgainstWall { get; set; } = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAgainstWall = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerAgainstWall = false;
        }
    }
}
