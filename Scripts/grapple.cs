﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Grapple : MonoBehaviour
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

    private GameObject grapplePoint;

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
        grappleHook.transform.parent = this.transform;
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
        grapplePoint = new GameObject();
        grapplePoint.transform.position = contact.point;
    }
}