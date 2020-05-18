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
    [SerializeField] private GameObject tempParent;
    [SerializeField] private bool isHolding = false;
    private bool canHold = true;

    private void Start()
    {
        tempParent = Camera.main.gameObject;
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

            if (Input.GetMouseButtonDown(1))
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
            item.gameObject.layer = 12;
            item.transform.GetChild(0).gameObject.layer = 12;
            objectPos = item.transform.position;
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = objectPos;
        }

        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            isHolding = false;
        }
    }

    private void FixedUpdate()
    {
        if (thrown)
        {
            item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
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
            if (GetComponent<SoundEffectProp>() != null) {
                GetComponent<SoundEffectProp>().PersonThatPushed = tempParent.transform.parent.gameObject;
            }
            isHolding = true;
            item.GetComponent<Rigidbody>().useGravity = false;
        }        
    }
}