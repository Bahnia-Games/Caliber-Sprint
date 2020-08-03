using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMovementController : MonoBehaviour
{

    public float speed = 5f;
    public float defSpeed;
    Rigidbody rb;
    public Transform player;
    public Vector3 movement;
    public float fallSpeed;
    public float drag;
    public float slideDrag;

    public LayerMask groundMask;
    public float checkRad;
    public Transform groundCheck;
    private bool isGrounded;
    public float jumpForce;
    int amtJump = 0;

    public Vector3 playerStand;
    public Vector3 playerCrouch;

    public float slideDecayRate;
    public float slideSpeed;
    private float slideVarSpeed;
    //public float slideTime;
    //private float slideTimer;
    private bool slideActive = true;
    private bool isSliding;
    private bool isSlideCool;
    private float slideCoolTimer;
    public float slideCooldownRate;


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

        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * fallSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.LeftShift) && isGrounded && isSlideCool)
        {
            player.transform.localScale = playerCrouch;
            speed -= slideDecayRate * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0, Mathf.Infinity);
            rb.drag = slideDrag;
            isSliding = true;
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            player.transform.localScale = playerStand;
            speed = defSpeed;
            rb.drag = drag;
            isSliding = false;
            isSlideCool = false;
            slideCoolTimer -= Time.deltaTime;
        }

        if (slideCoolTimer < 0)
        {
            isSlideCool = true;
            slideCoolTimer = slideCooldownRate;
        }
        Debug.Log(slideCoolTimer);

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
