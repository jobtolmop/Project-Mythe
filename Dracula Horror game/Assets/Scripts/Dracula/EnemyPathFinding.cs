using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathFinding : MonoBehaviour
{
    private NavMeshAgent agent;

    public NavMeshAgent Agent { get { return agent; } }

    private EnemyDestinationChooser chooser;
    private EnemyPlayerSpotter spotter;

    private Transform player;
    [SerializeField] private AudioSource footSteps;
    [SerializeField] private float walkSpeed = 3;
    [SerializeField] private float hearRunSpeed = 6;
    [SerializeField] private float runSpeed = 12;
    [SerializeField] private float acceleration = 3;

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
            if (agent.speed < runSpeed)
            {
                agent.speed += 3 * Time.deltaTime;
            }

            agent.angularSpeed = 690;
            agent.acceleration = 10.5f;
            agent.autoBraking = false;
        }
        else
        {
            if (chooser.HeardSoundBool)
            {
                agent.speed = hearRunSpeed;
            }
            else
            {
                agent.speed = walkSpeed;
            }
                
            agent.angularSpeed = 120;
            agent.acceleration = 8;
            agent.autoBraking = true;
        }

        agent.SetDestination(chooser.TargetPos);

        //Debug.Log(agent.speed);

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12 && agent.velocity.magnitude > 7)
        {
            agent.speed = 7;
        }
    }
}
