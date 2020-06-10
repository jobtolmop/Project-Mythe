using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    private Vector2 movement;
    [SerializeField] private float speed = 6;
    [SerializeField] private float runSpeed = 12;
    [SerializeField] private float walkSpeed = 6;
    [SerializeField] private float crouchSpeed = 3;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float roofDistance = 0.4f;
    [SerializeField] private float jumpHeight = 3;

    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform roofCheck;
    [SerializeField] private Transform physicsObject;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private bool crouching = false;
    [SerializeField] private bool jumping = false;

    public bool Crouching { get { return crouching; } }
    public bool Jumping { get { return jumping; } }
    public Vector2 Movement { get { return movement; } }
    [SerializeField] private bool cantMove = false;
    public bool CantMove { set { cantMove = value; } }

    private Transform cam;

    private PlayerSoundMaker soundMaker;
    private FootStepAudio footStepAudio;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        soundMaker = GetComponent<PlayerSoundMaker>();
        footStepAudio = GetComponentInChildren<FootStepAudio>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cantMove)
        {
            return;
        }

        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 move = transform.right * movement.x + transform.forward * movement.y;
        if (move.magnitude > 1)
        {
            move.Normalize();
        }
        //Move is called before controls like jump and sprint check because isGrounded must be called after move()
        controller.Move(move * speed * Time.deltaTime);      

        controller.Move(velocity * Time.deltaTime);

        footStepAudio.Grounded = controller.isGrounded;

        if (controller.isGrounded)
        {
            if (jumping)
            {
                jumping = false;
                soundMaker.PlayCoroutineJumpLand();
            }

            if (Input.GetButton("Sprint"))
            {                
                if (crouching)
                {
                    Crouch(false);
                }

                if (!crouching)
                {
                    footStepAudio.StepLoop(movement.magnitude, 1.8f, 1);
                    if (speed < runSpeed)
                    {
                        speed += 7 * Time.deltaTime;
                    }
                }
                else
                {
                    footStepAudio.StepLoop(movement.magnitude, 0.8f, 0.6f);
                }
            }
            else
            {
                if (!crouching)
                {
                    //if (movement.magnitude > 0.1f)
                    //{
                    //    footStepAudio.StartStepping(0.1f, 0.7f);
                    //}

                    footStepAudio.StepLoop(movement.magnitude, 1f, 0.8f);

                    if (speed > walkSpeed)
                    {
                        speed -= 10 * Time.deltaTime;
                    }
                    else
                    {
                        speed = walkSpeed;
                    }
                }    
                else
                {
                    footStepAudio.StepLoop(movement.magnitude, 0.8f, 0.6f);
                    /*if (movement.magnitude > 0.1f)
                    {
                        footStepAudio.StartStepping(0.2f, 0.4f);
                    }*/
                }
            }

            if (Input.GetButtonDown("Crouch"))
            {
                Crouch(!crouching);

                if (crouching)
                {                    
                    speed = crouchSpeed;
                }
            }

            if (velocity.y < 0)
            {
                velocity.y = -2;
            }

            if (Input.GetButtonDown("Jump"))
            {                
                jumping = true;
                soundMaker.PlayCoroutineJumpLand();
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }
        }        

        velocity.y += gravity * Time.deltaTime;
    }

    private void Crouch(bool down)
    {
        if (!down)
        {
            if (!Physics.CheckSphere(roofCheck.position, roofDistance, groundMask))
            {
                crouching = false;
                controller.height = 2.5f;
                physicsObject.localScale = Vector3.one;
                cam.localPosition = new Vector3(0, 0.95f, cam.localPosition.z);
            }            
        }
        else
        {
            crouching = true;
            controller.height = 1.25f;
            physicsObject.localScale = new Vector3(1, 0.5f, 1);
            cam.localPosition = new Vector3(0, 0.25f, cam.localPosition.z);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (roofCheck != null)
        {
            Gizmos.DrawWireSphere(roofCheck.position, roofDistance);
        }        
    }
}
