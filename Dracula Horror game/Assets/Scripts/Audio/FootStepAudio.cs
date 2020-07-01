using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepAudio : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;

    private AudioSource source;
    public bool Grounded { get; set; } = true;
    public bool Dead { get; set; } = false;
    //private bool delayTimer = false;
    //private float currVolume = 1;
    //private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        if (clips.Length > 0)
        {
            source.enabled = true;
            source.playOnAwake = false;
            source.loop = false;
        }
    }

    private void Update()
    {
        if (!Grounded && clips.Length == 0 || Dead)
        {
            source.enabled = false;
        }
    }

    public void StepLoop(float velocity, float pitch, float volume)
    {
        if (velocity > 0.1f && Time.timeScale > 0)
        {
            source.volume = volume;
            source.pitch = Random.Range(pitch - 0.4f, pitch);
            source.enabled = true;
        }
        else
        {
            source.enabled = false;
        }        
    }

    public void Step()
    {
        int rand = Random.Range(0, clips.Length);

        source.pitch = Random.Range(0.8f, 1);
        source.PlayOneShot(clips[rand]);        
    }

    /*public void StartStepping(float delay, float volume)
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && !source.isPlaying)
        {
            timer = delay;
            Step();
        }
    }

    public void Step()
    {
        Debug.Log("STEP!!!");
        int rand = Random.Range(0, clips.Length);
        source.PlayOneShot(clips[rand], currVolume);
    }*/
}
