using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    private float throwForce = 40000f;
    private Vector3 objectPos;
    private float distance;

    private bool thrown = false;
    [SerializeField] private GameObject item;
    private Rigidbody rb;
    [SerializeField] private GameObject tempParent;
    [SerializeField] private bool isHolding = false;
    private bool canHold = true;

    private void Start()
    {
        tempParent = Camera.main.gameObject;
        rb = item.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //check if isHolding
        if (isHolding)
        {
            item.gameObject.layer = 11;
            item.transform.GetChild(0).gameObject.layer = 11;
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            if (Input.GetButtonDown("Throw"))
            {
                item.gameObject.layer = 11;
                item.transform.GetChild(0).gameObject.layer = 11;
                thrown = true;
                canHold = false;
                StartCoroutine("HoldDelay");
                isHolding = false;
            }
        }
        else
        {
            Release();
        }

        if (Input.GetButtonUp("PickUp") && isHolding)
        {
            isHolding = false;
        }
    }

    private void FixedUpdate()
    {
        if (thrown)
        {
            if (item.GetComponent<SoundEffectProp>() != null)
            {
                item.GetComponent<SoundEffectProp>().ThrownByPlayer = true;
            }
            float multiplier = 1;

            if (rb.mass < 10)
            {
                multiplier = 0.25f;
            }
            rb.AddForce(tempParent.transform.forward * (throwForce * multiplier));
            thrown = false;
        }        
    }

    private IEnumerator HoldDelay()
    {
        yield return new WaitForSeconds(1);
        canHold = true;
    }

    public void Pickedup()
    {
        if (canHold)
        {       
            isHolding = true;
            item.GetComponent<Rigidbody>().useGravity = false;
        }        
    }

    public void Release()
    {
        isHolding = false;
        item.gameObject.layer = 12;
        item.transform.GetChild(0).gameObject.layer = 12;
        objectPos = item.transform.position;
        item.transform.SetParent(null);
        item.GetComponent<Rigidbody>().useGravity = true;
        item.transform.position = objectPos;
    }
}