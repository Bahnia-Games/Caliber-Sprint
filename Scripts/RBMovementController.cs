using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMovementController : MonoBehaviour
{
    //disable warnings
    #pragma warning disable CS0108, CS0104

    public float speed = 5f;
    public float defSpeed;
    Rigidbody rb;
    public Transform player;
    public CapsuleCollider collider;
    public Vector3 movement;
    public float fallSpeed;
    public float drag;
    public float slideDrag;
    public float decayDrag = 100f;
    public float fallDrag = 0.01f;

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
    public float notMovingSpeedDecayRate;

    public Vector2 maxAccelVelo;

    [HideInInspector] public bool isMobilityEquip;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        isMobilityEquip = false; // placeholder
    }

    private void Awake()
    {
        VisualDebugger.InitLog(1);
        VisualDebugger.InitLog(2);
        VisualDebugger.InitLog(3);
    }

    // Update is called once per frame
    void Update()
    {
        VisualDebugger.Log(1, "Amt Jump", amtJump);
        VisualDebugger.Log(2, "Is Grounded", isGrounded);
        VisualDebugger.Log(3, "Speed", rb.velocity);

        movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        isGrounded = Physics.CheckSphere(groundCheck.position, checkRad, groundMask);


        speed = Mathf.Clamp(speed, 0.0f , Mathf.Infinity);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) /* && Grapple.isGrapple */)
        {
            speed = defSpeed;
            rb.drag = drag;
        } else if (rb.velocity.x > 0 || rb.velocity.z > 0)
        {
            speed -= notMovingSpeedDecayRate * Time.deltaTime;
            rb.drag = decayDrag;
        }


        if (rb.velocity.y < 0 && !isGrounded)
            rb.drag = fallDrag;

        if (isGrounded)
            amtJump = 0;


        if (Input.GetButtonDown("Jump") && amtJump >= 2 && !isGrounded)
        {
            Debug.Log("Juping in air");
            Jump();
        }
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Debug.Log("Jumping off ground");
            Jump();
        }
        else if (Input.GetButtonUp("Jump")) // potential bug
            rb.drag = drag;

        if (!isGrounded)
            rb.AddRelativeForce(Vector3.down * fallSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift) && isGrounded)
        {
            //rb.transform.localScale = playerCrouch;
            collider.height = playerCrouch.y;
            speed -= slideDecayRate * Time.deltaTime;
            speed = Mathf.Clamp(speed, 0, Mathf.Infinity);
            rb.drag = slideDrag;
            isSliding = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            //rb.transform.localScale = playerStand;
            collider.height = playerStand.y;
            speed = defSpeed;
            rb.drag = drag;
            isSliding = false;
            isSlideCool = false;
            slideCoolTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!isSliding)
            MovePlayer(movement);
    }

    void MovePlayer(Vector3 direction)
    {
        //if (rb.velocity.x > maxAccelVelo.x || rb.velocity.z > maxAccelVelo.y)
        //    rb.transform.TransformDirection(direction * speed);
        //else
        //    rb.AddForce(direction * speed * Time.deltaTime);

        rb.AddRelativeForce(direction * speed * Time.deltaTime);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce);
        amtJump++;

        //if (!isMobilityEquip) // Worksround for now
        //    amtJump++;
    }

}