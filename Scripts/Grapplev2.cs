using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapplev2 : MonoBehaviour
{
    public Camera playerCam;
    public bool grappleEquipped;

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
    public float grappleCool;
    private bool isReturning;

    public float grappleEquipTime;

    public float maxGrappleTime;
    private bool movingWithGrapple;
    public GameObject pointToTarget;
    private bool grappleCollided;

    public ParticleSystem grapplePuff;

    public GameObject grapplePoint;

    private GameObject _grapplePoint; // grapple POI
    public static Grapplev2 Instance;


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
}
