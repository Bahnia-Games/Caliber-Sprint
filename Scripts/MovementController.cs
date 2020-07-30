using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    //haha yes

    public CharacterController controller;
    public float characterCrouch;
    public float characterStand;
    public float speed = 20;

    Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    private int amtJump;

    public Transform groundCheck;
    public float checkRad;
    public LayerMask floorMask;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        //Not used
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRad, floorMask);

        if (isGrounded && velocity.y < -2f)
        {
            velocity.y = 0f;
            amtJump = 0;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded || Input.GetButtonDown("Jump") && (amtJump < 2))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            amtJump++;
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetButtonDown("Crouch"))
        {
            
        }

    }
}
