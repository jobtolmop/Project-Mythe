using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleControls : MonoBehaviour
{
    [SerializeField] private GameObject candle;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Blow"))
        {
            candle.SetActive(!candle.activeSelf);
        }
    }
}
