using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 12f;
    public float gravity = -9.81f * 2f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPosition = new Vector3(0f,0f,0f);

    //GrapplingHook 
    public bool freeze;
    public bool activeGrapple;


    void Start()
    {
        controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        if (activeGrapple) return; //grapple freeze

        //ground check 
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //create a sphere below the player with groundDistance height that will check if we touch the ground
        
        //resetting the default velocity
        if(isGrounded && velocity.y < 0 && !activeGrapple)
        {
            velocity.y = -2f; //if we jump and land back on the ground, it will reset the velocity
        }

        //grappling
        if (freeze)
        {
            speed = 0f;
            velocity = Vector3.zero;
        }
        else
        {
            speed = 12f;
        }

        //getting the inputs
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //creating the moving vector 
        Vector3 move = transform.right * x + transform.forward * z;//right - red axis(move left right), forward - blue axis(move forward backward) in unity editor; mulitiply the input with direction
        
        //actually moving the player
        controller.Move(move * speed * Time.deltaTime);

        //check if the player can jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //falling down/apply custom gravity 
        velocity.y += gravity * Time.deltaTime;

        //execute jump/actually jumping
        controller.Move(velocity * Time.deltaTime);

        //is Moving
        if(lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        lastPosition = gameObject.transform.position;
    
    
    }
    //GrapplingHook:
    public void JumpToPosition(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);

        //Invoke(nameof(ResetRestrictions), 3f);

        velocity = CalculateJumpVelocity(transform.position, targetPosition, trajectoryHeight);
    }

    private Vector3 velocityToSet;
    private void SetVelocity()
    {
        //enableMovementOnNextTouch = true;
        //rb.velocity = velocityToSet;
        velocity = velocityToSet;

        //cam.DoFov(grappleFov);
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }

}
