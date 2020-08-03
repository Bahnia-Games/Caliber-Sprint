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

    private Vector3 force;
    private Vector3 oldForce;
    [SerializeField]
    private float traction;
    private bool isWalking;
    Vector3 dir;


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
            /*
            if (isWalking)
            {
                move();
            }
            else
            {
                rb.velocity *= traction;
            }*/
        }

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            isWalking = true;
        } else
        {
            isWalking = false;
        }

       

        if (Input.GetButtonDown("Jump") && amtJump < 2 || Input.GetButtonDown("Jump") && isGrounded) 
        {
            jump();
        }

        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * fallSpeed * Time.deltaTime);
        }

        if(Input.GetKey(KeyCode.LeftShift) && isGrounded)
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
           //isSlideCool = false;
           //slideCoolTimer -= Time.deltaTime;
           
        }
        

    }

    private void FixedUpdate()
    {
        movePlayer(movement);
        //move();
    }

    void movePlayer(Vector3 direction)
    {
        //rb.AddRelativeForce(direction * speed * Time.deltaTime);
    }

    void move()
    {
        dir.x = Input.GetAxis("Mouse X") * PlayerLook.mouseSensitivity * Time.deltaTime;
        force = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        //force = Quaternion.AngleAxis(dir.x, Vector3.up);
        //rb.rotation = Quaternion.Euler(rot, 0.0f, 0.0f);
        rb.velocity += force - oldForce;
        force = oldForce;
    }

    void jump()
    {
        rb.AddForce(Vector3.up * jumpForce);
        amtJump++;
    }

}
