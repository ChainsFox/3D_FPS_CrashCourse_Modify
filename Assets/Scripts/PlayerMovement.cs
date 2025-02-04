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
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //ground check 
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); //create a sphere below the player with groundDistance height that will check if we touch the ground
        
        //resetting the default velocity
        if(isGrounded && velocity.y < 0 )
        {
            velocity.y = -2f; //if we jump and land back on the ground, it will reset the velocity
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
}
