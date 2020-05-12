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
        if (chooser.TargetPos == player.position)
        {
            agent.speed = 7;
        }
        else
        {
            agent.speed = 3;
        }

        agent.SetDestination(chooser.TargetPos);
    }
}
