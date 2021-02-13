using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapplev2 : MonoBehaviour
{
    public Camera playerCam;
    public bool grappleEquipped;

    [Header("Properties")]
    public GameObject grappleHook;
    public Vector3 firingRotation;
    public Rigidbody grappleHookRB; //pub bc unity kept screaming at me
    public CapsuleCollider grappleHookTriggerCC;
    public Rigidbody playerRb;
    public float grappleForce;
    public float grapplePullForce;
    private float grappleMagnitude;
    private Vector3 grappleVelocity;
    [HideInInspector] public bool isGrapple;
    public GameObject returnGO;
    private bool waitForReturn;
    public float grappleCoolTime;
    private bool isGrappleCool;
    private bool isReturning;

    public float grappleEquipTime;

    public float maxGrappleTime;
    private bool movingWithGrapple;
    public GameObject pointToTarget;
    private bool grappleCollided;

    public ParticleSystem grapplePuff;

    public GameObject grapplePoint;

    private GameObject grapplePoi; // grapple POI
    public static Grapplev2 Instance;

    #region animation
    [Header("Animation")]
    public string EQUIP;
    public string UNEQUIP;
    public string FIRE;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        Instance = gameObject.GetComponent<Grapplev2>();
        playerRb = playerRb.GetComponent<Rigidbody>();
        grappleHookRB = grappleHook.GetComponent<Rigidbody>();
        grappleHookTriggerCC = grappleHook.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (grappleEquipped && isGrappleCool && Input.GetKeyDown(KeyCode.Mouse2) && !isGrapple) // mouse key pressed and grapple isnt grappling, is equipped, and has cooled down
        {
            StartCoroutine(FireGrapple());
        }
    }

    public void EquipGrapple(bool equip) // called in manager
    {
        Instance.StartCoroutine(Instance._EquipGrapple(equip));
    }

    private IEnumerator _EquipGrapple(bool equip)
    {
        if (equip)
        {
            // play anim
            yield return new WaitForSeconds(grappleEquipTime);
            grappleEquipped = true;
        }
        if (!equip)
        {
            // play unequip
            yield return new WaitForSeconds(grappleEquipTime);
            grappleEquipped = false;
        }
    }

    private IEnumerator FireGrapple()
    {
        // fire the grappling hook and tell the code to start waiting for a response from collider
        // play particles and animation
        yield return new WaitForSeconds(1f); // placeholder
    }

    private void WaitForImpact()
    {
        // this waits for the grapple to collide and sets the grapple POI to,
        // this also sets AddGrappleForce active
    }

    private void AddGrappleForce()
    {
        // physically adds the force
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(grappleCoolTime);
    }
}
