using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathFinding : MonoBehaviour
{
    private NavMeshAgent agent;

    private EnemyDestinationChooser chooser;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        chooser = GetComponent<EnemyDestinationChooser>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(chooser.TargetPos);
    }
}
