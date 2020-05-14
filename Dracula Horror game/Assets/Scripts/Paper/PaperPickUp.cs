using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaperPickUp : MonoBehaviour
{
    public PaperSpawner spawner {get; set;}

    private GameObject playerPageAnim;

    private void Start()
    {
        playerPageAnim = GameObject.FindGameObjectWithTag("Canvas").transform.GetChild(0).gameObject;
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("PickUpPage") && other.CompareTag("Player"))
        {
            spawner.Papers.Remove(transform.parent.gameObject);
            playerPageAnim.SetActive(false);
            playerPageAnim.SetActive(true);
            playerPageAnim.GetComponentInChildren<Text>().text = Mathf.Abs(spawner.Papers.Count - 7) + "/7";

            if (spawner.Papers.Count <= 0)
            {
                spawner.Win();
            }

            Destroy(transform.parent.gameObject);
        }
    }
}
