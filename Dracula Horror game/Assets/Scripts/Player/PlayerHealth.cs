using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameObject physicsInteraction;
    [SerializeField] private GameObject deathPanel;
    private UIHandler ui;
    private bool dying = false;

    private void Start()
    {
        ui = FindObjectOfType<UIHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !dying)
        {
            Die(other, false);
        }
    }

    public void Die(Collider enemy, bool crushed)
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.PlayCutscene = false;
        }
        
        dying = true;
        StartCoroutine("RestartRoom");
        deathPanel.SetActive(true);
        transform.parent.gameObject.layer = 18;
        physicsInteraction.SetActive(false);

        if (!crushed)
        {
            transform.parent.gameObject.AddComponent<CapsuleCollider>();
            transform.parent.GetComponent<CapsuleCollider>().height = 2.5f;
            transform.parent.gameObject.AddComponent<Rigidbody>();
            GetComponentInParent<Rigidbody>().AddForce(-transform.parent.forward * 10);
            GetComponentInParent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY;
        }
        
        transform.parent.GetComponentInChildren<PlayerLook>().enabled = false;
        transform.parent.GetComponentInChildren<FootStepAudio>().Dead = true;
        GetComponentInParent<CharacterController>().enabled = false;
        GetComponentInParent<PlayerMovement>().enabled = false;
        GetComponentInParent<CandleControls>().enabled = false;

        if (enemy != null)
        {
            enemy.GetComponent<EnemyPlayerSpotter>().PlayerDead = true;
        }
        else
        {
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyPlayerSpotter>().PlayerDead = true;
        }
        
        AudioManager.instance.FadeOut = true;
        AudioManager.instance.FadeOutRate = 0.005f;
        StartCoroutine(AudioManager.instance.StartFadeOut());
    }

    private IEnumerator RestartRoom()
    {
        yield return new WaitForSeconds(10);

        ui.FadeInPanel.SetActive(true);

        while (!GameManager.instance.ReadyLoading)
        {
            yield return null;
        }

        GameManager.instance.ReadyLoading = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
