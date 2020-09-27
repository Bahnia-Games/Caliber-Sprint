using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

#pragma warning disable CS0108
public class GrenadeController : MonoBehaviour
{
    public Camera camera;
    public Animator animator;
    public GameObject thrownGrenade;
    private GameObject thrownGrenadeGO;
    public string grenadeType;
    public bool isImpact;
    public float fuzeTime;
    public float flashTime;
    public float highBlindTime;
    public float loBlindTime;
    public float grenadeEquipTime;
    public float throwForce;
    //public ParticleSystem particle;
    //public Light explosionLight;
    public LayerMask playerLayer;
    public float loCheckRad;
    public float highCheckRad;
    public GameObject heldGrenadeMesh;
    public GameObject flashOverlay;

    private bool hasGrenade;
    private bool isExploding;
    private bool isGrenadeHeld;

    //public GunController gunController;

    // Start is called before the first frame update
    void Start()
    {
        if (grenadeType == null)
        {
            Debug.Log("No grenade type selected, defaulting to frag...");
            grenadeType = "frag";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isExploding && Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(HoldGrenade(true));
        } else if (Input.GetKeyUp(KeyCode.G))
        {
            StartCoroutine(HoldGrenade(false));
        }

        if(isGrenadeHeld && Input.GetMouseButtonDown(1))
        {
            StartCoroutine(Flash());
        }
    }

    IEnumerator HoldGrenade(bool equip)
    {
        if (equip)
        {
            animator.SetBool("isGrenadeHeld", true);
            yield return new WaitForSeconds(grenadeEquipTime);
            isGrenadeHeld = true;
        } if (!equip)
        {
            animator.SetBool("isGrenadeHeld", false);
            yield return new WaitForSeconds(grenadeEquipTime);
            isGrenadeHeld = false;
        }
        
    }

    IEnumerator Flash()
    {
        isExploding = true;
        Quaternion rot = Quaternion.Euler(camera.transform.forward);
        Instantiate(thrownGrenade, camera.transform.position, rot);
        thrownGrenadeGO = GameObject.FindGameObjectWithTag("thrown_flash");
        Rigidbody thrownGrenadeRB = thrownGrenadeGO.GetComponent<Rigidbody>();
        //Light thrownGrenadeL = thrownGrenadeGO.GetComponent<Light>();
        ParticleSystem thrownGrenadePS = thrownGrenadeGO.GetComponent<ParticleSystem>();
        thrownGrenadeRB.AddForce(Vector3.forward * throwForce);
        yield return new WaitForSeconds(fuzeTime);
        bool lo = Physics.CheckSphere(thrownGrenadeGO.transform.position, loCheckRad);
        bool hi = Physics.CheckSphere(thrownGrenadeGO.transform.position, highCheckRad);
        //thrownGrenadeL.enabled = true;
        thrownGrenadePS.Play();
        yield return new WaitForSeconds(flashTime);
        //thrownGrenadeL.enabled = false;
        if (lo)
        {
            flashOverlay.SetActive(true);
            yield return new WaitForSeconds(loBlindTime);
            flashOverlay.SetActive(false);
        }
        if (hi)
        {
            flashOverlay.SetActive(true);
            yield return new WaitForSeconds(highBlindTime);
            flashOverlay.SetActive(false);
        }
        
    }

    IEnumerator Frag()
    {
        yield return new WaitForSeconds(fuzeTime);
    }
}
