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
    private Animator anim;

    private Transform player;
    [SerializeField] private FootStepAudio footSteps;
    [SerializeField] private float walkSpeed = 3;
    [SerializeField] private float hearRunSpeed = 6;
    [SerializeField] private float closerRunSpeed = 8;
    [SerializeField] private float runSpeed = 12;
    [SerializeField] private float acceleration = 3;

    public bool CantMove { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        chooser = GetComponent<EnemyDestinationChooser>();
        spotter = GetComponent<EnemyPlayerSpotter>();
        player = spotter.Player;
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);

        if (transform.position.y > 10 || transform.position.y < -10 || agent.nextPosition.y > 10)
        {
            agent.Warp(new Vector3(transform.position.x, 1.8f, transform.position.z));
        }

        if (CantMove)
        {
            agent.velocity = Vector3.zero;
            return;
        }

        if (spotter.PlayerSpotted)
        {
            if (agent.speed < runSpeed)
            {
                agent.speed += acceleration * Time.deltaTime;
            }

            anim.speed = 2.5f;

            agent.angularSpeed = 690;
            agent.acceleration = 10.5f;
            agent.autoBraking = false;
        }
        else
        {
            if (chooser.EnemyState == EnemyDestinationChooser.state.HEARDSOUND)
            {
                anim.speed = 1.6f;

                agent.speed = hearRunSpeed;
            }
            else if (chooser.EnemyState == EnemyDestinationChooser.state.GOTOWARDSPLAYER)
            {
                if ((spotter.Player.position - transform.position).sqrMagnitude > 500)
                {
                    agent.speed = runSpeed + 5;
                }
                else
                {
                    agent.speed = closerRunSpeed;
                }

                anim.speed = 1.6f;
            }
            else
            {
                agent.speed = walkSpeed;
                anim.speed = 0.8f;
            }
                
            agent.angularSpeed = 120;
            agent.acceleration = 8;
            agent.autoBraking = true;
        }

        if (agent.velocity.magnitude < 0.01f)
        {
            anim.speed = 1;
        }

        agent.SetDestination(chooser.TargetPos);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12 && agent.velocity.magnitude > 7)
        {
            float colMass = collision.gameObject.GetComponent<Rigidbody>().mass / 10 / 2;
           
            if (agent.speed > 3)
            {
                agent.speed -= colMass;
            }             
        }
    }
}
