using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class grapple : MonoBehaviour
{
    public CharacterController controller;
    public Rigidbody rb;
    private bool isEquip;
    private int amtJump;
    private bool isGrounded;
    private bool isGrapple;
    private bool isGrappleReady;
    public float grappleSpeed;

    public float maxGrappleDistance;
    private Vector3 direction;
    private Vector3 origin;
    public LayerMask layerMask;


    void Start()
    {
        rb.GetComponent<Rigidbody>();
    }

    void Update()
    {
        isGrounded = MovementController.isGrounded;
        amtJump = MovementController.amtJump;
        origin = controller.transform.position;
        direction = controller.transform.TransformDirection(Vector3.forward);

        if (Input.GetKeyDown(KeyCode.G) && isGrounded && isEquip && isGrappleReady)
        {
            RaycastHit hit;
            if (Physics.Raycast(origin, direction, out hit, maxGrappleDistance, layerMask))
            {
                controller.enabled = false;
                rb.isKinematic = false;
                isGrapple = true;
                Vector3 anchor = hit.transform.TransformDirection(Vector3.forward);
                rb.AddForce(anchor * grappleSpeed, ForceMode.Force);

            }
        }
    }


}
