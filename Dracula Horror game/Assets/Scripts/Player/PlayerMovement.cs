using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private float speed = 6;
    [SerializeField] private float runSpeed = 12;
    [SerializeField] private float walkSpeed = 6;
    [SerializeField] private float crouchSpeed = 3;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private float jumpHeight = 3;

    private Vector3 velocity = Vector3.zero;

    private Transform groundCheck;
    private Transform roofCheck;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.GetChild(1);
        roofCheck = transform.GetChild(4);
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (grounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 move = transform.right * movement.x + transform.forward * movement.y;

        if (Input.GetButton("Sprint") && grounded && controller.height > 1)
        {
            if (controller.height < 2)
            {
                ScaleHeight(true);
            }            
            speed = runSpeed;
        }
        else
        {
            if (Input.GetButton("Crouch") && grounded)
            {
                if (controller.height > 1)
                {
                    ScaleHeight(false);
                }
                speed = crouchSpeed;
            }
            else
            {
                if (controller.height < 2)
                {
                    ScaleHeight(true);
                }

                if (controller.height > 1)
                {
                    speed = walkSpeed;
                }                
            }
        }   

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && grounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void ScaleHeight(bool up)
    {
        if (up)
        {
            if (!Physics.CheckSphere(roofCheck.position, groundDistance, groundMask))
            {
                controller.height = 2;
                transform.GetChild(3).localScale = new Vector3(1, 1f, 1);
                Camera.main.transform.localPosition = new Vector3(0, 0.8f, 0);
            }            
        }
        else
        {
            controller.height = 1;
            transform.GetChild(3).localScale = new Vector3(1, 0.5f, 1);
            Camera.main.transform.localPosition = new Vector3(0, 0.25f, 0);
        }
    }
}
