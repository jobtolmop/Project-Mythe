using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleControls : MonoBehaviour
{
    [SerializeField] private GameObject candle;
    [SerializeField] private float fadeRate;
    private ParticleSystem particles;
    private Light candleLight;

    private void Start()
    {
        candleLight = candle.GetComponent<Light>();
        particles = candle.GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Blow"))
        {
            StopAllCoroutines();

            if (candle.activeSelf)
            {
                StartCoroutine("LightOff");
            }
            else
            {
                StartCoroutine("LightOn");
            }
        }
    }

    private IEnumerator LightOff()
    {
        particles.Stop(true);

        while (candleLight.intensity > 0)
        {
            candleLight.intensity -= fadeRate;
            yield return null;
        }

        candleLight.intensity = 0;

        candle.SetActive(false);
    }

    private IEnumerator LightOn()
    {
        candle.SetActive(true);
        particles.Play(true);

        while (candleLight.intensity < 2)
        {
            candleLight.intensity += fadeRate;
            yield return null;
        }

        candleLight.intensity = 2;        
    }
}
