using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private Transform leftDoor;
    [SerializeField] private Transform rightDoor;
    [SerializeField] private AudioSource doorBash;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject text;
    private EnemyPathFinding enemy;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetButton("Submit"))
        {
            anim.speed = 20;
        }
        else
        {
            anim.speed = 1;
        }
    }

    public void FreezeGame()
    {
        enemy = FindObjectOfType<EnemyPathFinding>();
        enemy.CantMove = true;
    }

    public void CloseDoor()
    {
        leftDoor.localRotation = new Quaternion(leftDoor.localRotation.x, 0, leftDoor.localRotation.z, leftDoor.localRotation.w);
        rightDoor.localRotation = new Quaternion(rightDoor.localRotation.x, 0, rightDoor.localRotation.z, rightDoor.localRotation.w);
        doorBash.Play();
    }

    public void EndCutscene()
    {
        enemy.CantMove = false;        
        GetComponent<PlayerMovement>().CantMove = false;
        GetComponentInChildren<FootStepAudio>().enabled = true;
        Destroy(anim);
        Destroy(text);
        Destroy(this);
    }
}
