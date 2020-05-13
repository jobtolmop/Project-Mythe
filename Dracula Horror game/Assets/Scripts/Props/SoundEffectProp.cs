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

    private bool alreadyBreaking = false;

    private void Start()
    {
        //audioManager = GameObject.FindGameObjectWithTag("Audio").transform.GetChild(0).GetComponent<AudioManager>();
        soundCollider = transform.GetChild(1).GetComponent<SphereCollider>();
        Debug.Log(soundCollider);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > crashVel && !alreadyBreaking)
        {
            alreadyBreaking = true;
            currSFX = crashSFX.clip;
            currAudio = crashSFX;
            crashSFX.Play();
            if (!collision.gameObject.CompareTag("Enemy"))
            {
                StartCoroutine("SoundDelay");
            }
            Destroy(transform.GetChild(0).gameObject);
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            Destroy(gameObject, crashSFX.clip.length);
        }
        else if(collision.relativeVelocity.magnitude > hitVel)
        {
            currSFX = hitSFX.clip;
            currAudio = hitSFX;
            hitSFX.Play();
            if (!collision.gameObject.CompareTag("Enemy"))
            {
                StartCoroutine("SoundDelay");
            }            
        }
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
