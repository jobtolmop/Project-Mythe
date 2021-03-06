﻿using System.Collections;
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
    private Transform prevParent;
    [SerializeField] private bool isHolding = false;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float multiplier = 1;
    private bool canHold = true;

    private void Start()
    {
        prevParent = transform.parent;
        tempParent = Camera.main.gameObject;
        rb = item.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (transform.position.y < -2)
        {
            transform.position = new Vector3(transform.position.x, 3, transform.position.z);
        }

        //check if isHolding
        if (isHolding)
        {
            item.gameObject.layer = 11;
            item.transform.GetChild(0).gameObject.layer = 11;
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            if (Input.GetButtonDown("Throw") && !item.CompareTag("Door"))
            {
                item.gameObject.layer = 11;
                item.transform.GetChild(0).gameObject.layer = 11;
                thrown = true;
                canHold = false;
                StartCoroutine("HoldDelay");
                isHolding = false;
            }
            else if (Input.GetButton("Rotate"))
            {
                float x = Input.GetAxis("Mouse X") * rotationSpeed;
                float y = Input.GetAxis("Mouse Y") * rotationSpeed;

                //Vector3 tempVect = tempParent.transform.TransformVector(new Vector3(y, x, 0));

                //item.transform.Rotate(-tempParent.transform.up * x);
                //item.transform.Rotate(tempParent.transform.right * y);
                item.transform.Rotate(tempParent.transform.up, x, Space.World);
                item.transform.Rotate(tempParent.transform.right, -y, Space.World);
            }

            if (Input.GetButtonUp("PickUp"))
            {
                isHolding = false;
            }
        }
        else
        {
            Release();
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
        item.gameObject.layer = 12;
        item.transform.GetChild(0).gameObject.layer = 12;
        objectPos = item.transform.position;
        item.transform.SetParent(prevParent);
        item.GetComponent<Rigidbody>().useGravity = true;
        item.transform.position = objectPos;
    }
}