using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectProp : MonoBehaviour
{
    [SerializeField] private AudioSource hitSFX;
    [SerializeField] private AudioSource crashSFX;
    private AudioClip currSFX;
    private Transform currGameObject;

    private AudioManager audioManager;

    private SphereCollider soundCollider;

    private void Start()
    {
        //audioManager = GameObject.FindGameObjectWithTag("Audio").transform.GetChild(0).GetComponent<AudioManager>();
        soundCollider = transform.GetChild(0).GetComponent<SphereCollider>();
        Debug.Log(soundCollider);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 10)
        {
            currSFX = crashSFX.clip;
            currGameObject = crashSFX.transform;
            crashSFX.Play();
            if (!collision.gameObject.CompareTag("Enemy"))
            {
                StartCoroutine("SoundDelay");
            }
            Destroy(gameObject, crashSFX.clip.length);
        }
        else if(collision.relativeVelocity.magnitude > 2)
        {
            currSFX = hitSFX.clip;
            currGameObject = hitSFX.transform;
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
        soundCollider.radius = currGameObject.transform.localScale.x;

        yield return new WaitForSeconds(currSFX.length);

        soundCollider.radius = 0.1f;
        soundCollider.gameObject.SetActive(false);
    }
}
