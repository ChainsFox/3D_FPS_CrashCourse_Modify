using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] public float mouseSensitiviy = 510f;

    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    // Start is called before the first frame update
    void Start()
    {
        //locking the cursor to the middle of the screen and making it invisible
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //getting the mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitiviy * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitiviy * Time.deltaTime;

        //rotation around the x axis(look up and down)
        xRotation -= mouseY; //mouse move up body move down and reverse

        //clamp the rotation(limit the value between 2 other value)
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //rotation around the y axis(look left and right)
        yRotation += mouseX; //mouse move left body move right and reverse

        //apply rotations to our transform
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

    }
}
