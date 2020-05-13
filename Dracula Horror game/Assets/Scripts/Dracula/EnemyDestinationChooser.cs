using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDestinationChooser : MonoBehaviour
{
    public Vector3 TargetPos { get; set; }

    private EnemyPlayerSpotter spotter;
    //private EnemyPathFinding pathFinding;

    [SerializeField] private LayerMask layerMask;

    private bool alreadyInvoking = false;
    private bool locationMadeByRandom = false;

    private bool heardSound = false;

    // Start is called before the first frame update
    void Start()
    {
        spotter = GetComponent<EnemyPlayerSpotter>();
        //pathFinding = GetComponent<EnemyPathFinding>();
        TargetPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(TargetPos, Vector3.up, Color.blue);
        if (spotter.PlayerSpotted)
        {
            TargetPos = spotter.Player.position;
            locationMadeByRandom = false;
        }   
        else
        {
            Vector3 posCheck = transform.position;
            posCheck.y = 0;
            if ((!locationMadeByRandom || posCheck == TargetPos && locationMadeByRandom) && !alreadyInvoking && !heardSound)
            {                
                Invoke("ChooseRandomLocation", 2);
                alreadyInvoking = true;
            }
            else if (posCheck == TargetPos && heardSound)
            {
                heardSound = false;
            }
        }
    }

    private void ChooseRandomLocation()
    {
        RaycastHit groundHit;
        
        int layer = ~LayerMask.GetMask("Enemy");
        float x = 0;
        float z = 0;
        if (Physics.Raycast(transform.position, Vector3.down, out groundHit, 100, layer))
        {
            if (groundHit.collider != null && groundHit.collider.gameObject.layer == 10)
            {
                Collider ground = groundHit.collider;

                x = ground.bounds.extents.x;
                z = ground.bounds.extents.z;
            }
        }
        else
        {
            return;
        }

        Vector3 maybeTargetPos = new Vector3(Random.Range(-x + 2, x - 2), 0, Random.Range(-z + 2, z - 2));

        RaycastHit hit;

        int layerCheck = ~LayerMask.GetMask("Ground");
        if (Physics.Raycast(maybeTargetPos, Vector3.up, out hit, transform.localScale.y * 10, layerCheck))
        {
            if (hit.collider != null)
            {
                locationMadeByRandom = false;
                alreadyInvoking = false;
                Debug.Log("Something is there");
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

    public void HeardSound(Vector3 pos)
    {
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
