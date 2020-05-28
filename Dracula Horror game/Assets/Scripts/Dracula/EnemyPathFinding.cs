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
    [SerializeField] private FootStepAudio footSteps;
    [SerializeField] private float walkSpeed = 3;
    [SerializeField] private float hearRunSpeed = 6;
    [SerializeField] private float runSpeed = 12;
    [SerializeField] private float acceleration = 3;

    private bool alreadyPlayingFootSteps = false;

    public bool CantMove { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        chooser = GetComponent<EnemyDestinationChooser>();
        spotter = GetComponent<EnemyPlayerSpotter>();
        player = spotter.Player;
        footSteps = GetComponentInChildren<FootStepAudio>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spotter.PlayerSpotted)
        {
            if (CantMove)
            {
                agent.velocity = Vector3.zero;
                return;
            }

            if (agent.speed < runSpeed)
            {
                agent.speed += 3 * Time.deltaTime;
            }

            //if (agent.velocity.magnitude > 0.1f)
            //{
            //    footSteps.StartStepping(0.2f, 1);
            //}

            footSteps.StepLoop(agent.velocity.magnitude, 1.9f, 1);

            agent.angularSpeed = 690;
            agent.acceleration = 10.5f;
            agent.autoBraking = false;
        }
        else
        {
            if (chooser.HeardSoundBool)
            {
                //if (agent.velocity.magnitude > 0.1f)
                //{ 
                //    footSteps.StartStepping(0.4f, 0.9f);
                //}

                footSteps.StepLoop(agent.velocity.magnitude, 1.3f, 0.9f);

                agent.speed = hearRunSpeed;
            }
            else
            {
                /*if (agent.velocity.magnitude > 0.1f)
                {
                    footSteps.StartStepping(0.7f, 0.8f);
                }*/
                footSteps.StepLoop(agent.velocity.magnitude, 1f, 0.8f);
                agent.speed = walkSpeed;
            }
                
            agent.angularSpeed = 120;
            agent.acceleration = 8;
            agent.autoBraking = true;
        }

        agent.SetDestination(chooser.TargetPos);

        //Debug.Log(agent.speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12 && agent.velocity.magnitude > 7)
        {
            if (agent.speed < 8)
            {
                if (agent.speed > 0)
                {
                    agent.speed -= 1;
                }                
            }
            else
            {
                agent.speed = 7;
            }            
        }
    }
}
