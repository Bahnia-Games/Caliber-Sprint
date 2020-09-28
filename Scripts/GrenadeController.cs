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
    public GameObject kinematicHandle;
    public GameObject thrownGrenadeHandle;
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

    public float destroyTime;

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
        } /*else if (Input.GetKeyUp(KeyCode.G))
        {
            StartCoroutine(HoldGrenade(false));
        }*/

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
        Debug.Log("fired");
        isExploding = true;
        Quaternion rot = Quaternion.Euler(camera.transform.forward);
        Instantiate(thrownGrenade, transform.position, rot);
        Instantiate(thrownGrenadeHandle, transform.position, rot);
        //Rigidbody kinematicHandleRB = kinematicHandle.GetComponent<Rigidbody>();
        //kinematicHandleRB.isKinematic = false;
        thrownGrenadeGO = GameObject.FindGameObjectWithTag("thrown_flash");
        Rigidbody thrownGrenadeRB = thrownGrenadeGO.GetComponent<Rigidbody>();
        Light thrownGrenadeL = thrownGrenadeGO.GetComponentInChildren<Light>();
        ParticleSystem thrownGrenadePS = thrownGrenadeGO.GetComponentInChildren<ParticleSystem>();
        thrownGrenadeRB.AddForce(Vector3.forward * throwForce);
        yield return new WaitForSeconds(fuzeTime);
        bool lo = Physics.CheckSphere(thrownGrenadeGO.transform.position, loCheckRad, playerLayer);
        bool hi = Physics.CheckSphere(thrownGrenadeGO.transform.position, highCheckRad, playerLayer);
        //thrownGrenadeL.enabled = true;
        thrownGrenadePS.Play();
        Debug.Log(lo);
        Debug.Log(hi);
        yield return new WaitForSeconds(flashTime);
        thrownGrenadePS.Stop();
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

        thrownGrenadeGO.tag = "lol";
        if (destroyTime != 0)
        {
            //DestroyImmediate(thrownGrenadeGO, true);
            //DestroyImmediate(thrownGrenadeHandle, true);
            //Destroy(thrownGrenadeGO, destroyTime);
            //Destroy(thrownGrenadeHandle, destroyTime);
        }


        
    }

    IEnumerator Frag()
    {
        yield return new WaitForSeconds(fuzeTime);
    }
}
