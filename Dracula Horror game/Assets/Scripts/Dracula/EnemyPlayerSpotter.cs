using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerSpotter : MonoBehaviour
{
    public bool PlayerSpotted { get; set; } = false;

    private float sightTimer = 0;

    private Transform player;
    private PlayerMovement playerMov;
    [SerializeField] private Transform playerCandle;

    public Transform Player { get { return player; } }

    private EnemyDestinationChooser chooser;

    [SerializeField] private float viewDistance = 60;
    [SerializeField] private float playerCrouchviewDistance = 100;
    [SerializeField] private float playerCrouchFov = 50;
    [SerializeField] private float feelDistance = 5;
    [SerializeField] private float fov = 105;
    [SerializeField] private Transform eyes;

    private bool playerInLight = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerMov = player.GetComponent<PlayerMovement>();
        chooser = GetComponent<EnemyDestinationChooser>();
    }

    // Update is called once per frame
    void Update()
    {
        float lessView = 0;
        float lessFov = 0;
        if (playerMov.Crouching)
        {
            lessView = playerCrouchviewDistance;
            lessFov = playerCrouchFov;
        }

        if (playerCandle.gameObject.activeSelf)
        {
            viewDistance = 500 - lessView;
            fov = 150 - lessFov;
            ObjectInSightCheck(playerCandle);
        }        
        else
        {
            fov = 120 - lessFov;

            if (!playerInLight)
            {
                viewDistance = 300 - lessView;
            }
            else
            {
                viewDistance = 500- lessView;
            }
           
            ObjectInSightCheck(player);
        }        
    }

    private void ObjectInSightCheck(Transform thingToSee)
    {
        if ((thingToSee.position - eyes.position).sqrMagnitude < viewDistance)
        {
            Vector3 dirToObject = (thingToSee.position - eyes.position).normalized;
            //Debug.Log(dirToObject);

            if (Vector3.Angle(eyes.forward, dirToObject) < fov / 2)
            {
                //Debug.Log("Player in field of view");
                RaycastHit hit;

                int layer = ~LayerMask.GetMask("Enemy") | ~LayerMask.GetMask("Window");

                if (Physics.Raycast(eyes.position, dirToObject, out hit, viewDistance, layer))
                {
                    if (hit.collider != null && (hit.collider.gameObject.CompareTag("Player") || hit.collider.gameObject.CompareTag("Candle")))
                    {
                        //Only applies if looking for light
                        bool seesLight = true;
                        Debug.Log(hit.collider);
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

                            if (!AudioManager.instance.PlayingSong)
                            {
                                AudioManager.instance.Play("Chase");
                                AudioManager.instance.Play("JumpScare");
                            }

                            AudioManager.instance.CurrSound.source.volume = AudioManager.instance.CurrSound.volume;
                            return;
                        }                                   
                    }
                }
            }
        }

        if ((player.position - transform.position).sqrMagnitude < feelDistance && player.gameObject.layer == 8)
        {
            PlayerSpotted = true;
            sightTimer = 0;
            return;
        }

        Color color = Color.green;

        if (PlayerSpotted)
        {
            color = Color.cyan;
        }

        Debug.DrawRay(eyes.position, transform.forward, color);

        if (sightTimer < 4)
        {
            sightTimer += Time.deltaTime;
        }
        else
        {
            sightTimer = 0;
            if (!chooser.SearchLastPlayerLocation && PlayerSpotted)
            {
                chooser.SearchLastPlayerLocation = true;
                chooser.TargetPos = new Vector3(player.position.x, 0, player.position.z);

                AudioManager.instance.FadeOut = true;
            }

            PlayerSpotted = false;                                        
        }
    }

    public bool PointSeesCandlePos(Vector3 pos)
    {
        int layer = ~LayerMask.GetMask("Candle") | ~LayerMask.GetMask("Enemy") | ~LayerMask.GetMask("Window");
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

    public void PlayerInLight(bool enter)
    {
        playerInLight = enter;
    }
}
