using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChandelierFall : MonoBehaviour
{
    private bool fell = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (fell)
        {
            return;
        }        

        if (collision.gameObject.layer != 0 && collision.gameObject.layer != 25 && collision.gameObject.layer != 17 && collision.gameObject.layer != 24)
        {
            fell = true;

            for (int i = 1; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            GetComponent<Rigidbody>().mass = 100;
            transform.parent.GetChild(0).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
    }
}
