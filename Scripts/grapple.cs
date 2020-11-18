using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public Camera playerCam;
    public GameObject grappleHook;
    public Vector3 firingRotation;
    public Rigidbody grappleHookRB; //pub bc unity kept screaming at me
    public CapsuleCollider grappleHookTriggerCC;
    public Rigidbody playerRb;
    public float grappleForce;
    private float grappleMagnitude;
    private Vector3 grappleVelocity;
    [HideInInspector] public bool isGrapple;
    public GameObject returnGO;
    private bool waitForReturn;
    public float grappleCool;
    private bool isReturning;

    public ParticleSystem grapplePuff;

    public GameObject grapplePoint;

    private GameObject _grapplePoint; // grapple POI

    void Start()
    {
        playerRb = this.GetComponent<Rigidbody>();
        grappleHookRB = grappleHook.GetComponent<Rigidbody>();
        grappleHookTriggerCC = grappleHook.GetComponent<CapsuleCollider>();
    }

    void Awake()
    {
        if (grappleHookRB == null)
            Debug.LogError("Error! Could not find grappling hook rigidbody @Grapple.cs L???");
        if (grappleHookTriggerCC == null)
            Debug.LogError("Error! Could not find grappling hook trigger collider @Grapple.cs L???");

        //equip shit for debug idfk just call it in the manager later
        EquipGrapple(true);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.Mouse2) && !isGrapple)
        {
            FireGrapple();
            grapplePuff.Play();
        }

        if (Input.GetKeyUp(KeyCode.Mouse2) && waitForReturn)
            StartCoroutine(GrappleReturnWait());


    }

    void FixedUpdate() 
    {
        if (isReturning)
        {
            Return(); // plays every physics update
        }
    }

    private void FireGrapple()
    {
        returnGO.transform.position = grappleHook.transform.position;
        isGrapple = true;
        grappleHook.transform.parent = null;
        grappleHookRB.isKinematic = false;
        grappleHookRB.AddForce(grappleHook.transform.forward * grappleForce);
        waitForReturn = true;
    }

    IEnumerator GrappleReturnWait() // set the grappling hook to return and wait for cooldown
    {
        waitForReturn = false; // tell code to stop waiting for the return because the return is already playing
        isReturning = true; // tell the code that the grappling hook is returning
        grappleHook.transform.parent = this.transform; // set the transform of the grappling hook as a child of the wrist
        yield return new WaitForSeconds(grappleCool); // wait for grapple cooldown
        isGrapple = false; // tell the code that the player is ready to grapple again
        isReturning = false; // tell the code rgar the grappling hook is no longer returning
        _grapplePoint.tag = "inactiveGrapple"; // set old grapple POI to inactive
    }

    void Return() // fix after everytjing else is fixed
    {
        // plays on every physics update

        grappleHook.transform.position = Vector3.Lerp(grappleHook.transform.position, returnGO.transform.position, grappleCool); // lerp the grappling hook back to the wrist
        grappleHook.transform.rotation = returnGO.transform.rotation; // set the rotation to the rotation of the wrist
    }

    public void EquipGrapple(bool equip)
    {
        if(equip)
            if(grapplePuff != null)
                grapplePuff.gameObject.SetActive(true);
        if(!equip)
            if(grapplePuff)
                grapplePuff.gameObject.SetActive(false);
    }

    public void Collided(Collider collider, ContactPoint contact)
    {
        Debug.Log("Fired");
        //grapplePoint = new GameObject();
        if(collider.gameObject.layer != 11) // if the layer is not ungrapplable
        {
            _grapplePoint = Instantiate(grapplePoint); // spawn a news grappling POI at the first contact point (see Projectile.cs)
            _grapplePoint.name = "newGrapplePoint"; // set grapple POI name
            _grapplePoint.tag = "activeGrapple"; // set grapple POI to active
            _grapplePoint.transform.position = contact.point; // set the position of the new POI to the contact point of the grappling hook
            _grapplePoint.transform.parent = collider.transform; // set the transform of the new POI as a child of whatever mesh it collided with
            grappleHookRB.isKinematic = true; // set the grappling hook to be animated
            Debug.DrawLine(this.transform.position, _grapplePoint.transform.position, Color.red, 15f, false);
            Destroy(_grapplePoint, 60); // destroy the POI after 60 seconds
        }
        
    }
}