using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject physicsInteraction;
    [SerializeField] private GameObject deathPanel;
    private bool dying = false;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.isTrigger)
    //    {
    //        health--;

    //        if (health <= 0)
    //        {
    //            Die();
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !dying)
        {
            dying = true;
            StartCoroutine("RestartRoom");
            deathPanel.SetActive(true);
            transform.parent.gameObject.layer = 18;
            physicsInteraction.SetActive(false);
            transform.parent.gameObject.AddComponent<CapsuleCollider>();
            transform.parent.GetComponent<CapsuleCollider>().height = 2.5f;
            transform.parent.gameObject.AddComponent<Rigidbody>();
            GetComponentInParent<Rigidbody>().AddForce(-transform.parent.forward * 10);
            GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
            transform.parent.GetComponentInChildren<PlayerLook>().enabled = false;
            GetComponentInParent<CharacterController>().enabled = false;
            GetComponentInParent<PlayerMovement>().enabled = false;
            GetComponentInParent<CandleControls>().enabled = false;
            other.GetComponent<EnemyPlayerSpotter>().PlayerDead = true;
            AudioManager.instance.FadeOut = true;
            AudioManager.instance.FadeOutRate = 0.005f;
        }
    }

    private IEnumerator RestartRoom()
    {
        yield return new WaitForSeconds(10);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
