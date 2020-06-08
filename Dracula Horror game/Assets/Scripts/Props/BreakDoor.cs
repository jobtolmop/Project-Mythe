using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//For now he instabtly breaks the door
public class BreakDoor : MonoBehaviour
{
    [SerializeField] private float health = 3;
    [SerializeField] private bool explodingDoor = false;

    private bool breaking = false;

    private Quaternion neutralPos;

    private bool goodRotation = false;
    private Transform enemy;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private NavMeshObstacle obstacle;
    private EnemyDestinationChooser enemyDestinationChooser;
    [SerializeField] private BoxCollider trigger;
    [SerializeField] private GameObject door;
    [SerializeField] private SoundEffectProp sfx;
    [SerializeField] private Transform model;

    private void Start()
    {
        obstacle.carving = false;
        neutralPos = rb.transform.rotation;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        enemyDestinationChooser = enemy.GetComponent<EnemyDestinationChooser>();
    }

    private void Update()
    {
        if (Math.Round(rb.transform.rotation.y, 1) == Math.Round(neutralPos.y, 1))
        {
            goodRotation = true;            
        }           
        else
        {
            goodRotation = false;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy") && goodRotation)
        {
            obstacle.carving = true;
            rb.isKinematic = true;

            //enemy = collision.transform;
            //enemyDestinationChooser = enemy.GetComponent<EnemyDestinationChooser>();
            collision.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (goodRotation)
            {
                enemyDestinationChooser.DoorTrigger = trigger;
                //collision.gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;

                if (!breaking)
                {
                    StartCoroutine("WaitDoorBreak");
                }
            }
            else
            {
                enemyDestinationChooser.DoorTrigger = null;
            }                     
        }        
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            obstacle.carving = false;
            enemyDestinationChooser.DoorTrigger = null;       
        }
    }

    private IEnumerator WaitDoorBreak()
    {
        rb.isKinematic = true;

        breaking = true;
        yield return new WaitForSeconds(1);
        health -= 1;
        sfx.HitSFX.Play();

        if (health <= 0)
        {
            sfx.CrashSFX.Play();
            Destroy(sfx.SoundCollider);
            //gameObject.layer = 18;
            //transform.GetChild(0).gameObject.layer = 18;
            //Destroy(GetComponent<HingeJoint>());
            //transform.GetChild(0).GetComponent<BoxCollider>().size = new Vector3(2f, 3f, 0.26f);
            //rb.velocity = enemy.forward * 10;
            //Destroy(trigger);
            //enemyDestinationChooser.DoorTrigger = null;
            //Destroy(obstacle);
            obstacle.carving = false;

            if (explodingDoor)
            {
                if (model.GetComponent<BoxCollider>() == null)
                {
                    model.GetComponentInParent<BoxCollider>().enabled = false;
                }
                else
                {
                    model.GetComponent<BoxCollider>().enabled = false;
                }                

                for (int i = 0; i < model.transform.childCount; i++)
                {
                    Transform child = model.transform.GetChild(i);
                    if (child.GetComponent<MeshCollider>() != null)
                    {
                        Destroy(child.GetComponent<MeshCollider>());
                    }

                    child.gameObject.AddComponent<Rigidbody>();
                    child.gameObject.AddComponent<BoxCollider>();
                    child.GetComponent<Rigidbody>().mass = 3f;
                    Destroy(child.gameObject, 10);                    
                }
                Destroy(obstacle);
                Destroy(trigger);
                Destroy(gameObject, 10);
            }      
        }
        else
        {
            breaking = false;
        }        
    }
}
