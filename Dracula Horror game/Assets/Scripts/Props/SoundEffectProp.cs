using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectProp : MonoBehaviour
{
    [SerializeField] private AudioSource hitSFX;
    public AudioSource HitSFX { get { return hitSFX; } }
    [SerializeField] private AudioSource crashSFX;
    public AudioSource CrashSFX { get { return crashSFX; } }
    [SerializeField] private float hitVel;
    [SerializeField] private float crashVel;
    private AudioClip currSFX;
    private AudioSource currAudio;

    private AudioManager audioManager;

    [SerializeField] private SphereCollider soundCollider;
    public SphereCollider SoundCollider { get { return soundCollider; } }
    private Transform player;
    private Transform enemy;
    [SerializeField] private Transform model;

    public bool ThrownByPlayer { get; set; } = false;
    public bool DoorHold { get; set; } = false;

    private bool alreadyBreaking = false;

    private bool pushedByPlayer = false;

    private void Start()
    {
        //audioManager = GameObject.FindGameObjectWithTag("Audio").transform.GetChild(0).GetComponent<AudioManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //Debug.Log(soundCollider);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (DoorHold)
        {
            return;
        }

        if (player != null && (player.position - transform.position).sqrMagnitude < 5)
        {
            pushedByPlayer = true;
        }
        else
        {
            pushedByPlayer = false;
        }

        if (collision.relativeVelocity.magnitude > crashVel && !alreadyBreaking && (collision.gameObject.GetComponent<Rigidbody>() == null || collision.gameObject.GetComponent<Rigidbody>().mass > 10f))
        {
            alreadyBreaking = true;
            currSFX = crashSFX.clip;
            currAudio = crashSFX;
            crashSFX.Play();
            if (pushedByPlayer || ThrownByPlayer)
            {
                StartCoroutine("SoundDelay");
            }
            
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

            if (model.childCount > 0)
            {
                model.GetComponent<BoxCollider>().enabled = false;

                for (int i = 0; i < model.childCount; i++)
                {
                    model.GetChild(i).GetComponent<BoxCollider>().enabled = true;
                    model.GetChild(i).gameObject.AddComponent<Rigidbody>();
                    model.GetChild(i).GetComponent<Rigidbody>().mass = 0.1f;
                    Destroy(model.GetChild(i).gameObject, 10);
                }
            }

            Destroy(gameObject, 10);
        }
        else if(collision.relativeVelocity.magnitude > hitVel)
        {
            currSFX = hitSFX.clip;
            currAudio = hitSFX;
            hitSFX.Play();
            if (pushedByPlayer || ThrownByPlayer)
            {
                StartCoroutine("SoundDelay");
            }
        }

        ThrownByPlayer = false;
    }

    private IEnumerator SoundDelay()
    {
        soundCollider.gameObject.SetActive(true);
        soundCollider.radius = currAudio.maxDistance;

        yield return new WaitForSeconds(currSFX.length);

        soundCollider.radius = 0.01f;
        soundCollider.gameObject.SetActive(false);
    }
}
