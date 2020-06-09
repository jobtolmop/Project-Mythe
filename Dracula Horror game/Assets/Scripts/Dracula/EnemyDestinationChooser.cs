using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestinationChooser : MonoBehaviour
{
    public Vector3 TargetPos { get; set; }
    public bool DontGoAfterPlayer { get; set; } = true;

    private EnemyPlayerSpotter spotter;
    private EnemyPathFinding pathFinding;

    [SerializeField] private bool alreadyInvoking = false;
    public BoxCollider DoorTrigger { get; set; }

    private float standStillTimer = 0;
    private float tooFarTimer = 0;
    private float tooFarSec = 5;

    public enum state { RANDOM, HEARDSOUND, SEARCHLASTSEEN, GOTOWARDSPLAYER}

    [SerializeField] private state enemyState = state.RANDOM;

    public state EnemyState { get { return enemyState; } set { enemyState = value; } }

    private Collider ground;

    // Start is called before the first frame update
    void Start()
    {
        spotter = GetComponent<EnemyPlayerSpotter>();
        pathFinding = GetComponent<EnemyPathFinding>();
        Invoke("ChooseRandomLocation", 2);
        alreadyInvoking = true;
        tooFarSec = Random.Range(2, 6);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((spotter.Player.position - transform.position).sqrMagnitude);        

        Debug.DrawRay(TargetPos, Vector3.up, Color.blue);
        if (spotter.PlayerSpotted)
        {
            TargetPos = spotter.Player.position;
            enemyState = state.RANDOM;
            standStillTimer = 0;
            alreadyInvoking = false;
        }   
        else
        {
            if ((spotter.Player.position - transform.position).sqrMagnitude > 1500 && pathFinding.Agent.velocity.magnitude > 2 && enemyState != state.GOTOWARDSPLAYER && !DontGoAfterPlayer)
            {
                tooFarTimer += Time.deltaTime;

                if (tooFarTimer > tooFarSec)
                {
                    GoTowardsPlayer();
                }
            }

            Vector3 posCheck = transform.position;
            posCheck.y = 0;

            if (pathFinding.Agent.velocity.magnitude < 2 && posCheck != TargetPos && DoorTrigger == null)
            {
                standStillTimer += Time.deltaTime;
                Debug.Log("standing here...");

                if (standStillTimer > 5)
                {
                    MakeNewRandomLocation();
                    enemyState = state.RANDOM;
                }
            }
            else
            {
                standStillTimer = 0;
            }

            if (enemyState == state.RANDOM)
            {
                if (posCheck == TargetPos && !alreadyInvoking)
                {
                    MakeNewRandomLocation();
                }
            }
            else
            {
                if (posCheck == TargetPos)
                {
                    standStillTimer = 0;
                    enemyState = state.RANDOM;
                    alreadyInvoking = false;
                }
            }

            //Check if target pos is the location of a sound and if he is on that location
            /*if (posCheck == TargetPos && heardSound && !SearchLastPlayerLocation && !goCloserToPlayer)
            {
                standStillTimer = 0;
                heardSound = false;
                locationMadeByRandom = false;
                alreadyInvoking = false;
            }
            //Check if target pos is the location of a sound and if he is on that location
            else if (posCheck == TargetPos && SearchLastPlayerLocation && !goCloserToPlayer)
            {
                standStillTimer = 0;
                SearchLastPlayerLocation = false;
                locationMadeByRandom = false;
                alreadyInvoking = false;
            }
            //If he is on the pos when he wanted to go closer to the player
            else if (goCloserToPlayer && posCheck == TargetPos)
            {
                goCloserToPlayer = false;
                standStillTimer = 0;
            }
            //Check if target pos is made by random and if he is on that location
            else if ((!locationMadeByRandom || posCheck == TargetPos && locationMadeByRandom) && !alreadyInvoking)
            {
                standStillTimer = 0;
                Invoke("ChooseRandomLocation", 2);
                alreadyInvoking = true;
            }*/
        }
    }

    private void MakeNewRandomLocation()
    {
        standStillTimer = 0;
        Invoke("ChooseRandomLocation", 2);
        alreadyInvoking = true;
    }

    private void GoTowardsPlayer()
    {
        RaycastHit groundHit;

        int layer = LayerMask.GetMask("Ground");

        if (Physics.Raycast(spotter.Player.position, Vector3.down, out groundHit, 100, layer))
        {
            Debug.Log(groundHit.collider.tag);
            if (groundHit.collider.CompareTag("PuzzleGround"))
            {
                tooFarTimer = 0;
                return;
            }            
        }

        standStillTimer = 0;
        tooFarTimer = 0;
        tooFarSec = Random.Range(4, 9);
        /*goCloserToPlayer = true;
        locationMadeByRandom = false;
        standStillTimer = 0;
        alreadyInvoking = false;
        heardSound = false;
        SearchLastPlayerLocation = false;*/
        alreadyInvoking = false;
        enemyState = state.GOTOWARDSPLAYER;
        Vector3 playerPos = spotter.Player.position;
        playerPos.y = 0;
        TargetPos = playerPos;
    }

    private void ChooseRandomLocation()
    {
        RaycastHit groundHit;
        
        int layer = LayerMask.GetMask("Ground");
        float x = 0;
        float z = 0;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 100, layer))
        {
            ground = groundHit.collider;

            Debug.Log(ground.gameObject);

            x = ground.bounds.extents.x;
            z = ground.bounds.extents.z;
        }
        else
        {
            if (ground == null)
            {
                return;
            }            
        }

        Vector3 maybeTargetPos = new Vector3(ground.transform.position.x + Random.Range(-x + 2, x - 2), 0, ground.transform.position.z + Random.Range(-z + 2, z - 2));

        RaycastHit hit;

        int layerCheck = LayerMask.GetMask("Ground") | LayerMask.GetMask("DontDetectGround");
        layerCheck = ~layerCheck;

        if (Physics.Raycast(maybeTargetPos, Vector3.up, out hit, transform.localScale.y, layerCheck))
        {
            if (hit.collider != null)
            {
                alreadyInvoking = false;
                Debug.Log("Something is there " + hit.collider.gameObject);
                return;
            }
        }
        else
        {
            if (!Physics.CheckSphere(maybeTargetPos, 0.5f, layerCheck))
            {
                TargetPos = maybeTargetPos;
                alreadyInvoking = false;
            }   
            else
            {
                alreadyInvoking = false;
                Debug.Log("Someting is nearby the target pos");
            }
        }
    }

    public void HeardSound(Vector3 pos, float radius)
    {
        if (DontGoAfterPlayer)
        {
            return;
        }

        //AudioManager.instance.StopPlaying("HeartBeat");
        if (!AudioManager.instance.FindSound("HeartBeat").source.isPlaying)
        {
            AudioManager.instance.Play("HeartBeat");
        }

        enemyState = state.HEARDSOUND;

        pos.y = 0;
        TargetPos = pos;        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(TargetPos, 0.5f);
    }
}
