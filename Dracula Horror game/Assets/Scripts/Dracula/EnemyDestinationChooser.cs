using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestinationChooser : MonoBehaviour
{
    public Vector3 TargetPos { get; set; }

    private EnemyPlayerSpotter spotter;
    private EnemyPathFinding pathFinding;

    //[SerializeField] private LayerMask layerMask;

    private bool alreadyInvoking = false;
    private bool locationMadeByRandom = false;

    public bool SearchLastPlayerLocation { get; set; } = false;

    private bool heardSound = false;
    public bool HeardSoundBool { get { return heardSound; } }
    public BoxCollider DoorTrigger { get; set; }

    private float standStillTimer = 0;
    private float tooFarTimer = 0;
    private float tooFarSec = 5;
    private bool goCloserToPlayer = false;
    public bool GoCloserToPlayer { get { return goCloserToPlayer; } }

    private Collider ground;

    // Start is called before the first frame update
    void Start()
    {
        spotter = GetComponent<EnemyPlayerSpotter>();
        pathFinding = GetComponent<EnemyPathFinding>();
        TargetPos = transform.position;
        tooFarSec = Random.Range(2, 6);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log((spotter.Player.position - transform.position).sqrMagnitude);
        if ((spotter.Player.position - transform.position).sqrMagnitude > 1500 && !goCloserToPlayer)
        {
            tooFarTimer += Time.deltaTime;

            if (tooFarTimer > tooFarSec)
            {
                GoTowardsPlayer();          
            }            
        }

        Debug.DrawRay(TargetPos, Vector3.up, Color.blue);
        if (spotter.PlayerSpotted)
        {
            TargetPos = spotter.Player.position;
            locationMadeByRandom = false;
            standStillTimer = 0;
            alreadyInvoking = false;
            heardSound = false;
            SearchLastPlayerLocation = false;
        }   
        else
        {
            Vector3 posCheck = transform.position;
            posCheck.y = 0;

            if (pathFinding.Agent.velocity.magnitude < 2 && posCheck != TargetPos && DoorTrigger == null)
            {
                standStillTimer += Time.deltaTime;
                Debug.Log("standing here...");

                if (standStillTimer > 5)
                {
                    Debug.Log("Choose new location...");
                    heardSound = false;
                    standStillTimer = 0;
                    goCloserToPlayer = false;
                    tooFarTimer = 0;
                    Invoke("ChooseRandomLocation", 2);
                    alreadyInvoking = true;
                }
            }
            else
            {
                standStillTimer = 0;
            }

            
            if ((!locationMadeByRandom || posCheck == TargetPos && locationMadeByRandom) && !alreadyInvoking && !heardSound && !SearchLastPlayerLocation && !goCloserToPlayer)
            {
                standStillTimer = 0;
                Invoke("ChooseRandomLocation", 2);
                alreadyInvoking = true;
            }
            else if (posCheck == TargetPos && heardSound && !SearchLastPlayerLocation && !goCloserToPlayer)
            {
                standStillTimer = 0;
                heardSound = false;
                locationMadeByRandom = false;
                alreadyInvoking = false;
            }
            else if (posCheck == TargetPos && SearchLastPlayerLocation && !goCloserToPlayer)
            {
                standStillTimer = 0;
                SearchLastPlayerLocation = false;
                locationMadeByRandom = false;
                alreadyInvoking = false;
            }
            else if (goCloserToPlayer && posCheck == TargetPos)
            {
                goCloserToPlayer = false;
                standStillTimer = 0;
            }
        }
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

        tooFarTimer = 0;
        tooFarSec = Random.Range(4, 9);
        goCloserToPlayer = true;
        locationMadeByRandom = false;
        standStillTimer = 0;
        alreadyInvoking = false;
        heardSound = false;
        SearchLastPlayerLocation = false;
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
                locationMadeByRandom = false;
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
                locationMadeByRandom = true;
            }   
            else
            {
                locationMadeByRandom = false;
                alreadyInvoking = false;
                Debug.Log("Someting is nearby the target pos");
            }
        }
    }

    public void HeardSound(Vector3 pos, float radius)
    {
        //AudioManager.instance.StopPlaying("HeartBeat");
        if (!AudioManager.instance.FindSound("HeartBeat").source.isPlaying)
        {
            AudioManager.instance.Play("HeartBeat");
        }
        
        heardSound = true;
        pos.y = 0;
        TargetPos = pos;        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(TargetPos, 0.5f);
    }
}
