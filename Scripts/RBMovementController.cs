using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMovementController : MonoBehaviour
{

    public float speed = 5f;
    Rigidbody rb;
    public Vector3 movement;

    public LayerMask groundMask;
    public float checkRad;
    public Transform groundCheck;
    private bool isGrounded;
    public float jumpForce;
    int amtJump = 0;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        isGrounded = Physics.CheckSphere(groundCheck.position, checkRad, groundMask);

        if (isGrounded)
        {
            amtJump = 0;
        }

        if (Input.GetButtonDown("Jump") && amtJump < 2 || Input.GetButtonDown("Jump") && isGrounded) 
        {
            jump();
        }
        Debug.Log(amtJump);

    }

    private void FixedUpdate()
    {
        movePlayer(movement);
    }

    void movePlayer(Vector3 direction)
    {
        rb.AddRelativeForce(direction * speed * Time.deltaTime);
    }

    void jump()
    {
        rb.AddForce(Vector3.up * jumpForce);
        amtJump++;
    }
}
