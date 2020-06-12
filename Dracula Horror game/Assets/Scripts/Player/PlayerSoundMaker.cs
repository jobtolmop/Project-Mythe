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
    [SerializeField] private AudioSource jumpLandSource;

    [SerializeField] private AudioClip[] sfx;

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
            jumpLandSource.volume = 0.2f;
            jumpLandSource.PlayOneShot(sfx[0]);
        }
        else
        {
            jumpLandSource.volume = 1f;
            jumpLandSource.PlayOneShot(sfx[1]);
        }

        jumpSound.SetActive(movement.Jumping);
        landingSound.SetActive(!movement.Jumping);
        
        yield return new WaitForSeconds(0.5f);

        jumpSound.SetActive(false);
        landingSound.SetActive(false);
    }
}
