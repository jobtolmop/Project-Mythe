using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    float throwForce = 1000;
    Vector3 objectPos;
    float distance;

    [SerializeField] private bool canHold = true;
    [SerializeField] private GameObject item;
    [SerializeField] private GameObject tempParent;
    [SerializeField] private bool isHolding = false;

    private void Update()
    {
        distance = Vector3.Distance(item.transform.position, tempParent.transform.position);
        if (distance > 1f)
        {
            isHolding = false;
        }

        //check if isHolding
        if (isHolding == true)
        {
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            if (Input.GetMouseButtonDown(1))
            {
                item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
                isHolding = false;
            }
        }
        else
        {
            objectPos = item.transform.position;
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = objectPos;
        }
    }

    private void OnMouseDown()
    {
        if (distance <= 1f)
        {
            isHolding = true;
            item.GetComponent<Rigidbody>().useGravity = false;
            item.GetComponent<Rigidbody>().detectCollisions = true;
        }
        
    }

    private void OnMouseUp()
    {
        isHolding = false;
    }
}
