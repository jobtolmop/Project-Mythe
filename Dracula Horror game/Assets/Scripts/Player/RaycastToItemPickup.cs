using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastToItemPickup : MonoBehaviour
{
    private bool alreadyPickedUp = false;
    private bool doorHold = false;
    private GameObject pickedUpObject;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            int layer = LayerMask.GetMask("Props") | LayerMask.GetMask("Default");

            if (Physics.Raycast(transform.position, transform.forward, out hit, 3, layer))
            {
                //Debug.Log(hit.collider);

                if (hit.collider.gameObject.layer == 12 && pickedUpObject == null)
                {
                    pickedUpObject = hit.collider.transform.parent.gameObject;                    

                    if (pickedUpObject.CompareTag("Door"))
                    {
                        doorHold = true;
                    }
                    else
                    {                       
                        if (pickedUpObject.GetComponent<PlayerPickup>() != null)
                        {
                            pickedUpObject.GetComponent<PlayerPickup>().Pickedup();
                        }
                    }                                                         
                }
            }

            //Debug.Log((pickedUpObject.transform.position - transform.position).sqrMagnitude);

            if (pickedUpObject != null && (pickedUpObject.transform.position - transform.position).sqrMagnitude > 20)
            {
                if (pickedUpObject.GetComponent<PlayerPickup>() != null)
                {
                    pickedUpObject.GetComponent<PlayerPickup>().Release();
                }                
                pickedUpObject = null;
                doorHold = false;
            }
        }
        else
        {
            doorHold = false;
            pickedUpObject = null;
        }
    }

    private void FixedUpdate()
    {
        if (doorHold && pickedUpObject != null)
        {
            Vector3 nextPos = transform.position + transform.forward * 2;
            Vector3 currPos = pickedUpObject.transform.position;

            pickedUpObject.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 20;
            //Debug.Log(pickedUpObject.GetComponent<Rigidbody>().velocity);
        }        
    }
}
