using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
<<<<<<< HEAD
    float throwForce = 40000f;
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
=======
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
>>>>>>> master
            item.GetComponent<Rigidbody>().velocity = Vector3.zero;
            item.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            item.transform.SetParent(tempParent.transform);

            if (Input.GetMouseButtonDown(1))
            {
<<<<<<< HEAD
                item.GetComponent<Rigidbody>().AddForce(tempParent.transform.forward * throwForce);
=======
                item.gameObject.layer = 11;
                item.transform.GetChild(0).gameObject.layer = 11;
                thrown = true;
                canHold = false;
                StartCoroutine("HoldDelay");
>>>>>>> master
                isHolding = false;
            }
        }
        else
        {
<<<<<<< HEAD
=======
            item.gameObject.layer = 12;
            item.transform.GetChild(0).gameObject.layer = 12;
>>>>>>> master
            objectPos = item.transform.position;
            item.transform.SetParent(null);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = objectPos;
        }
<<<<<<< HEAD
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
=======

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
            GetComponent<SoundEffectProp>().PersonThatPushed = tempParent.transform.parent.gameObject;
            isHolding = true;
            item.GetComponent<Rigidbody>().useGravity = false;
        }        
    }
}
>>>>>>> master
