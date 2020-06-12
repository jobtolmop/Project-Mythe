using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Submit"))
        {
            anim.speed = 5;
        }
        else
        {
            anim.speed = 1;
        }
    }

    public void DisableCreditsPanel()
    {
        gameObject.SetActive(false);
    }
}
