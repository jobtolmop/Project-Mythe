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
        text.GetComponentInParent<Animator>().SetBool("cutscene", GameManager.instance.PlayCutscene);
        enemy = FindObjectOfType<EnemyPathFinding>();

        if (!GameManager.instance.PlayCutscene)
        {
            CloseDoor();
            EndCutscene();
        }
    }

    public void FreezeGame()
    {        
        enemy.CantMove = true;
        GetComponentInChildren<PlayerLook>().CantMove = true;
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
        StartCoroutine(GetComponent<DontHearStartingSounds>().HearSounds(1));
        GetComponentInChildren<FootStepAudio>().enabled = true;
        GetComponentInChildren<PlayerLook>().CantMove = false;
        Destroy(anim);
        Destroy(text);
    }
}
