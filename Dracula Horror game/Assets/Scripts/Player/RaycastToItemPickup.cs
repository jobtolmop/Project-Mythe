using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastToItemPickup : MonoBehaviour
{
    private bool alreadyPickedUp = false;
    private bool doorHold = false;
    private GameObject pickedUpObject;
    private Rigidbody doorRb;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("PickUp"))
        {
            RaycastHit hit;

            int layer = LayerMask.GetMask("Props") | LayerMask.GetMask("Default") | LayerMask.GetMask("Door");

            if (Physics.Raycast(transform.position, transform.forward, out hit, 3, layer))
            {
                Debug.Log(hit.collider.gameObject);

                if ((hit.collider.gameObject.layer == 12 || hit.collider.gameObject.layer == 23) && pickedUpObject == null)
                {
                    if (!hit.collider.CompareTag("Door") && !hit.collider.CompareTag("Paper"))
                    {
                        
                        pickedUpObject = hit.collider.transform.parent.gameObject;
                    }     
                    else
                    {
                        pickedUpObject = hit.collider.gameObject;
                    }

                    if (pickedUpObject.CompareTag("Door") && !doorHold)
                    {
                        doorRb = pickedUpObject.GetComponent<Rigidbody>();
                        doorHold = true;
                        pickedUpObject.GetComponent<SoundEffectProp>().DoorHold = true;                        
                    }
                    else if (pickedUpObject.CompareTag("Paper"))
                    {
                        pickedUpObject.GetComponent<PaperPickUp>().PickUpPage();
                        pickedUpObject = null;
                    }
                    else
                    {                       
                        if (pickedUpObject.GetComponent<PlayerPickup>() != null && !doorHold)
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
                if (pickedUpObject.GetComponent<SoundEffectProp>() != null)
                {                    
                    pickedUpObject.GetComponent<SoundEffectProp>().DoorHold = false;
                }
                pickedUpObject = null;                
                doorHold = false;
            }
        }
        else
        {
            doorHold = false;
            if (pickedUpObject != null && pickedUpObject.GetComponent<SoundEffectProp>() != null)
            {
                pickedUpObject.GetComponent<SoundEffectProp>().DoorHold = false;
            }            
            pickedUpObject = null;
        }
    }

    private void FixedUpdate()
    {
        if (doorHold && pickedUpObject != null)
        {
            Vector3 nextPos = transform.position + transform.forward * 2;
            Vector3 currPos = pickedUpObject.transform.position;

            doorRb.velocity = (nextPos - currPos) * 20;
            //Debug.Log(pickedUpObject.GetComponent<Rigidbody>().velocity);
        }        
    }
}
