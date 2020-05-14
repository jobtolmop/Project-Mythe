using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastToItemPickup : MonoBehaviour
{
    private bool alreadyPickedUp = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && !alreadyPickedUp)
        {
            RaycastHit hit;

            int layer = LayerMask.GetMask("Props") | LayerMask.GetMask("Default");

            if (Physics.Raycast(transform.position, transform.forward, out hit, 3, layer))
            {
                Debug.Log(hit.collider);

                if (hit.collider.gameObject.layer == 12)
                {
                    hit.collider.GetComponentInParent<PlayerPickup>().Pickedup();
                    alreadyPickedUp = true;
                }
            }            
        }
        else
        {
            alreadyPickedUp = false;
        }
    }
}
