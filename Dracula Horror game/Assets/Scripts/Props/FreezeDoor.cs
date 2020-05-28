using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//For now he instabtly breaks the door
public class FreezeDoor : MonoBehaviour
{
    private float health = 3;

    private bool breaking = false;

    private Quaternion neutralPos;

    private bool goodRotation = false;
    private Transform enemy;
    private Rigidbody rb;
    private NavMeshObstacle obstacle;
    [SerializeField] private BoxCollider trigger;

    private void Start()
    {
        neutralPos = transform.rotation;
        rb = GetComponent<Rigidbody>();
        obstacle = GetComponent<NavMeshObstacle>();
    }

    private void Update()
    {
        if (Math.Round(transform.rotation.y, 1) == Math.Round(neutralPos.y, 1))
        {
            goodRotation = true;
            if (obstacle != null)
            {
                obstacle.enabled = true;
            }
        }           
        else
        {
            if (obstacle != null)
            {
                obstacle.enabled = false;
            }            
            goodRotation = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy") && goodRotation)
        {
            rb.isKinematic = true;
            enemy = collision.transform;
            collision.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Enemy") && goodRotation)
        {
            collision.GetComponent<EnemyDestinationChooser>().InFrontOfDoor = true;
            //collision.gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

            if (!breaking)
            {
                StartCoroutine("WaitDoorBreak");
            }            
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyDestinationChooser>().InFrontOfDoor = false;       
        }
    }

    private IEnumerator WaitDoorBreak()
    {
        rb.isKinematic = true;
        breaking = true;
        yield return new WaitForSeconds(1);
        health -= 1;
        rb.isKinematic = false;
        if (health <= 0)
        {
            enemy.GetComponent<EnemyPathFinding>().CantMove = true;
            gameObject.layer = 18;
            transform.GetChild(0).gameObject.layer = 18;
            transform.GetChild(0).GetComponent<BoxCollider>().size = new Vector3(2f, 3.8f, 0.26f);
            rb.velocity = enemy.forward * 10;
            Destroy(trigger);
            Destroy(GetComponent<HingeJoint>());
            Destroy(obstacle);
            yield return new WaitForSeconds(2);
            enemy.GetComponent<EnemyPathFinding>().CantMove = false;
            Destroy(gameObject, 10);
        }

        breaking = false;        
    }
}
