using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowByCandle : MonoBehaviour
{
    private GameObject playerCandle;
    private Light pointLight;

    // Start is called before the first frame update
    void Start()
    {
        playerCandle = GameObject.FindGameObjectWithTag("Candle");
        pointLight = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerCandle.activeSelf)
        {
            pointLight.enabled = true;
        }
        else
        {
            pointLight.enabled = false;
        }
    }
}
