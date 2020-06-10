using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSpawn : MonoBehaviour
{
    [SerializeField]
    GameObject partPrefab, parentObject, chandelierPrefab;

    [SerializeField]
    [Range(1, 1000)]
    int length = 1;

    [SerializeField]
    float partDistance = 0.21f;

    [SerializeField]
    bool snapFirst, snapLast;

    //private Transform player;
    private MeshRenderer chandelierRenderer;
    
    private void Start()
    {
        Spawn();
        //player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (chandelierRenderer != null && other.CompareTag("Player"))
        {
            chandelierRenderer.enabled = true;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (chandelierRenderer != null && other.CompareTag("Player"))
        {
            chandelierRenderer.enabled = false;
        }
    }

    public void Spawn()
    {
        int count = (int)(length / partDistance);

        for (int x = 0; x < count; x++)
        {
            GameObject tmp;

            tmp = Instantiate(partPrefab, new Vector3(transform.position.x, transform.position.y + partDistance * (x + 1), transform.position.z), Quaternion.identity, parentObject.transform);
            tmp.transform.eulerAngles = new Vector3(180, 0, 0);

            tmp.name = parentObject.transform.childCount.ToString();

            if (x == 0)
            {
                Destroy(tmp.GetComponent<CharacterJoint>());
                if (snapFirst)
                {
                    tmp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }
            else
            {
                tmp.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.Find((parentObject.transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
            }
        }

        GameObject chand = Instantiate(chandelierPrefab, new Vector3(transform.position.x, transform.position.y + partDistance * (parentObject.transform.childCount - 1), transform.position.z), Quaternion.identity, parentObject.transform);
        chand.GetComponent<CharacterJoint>().connectedBody = parentObject.transform.GetChild(parentObject.transform.childCount - 2).GetComponent<Rigidbody>();
        chandelierRenderer = chand.GetComponent<MeshRenderer>();
        chandelierRenderer.enabled = false;

        if (snapLast)
        {
            parentObject.transform.Find((parentObject.transform.childCount).ToString()).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
    }
}
