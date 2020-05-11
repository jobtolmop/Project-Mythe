using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTouch : MonoBehaviour
{
    private EnemyPlayerSpotter spotter;

    // Start is called before the first frame update
    void Start()
    {
        spotter = GetComponentInParent<EnemyPlayerSpotter>();
    }

    private void OnTriggerStay(Collider other)
    {        
        if (other.CompareTag("Candle") && spotter.PointSeesCandlePos(transform.position) && !spotter.PlayerSpotted)
        {
            Debug.Log("Light touches me");
            spotter.SpottedPlayer();
        }
    }
}
