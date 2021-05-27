using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMovementController : MonoBehaviour
{
    //disable warnings
#pragma warning disable CS0108, CS0104

    [SerializeField] private Camera cam;
    [SerializeField] private Animator cameraAnimator;

    public float speed = 5f;
    public float defSpeed;
    private Rigidbody rb;
    [SerializeField] private Transform player;
    [SerializeField] private CapsuleCollider collider;
    public Vector3 movement;
    public float fallSpeed;
    public float drag;
    public float slideDrag;
    public float decayDrag = 100f;
    public float fallDrag = 0.01f;

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float checkRad;
    [SerializeField] private Transform groundCheck;
    private bool isGrounded;
    public float jumpForce;
    private int amtJump = 0;

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

    [SerializeField] private Transform wallCheckL;
    [SerializeField] private Transform wallCheckR;
    [SerializeField] private float wallRunCheckRad;
    private LayerMask wallRunMask = 16;

    public Vector3 wallrunRotationROffset;
    public Vector3 wallrunRotationLOffset;
    public Vector3 wallrunPositionROffset;
    public Vector3 wallrunPositionLOffset;
    public float wallrunRotOffsetSpeed;
    public float wallrunPosOffsetSpeed;

    public float wallruntime;
    public float wallrunEnterAcceleration;
    public float wallrunExitAcceletation;

    Vector3 cameraOriginalPos;

    [HideInInspector] public bool isMobilityEquip;

    bool canWallrunFlag;
    bool wallrunStartFireOnce = true;

    public enum States
    {
        idle,
        sprint,
        slide,
        wallrun,
        jump,
        doublejump
    }

    enum WrType
    {
        l,
        r
    }

    enum AnimationStates
    {
        EnterWRR,
        EnterWRL,
        WRR,
        WRL,
        ExitWRR,
        ExitWRL
    }

    private States state;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        isMobilityEquip = false; // placeholder
        cameraOriginalPos = cam.transform.position;
    }

    private void Awake()
    {
        VisualDebugger.InitLog(1);
        VisualDebugger.InitLog(2);
        VisualDebugger.InitLog(3);
        VisualDebugger.InitLog(4);
    }

    // Update is called once per frame
    void Update()
    {
        state = States.idle;
        VisualDebugger.Log(1, "Amt Jump", amtJump);
        VisualDebugger.Log(2, "Is Grounded", isGrounded);
        VisualDebugger.Log(3, "Speed", rb.velocity);

        movement = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));

        isGrounded = Physics.CheckSphere(groundCheck.position, checkRad, groundMask);


        speed = Mathf.Clamp(speed, 0.0f , Mathf.Infinity);
        
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) /* && Grapple.isGrapple */)
        {
            state = States.sprint;
            speed = defSpeed;
            rb.drag = drag;
        } else if (rb.velocity.x > 0 || rb.velocity.z > 0)
        {
            state = States.idle;
            speed -= notMovingSpeedDecayRate * Time.deltaTime;
            rb.drag = decayDrag;
        }

        if (rb.velocity.y < 0 && !isGrounded)
            rb.drag = fallDrag;

        if (isGrounded)
        {
            amtJump = 0;
            canWallrunFlag = true;
        }


        if (Input.GetButtonDown("Jump") && amtJump >= 2 && (!isGrounded || state == States.wallrun))
        {
            state = States.doublejump;
            Debug.Log("Juping in air");
            Jump();
        }
        if (Input.GetButtonDown("Jump") && (isGrounded || state == States.wallrun))
        {
            state = States.jump;
            Debug.Log("Jumping off ground");
            Jump();
        }
        else if (Input.GetButtonUp("Jump")) // potential bug
        {
            state = States.jump;
            rb.drag = drag;
        }

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

        WrType curtype = WrType.r;
        if ((Physics.CheckSphere(wallCheckL.position, wallRunCheckRad, 1 << 16) || Physics.CheckSphere(wallCheckR.position, wallRunCheckRad, 1 << 16)) && !isGrounded && canWallrunFlag) // check left/right wallrun
        {
            amtJump = 0;
            state = States.wallrun;
            rb.useGravity = false;
            if (wallrunStartFireOnce && Physics.CheckSphere(wallCheckR.position, wallRunCheckRad, 1 << 16))
            {
                StartCoroutine(Wallrun(WrType.l));
                curtype = WrType.l;
            }
            if (wallrunStartFireOnce && Physics.CheckSphere(wallCheckL.position, wallRunCheckRad, 1 << 16))
            {
                StartCoroutine(Wallrun(WrType.r));
                curtype = WrType.r;

            }
        }
        else
        {
            StopCoroutine(Wallrun(curtype));
            switch (curtype)
            {
                case WrType.r:
                    PlayAnim(AnimationStates.ExitWRR);
                    break;
                case WrType.l:
                    PlayAnim(AnimationStates.ExitWRL);
                    break;
            }
        }

        if (!isGrounded && state != States.wallrun)
            rb.AddRelativeForce(Vector3.down * fallSpeed * Time.deltaTime);

        VisualDebugger.Log(4, "State", state);
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

    IEnumerator Wallrun(WrType t)
    {
        switch (t)
        {
            case WrType.r:
                PlayAnim(AnimationStates.EnterWRR);
                break;
            case WrType.l:
                PlayAnim(AnimationStates.EnterWRL);
                break;
        }
        wallrunStartFireOnce = false;
        rb.AddForce(rb.transform.forward * wallrunEnterAcceleration, ForceMode.Acceleration);
        yield return new WaitForSeconds(wallruntime);
        wallrunStartFireOnce = true;
        canWallrunFlag = false;

        switch (t)
        {
            case WrType.r:
                rb.AddForce(rb.transform.right * wallrunExitAcceletation * 0.01f, ForceMode.Impulse);
                PlayAnim(AnimationStates.ExitWRR);
                break;
            case WrType.l:
                rb.AddForce(-rb.transform.right * wallrunExitAcceletation * 0.01f, ForceMode.Impulse);
                PlayAnim(AnimationStates.ExitWRL);
                break;
        }
    }

    void PlayAnim(AnimationStates state)
    {
        switch (state)
        {
            case AnimationStates.EnterWRR:
                cameraAnimator.Play("StartWallrunRight");
                break;
            case AnimationStates.EnterWRL:
                cameraAnimator.Play("StartWallrunLeft");
                break;
            case AnimationStates.WRR:
                cameraAnimator.Play("WallrunRight");
                break;
            case AnimationStates.WRL:
                cameraAnimator.Play("WallRunLeft");
                break;
            case AnimationStates.ExitWRR:
                cameraAnimator.Play("ExitWallrunRight");
                break;
            case AnimationStates.ExitWRL:
                cameraAnimator.Play("ExitWallrunLeft");
                break;
        }
    }
}