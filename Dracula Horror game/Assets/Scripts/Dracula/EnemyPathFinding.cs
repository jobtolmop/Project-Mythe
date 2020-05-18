using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathFinding : MonoBehaviour
{
    private NavMeshAgent agent;

    private EnemyDestinationChooser chooser;
    private EnemyPlayerSpotter spotter;

    private Transform player;
    [SerializeField] private AudioSource footSteps;

    private bool alreadyPlayingFootSteps = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        chooser = GetComponent<EnemyDestinationChooser>();
        spotter = GetComponent<EnemyPlayerSpotter>();
        player = spotter.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (spotter.PlayerSpotted)
        {
            agent.speed = 14;
            agent.angularSpeed = 690;
            agent.acceleration = 10.5f;
        }
        else
        {
            agent.speed = 3;
            agent.angularSpeed = 120;
            agent.acceleration = 8;
        }

        agent.SetDestination(chooser.TargetPos);

        //Debug.Log(agent.velocity.magnitude);

        if (agent.velocity.magnitude > 1 && !alreadyPlayingFootSteps)
        {
            footSteps.loop = true;
            footSteps.Play();
            alreadyPlayingFootSteps = true;
        }
        else if (agent.velocity.magnitude <= 1 && alreadyPlayingFootSteps)
        {
            footSteps.loop = false;
            alreadyPlayingFootSteps = false;
        }
    }
}
