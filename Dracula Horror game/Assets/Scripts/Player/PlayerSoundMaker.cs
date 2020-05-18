using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundMaker : MonoBehaviour
{
    [SerializeField] private GameObject walkSound;
    [SerializeField] private GameObject runSound;
    [SerializeField] private GameObject crouchSound;
    [SerializeField] private GameObject jumpSound;
    [SerializeField] private GameObject landingSound;

    private PlayerMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movement.Jumping || movement.Movement.magnitude == 0)
        {
            walkSound.SetActive(false);
            runSound.SetActive(false);
            crouchSound.SetActive(false);

            return;
        }

        if (movement.Crouching)
        {
            walkSound.SetActive(false);
            runSound.SetActive(false);
            crouchSound.SetActive(true);
        }
        else
        {
            crouchSound.SetActive(false);

            if (Input.GetButton("Sprint"))
            {
                walkSound.SetActive(false);
                runSound.SetActive(true);
            }
            else
            {
                walkSound.SetActive(true);
                runSound.SetActive(false);
            }
        }   
    }

    public void PlayCoroutineJumpLand()
    {
        StartCoroutine("PlayJumpingOrLandingSound");
    }

    private IEnumerator PlayJumpingOrLandingSound()
    {
        if (movement.Jumping)
        {
            jumpSound.SetActive(true);
        }
        else
        {
            landingSound.SetActive(true);
        }
        
        yield return new WaitForSeconds(0.5f);

        if (movement.Jumping)
        {
            jumpSound.SetActive(false);
        }
        else
        {
            landingSound.SetActive(false);
        }
    }
}
