using System.Collections;
using UnityEngine;
using d = UnityEngine.Debug;

namespace Assets.Git.Scripts.Player
{
    [RequireComponent(typeof(Camera))]
    public class MovementController : MonoBehaviour // state based movement, where a state controls a fallin subloop
    {
        #region inspector variables

        [Header("Player Properties")]
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float crouchSpeed;
        [SerializeField] private float slideSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float antiMoveForce;
        [SerializeField] private float speedCap;
        [SerializeField] private float crouchSpeedCap;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float fallDrag;
        [SerializeField] private float decayDrag;
        [SerializeField] private float slideDrag;

        [Header("Objects")]
        [SerializeField] private Camera camera;
        [SerializeField] private Transform groundCheck;

        [Header("Misc")]
        [SerializeField] private float groundCheckRad = 0.2f;

        #endregion

        #region controls

        private KeyCode jumpKC,
            crouchKC;

        #endregion

        #region global variables
        private float drag;

        private Rigidbody rb;
        private State state;
        private int amtJump = 0;
        private bool isGrounded;

        private Vector2 desiredMovement;
        private bool dirInputted,
            dirTapped,
            shiftPressed,
            spacePressed,
            spaceTapped;

        private bool falling;

        private float previousX,
            previousZ;
        #endregion

        #region enums
        enum State
        {
            idle,
            sprinting,
            crouching,
            sliding,
            wallrunning,
            //walljumping,
            vaulting
            // etc
        }
        #endregion

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            drag = rb.drag;
            Cursor.lockState = CursorLockMode.Locked;
            state = State.idle;

            SetControls();

            for (int i = 1; i <= 6; i++)
                VisualDebugger.InitLog(i);
        }

        private void Update()
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRad, groundMask);
            falling = rb.velocity.y < 0 && !isGrounded;
            if (isGrounded)
            {
                amtJump = 0;
                rb.drag = drag;
            }

            if (falling) // decrease drag when falling (i may end up just adding a downwards force instead)
                rb.drag = fallDrag;
            else if (state != State.wallrunning || state != State.sliding) // wallrunning doesnt reduce drag even when falling, and sliding has decay drag
                rb.drag = drag;
            
            // todo: add speed cap

            GetControls();
            state = GetState(desiredMovement);
            Debug();
            Move(desiredMovement, state);
        }


        private void Move(Vector2 input, State state)
        {
            if (spaceTapped && amtJump < 2)
            { 
                amtJump++;
                rb.AddForce(jumpForce * Time.deltaTime * Vector3.up, ForceMode.Impulse);
            }

            switch (state)
            {
                case State.idle: // ok so this now freezing mid air / cant jump in place
                    // method 1

                    // im leaving this because its funny
                    //var antiMove = Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(rb.velocity.x * -1, 0, rb.velocity.z * -1);
                    //if (rb.velocity.x < 0.01f) // min speed to avoid jitter
                    //    antiMove[0] = 0;
                    //if (rb.velocity.z < 0.01f)
                    //    antiMove[2] = 0;
                    //rb.AddForce(antiMoveForce * Time.deltaTime * antiMove);

                    // method 2
                    //if (!falling && !isGrounded)
                    //    rb.drag = decayDrag;

                    rb.drag = decayDrag;

                    break;
                case State.sprinting:
                    rb.drag = 0.0f; // Change this number to make the handling easier
                    // I can still move relative to the player without actually rotating it. Perfect!
                    var move = (Quaternion.AngleAxis(camera.transform.rotation.eulerAngles.y, Vector3.up) * new Vector3(input.x, 0, input.y)).normalized;

                    Vector3 sprintForce = Time.deltaTime * sprintSpeed * move;
                    sprintForce *= (Mathf.Sign(slope) >= 0) ? 1 : 0; //don't apply force when button released
                    rb.AddForce(sprintForce);

                    // Caps the velocity of the rigidbody if it hits above the sprint speed
                    Vector3 velDir = rb.velocity.normalized;
                    rb.velocity = velDir * Mathf.Min(rb.velocity.magnitude, speedCap);
                    
                    break;
                case State.crouching:
                    // TODO: change the player's height and add force like sprint witha lower speedcap
                    break;
                    ;
                case State.sliding:
                    ;
                    break;
                case State.wallrunning:
                    ;
                    break;
            }
        }

        private State GetState(Vector2 input) // order matters!
        {
            if (dirInputted && !shiftPressed) // sprinting, not crouching or sliding
                return State.sprinting;
            else if (dirInputted && shiftPressed && state == State.idle) // crouching
                return State.crouching;
            else if (dirInputted && shiftPressed && state == State.sprinting) // sliding after a sprint
                return State.sprinting;
            else
                return State.idle; // fallout
        }

        float slope;
        private void GetControls()
        {
            float x = Input.GetAxis("Horizontal"), z = Input.GetAxis("Vertical");
            desiredMovement = new Vector2(x, z);
            dirInputted = (desiredMovement.magnitude > 0);
            
            shiftPressed = Input.GetKey(crouchKC);
            spacePressed = Input.GetKey(jumpKC);
            spaceTapped = Input.GetKeyDown(jumpKC);

            Vector2 l = new Vector2(x, z), lPrev = new Vector2(previousX, previousZ);
            slope = l.magnitude - lPrev.magnitude;

            if (Mathf.Sign(slope) == 1) // button pressed / stick pushed up
                dirTapped = true;
            else if (Mathf.Sign(slope) == -1) // button released / stick returning to neutral
                dirTapped = true;
            else dirTapped = false;

            previousX = x;
            previousZ = z;
        }

        private void SetControls()
        {
            jumpKC = Inputs.InputHandler.JumpKC;
            crouchKC = Inputs.InputHandler.CrouchKC;
        }

        void Debug()
        {
            VisualDebugger.Log(1, "Speed", rb.velocity.magnitude);
            VisualDebugger.Log(2, "State", state);
            VisualDebugger.Log(3, "Amt Jump", amtJump);
            VisualDebugger.Log(4, "Drag", rb.drag);
            VisualDebugger.Log(5, "DeltaIn", slope);
            VisualDebugger.Log(6, "Is Grounded", isGrounded);
        }
    }
}
