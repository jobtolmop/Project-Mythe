using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderTouch : MonoBehaviour
{
    private EnemyPlayerSpotter spotter;
    private EnemyDestinationChooser chooser;

    // Start is called before the first frame update
    void Start()
    {
        spotter = GetComponentInParent<EnemyPlayerSpotter>();
        chooser = GetComponentInParent<EnemyDestinationChooser>();
    }

    private void OnTriggerStay(Collider other)
    {        
        if (other.CompareTag("Candle") && spotter.PointSeesCandlePos(transform.position) && !spotter.PlayerSpotted)
        {
            Debug.Log("Light touches me");
            spotter.SpottedPlayer();
        }
        else if(other.CompareTag("Sound") && !spotter.PlayerSpotted)
        {
            chooser.HeardSound(other.transform.position, other.GetComponent<SphereCollider>().radius);
            Debug.Log("Heard Sound!!");
        }
    }
}
