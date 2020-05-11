using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerSpotter : MonoBehaviour
{
    public bool PlayerSpotted { get; set; } = false;

    private float sightTimer = 0;

    private Transform player;
    [SerializeField] private Transform playerCandle;

    public Transform Player { get { return player; } }

    private EnemyDestinationChooser chooser;

    [SerializeField] private float viewDistance = 60;
    [SerializeField] private float feelDistance = 5;
    [SerializeField] private float fov = 105;
    [SerializeField] private Transform eyes;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        chooser = GetComponent<EnemyDestinationChooser>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCandle.gameObject.activeSelf)
        {
            ObjectInSightCheck(playerCandle);
        }        
        ObjectInSightCheck(player);
    }

    private void ObjectInSightCheck(Transform thingToSee)
    {
        if ((thingToSee.position - eyes.position).sqrMagnitude < viewDistance)
        {
            Vector3 dirToObject = (thingToSee.position - eyes.position).normalized;

            if (Vector3.Angle(eyes.forward, dirToObject) < fov / 2)
            {
                //Debug.Log("Player in field of view");
                RaycastHit hit;

                int layer = ~LayerMask.GetMask("Enemy");

                if (Physics.Raycast(eyes.position, dirToObject, out hit, viewDistance, layer))
                {
                    if (hit.collider != null && (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Candle")))
                    {
                        //Only applies if looking for light
                        bool seesLight = true;

                        if (hit.collider.gameObject.CompareTag("Candle"))
                        {
                            seesLight = PointSeesCandlePos(hit.point);
                            if (seesLight)
                            {
                                Debug.Log("Saw light!");
                            }                            
                        }

                        if (seesLight)
                        {
                            Debug.DrawRay(eyes.position, dirToObject, Color.red);
                            PlayerSpotted = true;
                            sightTimer = 0;
                            return;
                        }                                   
                    }
                }
            }
        }

        if ((player.position - transform.position).sqrMagnitude < feelDistance)
        {
            PlayerSpotted = true;
            sightTimer = 0;
            return;
        }

        Debug.DrawRay(eyes.position, transform.forward, Color.green);

        if (sightTimer < 5)
        {
            sightTimer += Time.deltaTime;
        }
        else
        {
            sightTimer = 0;
            PlayerSpotted = false;
        }
    }

    public bool PointSeesCandlePos(Vector3 pos)
    {
        int layer = ~LayerMask.GetMask("Candle") | ~LayerMask.GetMask("Enemy");
        RaycastHit candleHit;
        Vector3 dirToCandle = (playerCandle.GetChild(0).position - pos).normalized;

        Debug.DrawRay(pos, dirToCandle, Color.magenta);

        if (Physics.Raycast(pos, dirToCandle, out candleHit, 100, layer))
        {
            if (candleHit.collider != null && candleHit.collider.gameObject.CompareTag("CandleCol"))
            {
                return true;
            }
        }

        return false;
    }

    public void SpottedPlayer()
    {
        PlayerSpotted = true;
        sightTimer = 0;
    }
}
