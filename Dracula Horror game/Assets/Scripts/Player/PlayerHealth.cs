using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject physicsInteraction;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Die");
            transform.parent.gameObject.layer = 18;
            physicsInteraction.SetActive(false);
            transform.parent.gameObject.AddComponent<CapsuleCollider>();
            transform.parent.GetComponent<CapsuleCollider>().height = 2;
            transform.parent.gameObject.AddComponent<Rigidbody>();
            GetComponentInParent<Rigidbody>().AddForce(-transform.parent.forward * 10);
            GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
            transform.parent.GetComponentInChildren<PlayerLook>().enabled = false;
            GetComponentInParent<CharacterController>().enabled = false;
            GetComponentInParent<PlayerMovement>().enabled = false;
            GetComponentInParent<CandleControls>().enabled = false;
            Destroy(gameObject);
        }
    }
}
