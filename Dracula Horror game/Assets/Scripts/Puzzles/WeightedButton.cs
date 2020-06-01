using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeightedButton : MonoBehaviour
{
    private bool buttonPressed = false;
    public bool ButtonPressed { get { return buttonPressed; } }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            buttonPressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Button"))
        {
            buttonPressed = false;
        }
    }
}
