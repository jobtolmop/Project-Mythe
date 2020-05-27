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
    public bool CantMove { get; set; } = false;

    private Transform cam;

    private PlayerSoundMaker soundMaker;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        //soundMaker = GetComponent<PlayerSoundMaker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CantMove)
        {
            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            Vector3 move = transform.right * movement.x + transform.forward * movement.y;
            if (move.magnitude > 1)
            {
                move.Normalize();
            }
            //Move is called before controls like jump and sprint check because isGrounded must be called after move()
            controller.Move(move * speed * Time.deltaTime);
        }        

        controller.Move(velocity * Time.deltaTime);

        if (controller.isGrounded)
        {
            if (jumping)
            {
                jumping = false;
                //soundMaker.PlayCoroutineJumpLand();
            }

            if (Input.GetButton("Sprint"))
            {
                if (crouching)
                {
                    Crouch(false);
                }

                if (speed < runSpeed) 
                {
                    speed += 7 * Time.deltaTime;
                }
            }
            else
            {
                if (!crouching)
                {
                    if (speed > walkSpeed)
                    {
                        speed -= 10 * Time.deltaTime;
                    }
                    else
                    {
                        speed = walkSpeed;
                    }
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
                //soundMaker.PlayCoroutineJumpLand();
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
                controller.height = 2;
                physicsObject.localScale = Vector3.one;
                cam.localPosition = new Vector3(0, 0.8f, cam.localPosition.z);
            }            
        }
        else
        {
            crouching = true;
            controller.height = 1;
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
