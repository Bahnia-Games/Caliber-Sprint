using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Grapple : MonoBehaviour
{
    public GameObject grappleHook;
    private Rigidbody grappleHookRB;
    private Rigidbody rb;
    public float grappleForce;
    private float grappleMagnitude;
    private Vector3 grappleVelocity;
    [HideInInspector] public bool isGrapple;

    

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        grappleHookRB = grappleHook.GetComponent<Rigidbody>();
    }

    void Awake()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.Mouse2))
        {
            StartCoroutine(Grapplee());
        }
    }

    void FixedUpdate() // may or may not be used idk
    {
        
    }

    private IEnumerator Grapplee()
    {
        isGrapple = true;
        yield return new WaitForSeconds(0.001f); // placeholder

        isGrapple = false;
    }

}