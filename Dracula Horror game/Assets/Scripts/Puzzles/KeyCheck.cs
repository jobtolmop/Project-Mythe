using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCheck : MonoBehaviour
{
    [SerializeField] private Rigidbody leftRb;
    [SerializeField] private Rigidbody rightRb;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Key") && other.transform.parent.gameObject.layer == 11)
        {
            leftRb.isKinematic = false;
            rightRb.isKinematic = false;
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();            
        }
    }
}
