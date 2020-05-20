using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectProp : MonoBehaviour
{
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource crashSFX;
    [SerializeField] private float hitVel;
    [SerializeField] private float crashVel;
    private AudioClip currSFX;
    private AudioSource currAudio;

    private AudioManager audioManager;

    private SphereCollider soundCollider;
    private Transform player;
    private Transform enemy;

    public bool ThrownByPlayer { get; set; } = false;

    private bool alreadyBreaking = false;

    private bool pushedByPlayer = false;

    private void Start()
    {
        //audioManager = GameObject.FindGameObjectWithTag("Audio").transform.GetChild(0).GetComponent<AudioManager>();
        soundCollider = transform.GetChild(1).GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        //Debug.Log(soundCollider);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (player != null && (player.position - transform.position).sqrMagnitude < 5)
        {
            pushedByPlayer = true;
        }
        else if (enemy != null && (enemy.position - transform.position).sqrMagnitude < 5)
        {
            pushedByPlayer = false;
        }

        if (collision.relativeVelocity.magnitude > crashVel && !alreadyBreaking && (collision.gameObject.GetComponent<Rigidbody>() == null || collision.gameObject.GetComponent<Rigidbody>().mass > 0.1f))
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
            Transform model = transform.GetChild(0);
            model.GetComponent<BoxCollider>().enabled = false;

            for (int i = 0; i < model.childCount; i++)
            {
                model.GetChild(i).GetComponent<BoxCollider>().enabled = true;
                model.GetChild(i).gameObject.AddComponent<Rigidbody>();
                model.GetChild(i).GetComponent<Rigidbody>().mass = 0.1f;
                Destroy(model.GetChild(i).gameObject, 10);
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

        soundCollider.radius = 0.1f;
        soundCollider.gameObject.SetActive(false);
    }
}
