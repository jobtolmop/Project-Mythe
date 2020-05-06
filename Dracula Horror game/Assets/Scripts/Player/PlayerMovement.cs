using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField] private float speed = 6;
    [SerializeField] private float runSpeed = 12;
    [SerializeField] private float walkSpeed = 6;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundDistance = 0.4f;

    private Vector3 velocity = Vector3.zero;

    private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;

    [SerializeField] private bool grounded = false;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        groundCheck = transform.GetChild(1);
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

        if (Input.GetButton("Sprint"))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
