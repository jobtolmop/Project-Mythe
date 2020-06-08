using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearedTurorial : MonoBehaviour
{
    private EnemyDestinationChooser chooser;

    // Start is called before the first frame update
    void Start()
    {
        chooser = FindObjectOfType<EnemyDestinationChooser>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
