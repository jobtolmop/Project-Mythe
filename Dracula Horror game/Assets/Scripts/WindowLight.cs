using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowLight : MonoBehaviour
{
    private EnemyPlayerSpotter spotter;

    // Start is called before the first frame update
    void Start()
    {
        spotter = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyPlayerSpotter>();    
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.GetComponent<CharacterController>() != null)
        {
            spotter.PlayerInLight(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null)
        {
            spotter.PlayerInLight(false);
        }
    }
}
