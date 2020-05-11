using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlayerSpotter : MonoBehaviour
{
    public bool PlayerSpotted { get; set; } = false;

    private bool checkSeePlayer = true;

    private Transform player;

    public Transform Player { get { return player; } }

    private EnemyDestinationChooser chooser;

    [SerializeField] private float viewDistance = 60;
    [SerializeField] private float fov = 105;

    private Vector3 dirToPlayerDebug;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        chooser = GetComponent<EnemyDestinationChooser>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerSpotted)
        {
            Debug.DrawRay(transform.position, dirToPlayerDebug, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.forward, Color.green);
        }

        if (checkSeePlayer && (player.position - transform.position).sqrMagnitude < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            dirToPlayerDebug = dirToPlayer;

            if (Vector3.Angle(transform.forward, dirToPlayer) < fov / 2)
            {
                //Debug.Log("Player in field of view");
                RaycastHit hit;                

                int layer = ~LayerMask.GetMask("Enemy");

                if (Physics.Raycast(transform.position, dirToPlayer, out hit, viewDistance, layer))
                {
                    if (hit.collider != null && hit.collider.gameObject.CompareTag("Player"))
                    {
                        Debug.Log("Player in sight");
                        PlayerSpotted = true;
                        checkSeePlayer = false;
                        StartCoroutine(CheckPlayerDelay());
                    }
                }
            }
        }

        if (checkSeePlayer)
        {
            PlayerSpotted = false;
        }
    }

    private IEnumerator CheckPlayerDelay()
    {
        yield return new WaitForSeconds(5);

        Debug.Log("Putting it back");

        checkSeePlayer = true;
    }
}
