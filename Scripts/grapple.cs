using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    public Camera playerCam;
    public GameObject grappleHook;
    public Vector3 firingRotation;
    //private GameObject launchedGrappleHook;
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

    private MeshRenderer /* i hope this works*/ grappleMesh;

    void Start()
    {
        playerRb = this.GetComponent<Rigidbody>();
        grappleHookRB = grappleHook.GetComponent<Rigidbody>();
        grappleHookTriggerCC = grappleHook.GetComponent<CapsuleCollider>();
        grappleMesh = grappleHook.GetComponent<MeshRenderer>();
    }

    void Awake()
    {
        if (grappleHookRB == null)
            Debug.LogError("Error! Could not find grappling hook rigidbody @Grapple.cs L???");
        if (grappleHookTriggerCC == null)
            Debug.LogError("Error! Could not find grappling hook trigger collider @Grapple.cs L???");

        //equip shit for debug idfk just cal it in the manager later
        EquipGrapple(true);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.Mouse2) && !isGrapple)
        {
            StartCoroutine(Grapplee());
            grapplePuff.Play();
        }

        if (Input.GetKeyUp(KeyCode.Mouse2) && waitForReturn)
            StartCoroutine(GrappleReturnWait());


    }

    void FixedUpdate() // may or may not be used idk
    {
        if (isReturning)
        {
            Return();
        }
    }

    private IEnumerator Grapplee()
    {
        returnGO.transform.position = grappleHook.transform.position;
        isGrapple = true;
        grappleHook.transform.parent = null;
        grappleHookRB.isKinematic = false;
        grappleHookRB.AddForce(grappleHook.transform.forward * grappleForce);
        yield return new WaitForSeconds(0.001f); // placeholder

        waitForReturn = true;
        //isGrapple = false;
    }

    IEnumerator GrappleReturnWait()
    {
        isReturning = true;
        grappleHookRB.isKinematic = true;
        grappleMesh.enabled = true;
        grappleHook.transform.parent = this.transform;
        //DestroyImmediate(grapplePoint, true);
        yield return new WaitForSeconds(grappleCool);
        isGrapple = false;
        waitForReturn = false;
        isReturning = false;
    }

    void Return()
    {
        Debug.Log("fired");
        //Vector3 orRot = new Vector3(grappleHook.transform.rotation.x, grappleHook.transform.rotation.y, grappleHook.transform.rotation.z);
        //Vector3 reRot = new Vector3(returnGO.transform.rotation.x, returnGO.transform.rotation.y, returnGO.transform.rotation.z);

        grappleHook.transform.position = Vector3.Lerp(grappleHook.transform.position, returnGO.transform.position, grappleCool);
        grappleHook.transform.rotation = returnGO.transform.rotation;
        //grappleHook.transform.rotation.SetFromToRotation(orRot, reRot);
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
        if(collider.gameObject.layer != 11)
        {
            GameObject _grapplePoint = Instantiate(grapplePoint);
            _grapplePoint.name = "newGrapplePoint";
            _grapplePoint.tag = "activeGrapple";
            _grapplePoint.transform.position = contact.point;
            _grapplePoint.transform.parent = collider.transform;
            grappleHook.transform.parent = collider.transform;
            grappleHookRB.isKinematic = false;
            grappleMesh.enabled = false;
            Debug.DrawLine(this.transform.position, _grapplePoint.transform.position, Color.black, 15f);
            _grapplePoint.tag = "inactiveGrapple";
            Destroy(_grapplePoint, 60);
        }
        
    }
}