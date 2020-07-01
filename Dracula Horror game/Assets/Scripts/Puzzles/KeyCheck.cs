using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCheck : MonoBehaviour
{
    [SerializeField] private Rigidbody leftRb;
    [SerializeField] private Rigidbody rightRb;
    [SerializeField] private BreakDoor doorBreakL;
    [SerializeField] private BreakDoor doorBreakR;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key") && other.transform.parent.gameObject.layer == 11)
        {
            leftRb.isKinematic = false;
            rightRb.isKinematic = false;
            gameObject.AddComponent<Rigidbody>();
            doorBreakL.Locked = false;
            doorBreakR.Locked = false;
        }
    }
}
