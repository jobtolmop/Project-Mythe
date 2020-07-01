using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DontHearStartingSounds : MonoBehaviour
{
    [SerializeField] private AudioSource landingSfx;

    // Start is called before the first frame update
    void Start()
    {
        landingSfx.mute = true;
        StartCoroutine(HearSounds(1.5f));
    }

    public IEnumerator HearSounds(float time)
    {
        yield return new WaitForSeconds(time);
        
        landingSfx.mute = false;      
    }
}
