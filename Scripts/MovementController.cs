using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [Header("Depricated")]

    //haha yes
    #region character controller and general stuff

    public CharacterController controller;
    public float characterCrouch;
    public float characterStand;
    public float speed = 20;

    #endregion

    #region sliding

    Vector3 slideDirection;
    public float minSpeed = 4;
    public float slideSpeed = 40f; 
    public float slideDecayRate = 20f;
    private bool isSliding;
    private float slideTimer;
    public float maxSildeTime = 2.0f;
    public float slideCooldown;
    private bool isSlideCool;
    private float slideCoolTimer;

    #endregion

    #region crouching

    private bool isCrouching;

    #endregion

    #region jumping

    Vector3 velocity;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    #endregion

    #region ground checks

    public Transform groundCheck;
    public float checkRad;
    public LayerMask floorMask;

    #endregion

    #region public static vars

    public static bool isGrounded;
    public static int amtJump;

    #endregion

    void Start() // Not used
    {
        
    }

    
    void Update() // Update is called once per frame
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, checkRad, floorMask); // Ground Check

        if (isGrounded && velocity.y < -2f) // Reset jumps and velocity when the player hits the ground
        {
            velocity.y = 0f;
            amtJump = 0;
        }

        #region general movement

        float x = Input.GetAxis("Horizontal"); // these two lines gather input axis info
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; // make move position relative to player...

        if (!isSliding) // Check if the player is sliding... general movement goes in here...
        {
            controller.SimpleMove(Vector3.forward);
            controller.height = characterStand;
        }

        #endregion

        #region jumping

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded || Input.GetKeyDown(KeyCode.Space) && (amtJump < 2))
            // check if space is pressed, the player has only jumped once, and the player is on the ground before performing a jump (or another)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            amtJump++;
        }

        velocity.y += gravity * Time.deltaTime; // calculates velocity
        controller.SimpleMove(velocity * Time.deltaTime); // Time.deltaTime a second time counts as a square (deltaV = 1/2 gravity * Time^2)
                                                    //The jump is actually performed on this line btw ^

        #endregion

        #region Sliding

        if (Input.GetKey(KeyCode.LeftShift) && !isSliding && isGrounded && isSlideCool) // check to see if the player is grounded, not sliding, and if shift pressed...
        {
            isSliding = true;
            slideTimer = 0.0f;
            controller.height = characterCrouch;

        }

        if (isSliding) // is the player sliding?
        {
            slideTimer += Time.deltaTime; // ticks off a timer (fuck IENumerators, Time.deltaTime is where its at bby!)
            slideSpeed = slideSpeed - slideDecayRate * Time.deltaTime; // Subtract the speed of the slide by a set rate (slideDecayRate)
            controller.SimpleMove(move * slideSpeed * Time.deltaTime); // this is what actually moves the player during the slide
        }
        slideSpeed = Mathf.Clamp(slideSpeed, 0f, Mathf.Infinity); // make sure the player doesnt have negative speed... ((Yeah this really dont work)))
        if (slideTimer > maxSildeTime) //checks to see if the timer is up
        {
            isSliding = false;
            isCrouching = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift)) // cancel the slide
        {
            isSlideCool = false;
            slideCoolTimer = 0.0f;

            isSliding = false;
            controller.height = characterStand;
            slideSpeed = 60f;
        }

        if (!isSlideCool)
        {
            slideCoolTimer += Time.deltaTime;
        } if (slideCoolTimer > slideCooldown)
        {
            isSlideCool = true;
        }
        Debug.Log(isSlideCool);
        #endregion

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 9)
        {
            controller.velocity.Set(0,0,0);
        }
    }
}