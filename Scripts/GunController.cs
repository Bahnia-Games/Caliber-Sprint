using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;

#pragma warning disable CS0108

public class GunController : MonoBehaviour
{

    #region global variables (weapon parameters and other stuff i guess)

    //public string currentWeapon;
    public float reloadDelay = 1.0f;
    public float emptyReloadDelay = 1.0f;
    public float ADSReloadDelay = 1.0f;
    public float emptyADSRelaodDelay = 1.0f;
    public float fireDelay = 0.1f;
    public float damage = 1f;
    public float range = 1f;
    public int magSize = 1;
    public float impactForce = 1f;
    public float maxRandSpread = 0.1f;
    public float minRandSpread = -0.1f;
    public float maxRandADSSpread = 0.1f;
    public float minRandADSSpread = -0.1f;
    [SerializeField] private int currentAmmo;
    public string fireMode = "SemiAuto";
    public Camera camera;
    public Animator animator;

    public ParticleSystem muzzleFlash;
    public ParticleSystem muzzleFlash2;
    public ParticleSystem muzzleFlash3;
    public GameObject muzzleFlashSpriteGO;
    public float muzzleFlashLifetime;

    public GameObject fleshBulletImpact;
    public GameObject glassBulletImpact;
    public GameObject metalBulletImpact;
    public GameObject defaultBulletImapct;

    public float muzzleLightFlashTime;
    public Light muzzleLight;
    public GameObject impactEffect;
    private int layerMask = 1 << 8;
    public GameObject crosshair;

    [HideInInspector] public bool isFire;
    private bool isReload;
    private bool isAds;

    private bool isEmpty;
    private bool isTac;

    public string weaponDeployBoolName;
    public float weaponDeployTime;
    [HideInInspector] public bool isEquip;

    public GameObject shellCasingPrefab; // as GO to add cool shit later
    public Transform shellCasingInstantiationPoint;
    public float shellEjectionForce;
    public Vector3 ejectRotationTune;
    public Vector2 ejectShellTorque;
    public float ejectionTuneTime;
    public float instantiatedObjectLifetime;

    private GameObject hitObject;
    private RaycastHit hit;

    #endregion

    #region global local variables

    private float randX;
    private float randY;
    private float randZ;

    #endregion



    private void Awake()
    {
        if (weaponDeployBoolName != null)
        {
            //animator.SetBool(weaponDeployBoolName, true);
        } else
        {
            Debug.Log("No Deploy Animation Added!");
        }

        if (fireMode == null) //Check if no fire mode is entered. Default is Semi Auto
        {
            fireMode = "SemiAuto";
        }
        

    }

    void Start() //sex
    {
        isEquip = true;
        currentAmmo = magSize;

    }

    void Update()
    {
        #region firing

        if (Input.GetKeyDown(KeyCode.Mouse0) && fireMode == "SemiAuto" && !isReload && !isFire) //Fire everytime mouse is clicked (Semi Auto)
        {
            StartCoroutine(Shoot());
        }
        if (Input.GetKey(KeyCode.Mouse0) && fireMode == "Auto" && !isReload && !isFire) //Fire every frame mouse is held (Auto)
        {
            StartCoroutine(Shoot());
        }

        #endregion

        #region reloading

        if (Input.GetKeyDown(KeyCode.R) && !isFire && currentAmmo >= 1 && currentAmmo != magSize) // if the gun isnt firing and r is pressed, determine reload type
        {
            // does the magazoine have at least 1 bullet and is less than the max ammo size?
            isTac = true; // tactical reload
            StartCoroutine(Reload());
        }

        if (currentAmmo <= 0) // is the current ammo 0?
        {
            isEmpty = true; //mag empty reload
            StartCoroutine(Reload());
        }

        #endregion

        #region ads

        if (Input.GetKey(KeyCode.Mouse1)) //ADS
        {
            isAds = true;
            animator.SetBool("isAds", true);
            crosshair.SetActive(false);
        } else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            animator.SetBool("isAds", false);
            isAds = false;
            crosshair.SetActive(true);
        }

        #endregion

        #region undeploy

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Deploy(false));
        }

        #endregion
    }

    IEnumerator Shoot()
    {
        isFire = true;
        currentAmmo--; //subtract 1 bullet
        Debug.Log("Fired");

        if (!isAds)
        {
            animator.SetBool("isFire", true);
        }
        if (isAds)
        {
            animator.SetBool("isAdsFire", true);
        }

        #region muzzle flashing

        StartCoroutine(SpriteMuzzleFlash()); //da

        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        if (muzzleFlash2 != null)
        {
            muzzleFlash2.Play();
        }

        if (muzzleFlash3 != null)
        {
            muzzleFlash3.Play();
        }

        if (muzzleLight != null)
        {
            muzzleLight.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(muzzleLightFlashTime);

        muzzleLight.gameObject.SetActive(false);
        #endregion

        #region shell ejection

        StartCoroutine(ShellEject());

        #endregion

        #region bullet spread

        if (!isAds) // I swear if people suggest dynamic bullet spread im gonna cry, ffs this isnt csgo
        {
            randX = UnityEngine.Random.Range(maxRandSpread, minRandSpread);
            randY = UnityEngine.Random.Range(maxRandSpread, minRandSpread);
            randZ = UnityEngine.Random.Range(maxRandSpread, minRandSpread);
        } 
        if (isAds)
        {
            randX = UnityEngine.Random.Range(maxRandADSSpread, minRandADSSpread);
            randY = UnityEngine.Random.Range(maxRandADSSpread, minRandADSSpread);
            randZ = UnityEngine.Random.Range(maxRandADSSpread, minRandADSSpread);
        }

        Vector3 rand = new Vector3(randX, randY, randZ);

        #endregion

        #region hitscan

        //RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward + rand, out hit, range, layerMask))
        {
            target target = hit.transform.GetComponent<target>();

            if (target != null)
            {
                target.takeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            GameObject impactGameObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGameObj, 1f);

            #endregion

            #region impact effects

            // hitObject = hit.transform.GetComponent<GameObject>();
            if (hit.transform.tag == "glass" && glassBulletImpact != null)
            {
                InstantiateImpact("glass");
            }
            if (hit.transform.tag == "enemy" && fleshBulletImpact != null)
            {
                InstantiateImpact("enemy");
            }
            if (hit.transform.tag == "metal" && metalBulletImpact != null)
            {
                InstantiateImpact("metal");
            }

            if (hit.point != null && defaultBulletImapct != null)
            {
                InstantiateImpact("default");
            }

            #endregion
        }

        yield return new WaitForSeconds(fireDelay); //wait for fire delay

        muzzleLight.gameObject.SetActive(false);
        animator.SetBool("isFire", false);
        animator.SetBool("isAdsFire", false);
        isFire = false;
    }

    private void InstantiateImpact(string type)
    {
        Vector3 rotMod = new Vector3(90, 0, 0);
        Vector3 posMod = new Vector3(0, 0.01f, 0);
        if (type == "glass")
        {
            GameObject impact = Instantiate(glassBulletImpact, hit.point + posMod, Quaternion.LookRotation(hit.normal) * Quaternion.Euler(rotMod));
            Destroy(impact, instantiatedObjectLifetime);
        }
        if (type == "enemy"){
            GameObject impact = Instantiate(fleshBulletImpact, hit.point + posMod, Quaternion.LookRotation(hit.normal) * Quaternion.Euler(rotMod));
            Destroy(impact, instantiatedObjectLifetime);
        }
        if (type == "metal")
        {
            GameObject impact = Instantiate(metalBulletImpact, hit.point + posMod, Quaternion.LookRotation(hit.normal) * Quaternion.Euler(rotMod));
            Destroy(impact, instantiatedObjectLifetime);
        }
        if (type == "default")
        {
            GameObject impact = Instantiate(defaultBulletImapct, hit.point + posMod, Quaternion.LookRotation(hit.normal) * Quaternion.Euler(rotMod));
            Destroy(impact, instantiatedObjectLifetime);
        }
        if (type != "glass" && type != "enemy" && type != "metal" && type != "default")
            Debug.LogWarning("Cannot instantiate Impact Effect, invalid type @GunController.cs InstantiateImpact()");


    }
    private IEnumerator SpriteMuzzleFlash()
    {
        if (muzzleFlashSpriteGO != null)
        {
            muzzleFlashSpriteGO.SetActive(true);
            yield return new WaitForSeconds(muzzleFlashLifetime);
            muzzleFlashSpriteGO.SetActive(false);
        }
    }

    IEnumerator ShellEject()
    {
        yield return new WaitForSeconds(ejectionTuneTime);
        Instantiate(shellCasingPrefab, shellCasingInstantiationPoint.position, Quaternion.Euler(ejectRotationTune));
        GameObject thisShellCaseGO = GameObject.FindGameObjectWithTag("active_shell");
        Rigidbody thisShellCaseRB = thisShellCaseGO.GetComponent<Rigidbody>();
        thisShellCaseRB.AddForce(shellCasingInstantiationPoint.up * shellEjectionForce);
        Vector3 randomRot = new Vector3(UnityEngine.Random.Range(ejectShellTorque.x, ejectShellTorque.y), UnityEngine.Random.Range(ejectShellTorque.x, ejectShellTorque.y), UnityEngine.Random.Range(ejectShellTorque.x, ejectShellTorque.y)); // leave as is
        thisShellCaseRB.AddTorque(randomRot);
        Destroy(thisShellCaseGO, instantiatedObjectLifetime);
        thisShellCaseGO.tag = "spent_shell";

    }

    IEnumerator Reload()
    {
        isReload = true;

        if (isEmpty && !isAds)
        {
            animator.SetBool("isReload", true);
            yield return new WaitForSeconds(emptyReloadDelay);
        }
        if (isTac && !isAds)
        {
            animator.SetBool("isTacReload", true);
            yield return new WaitForSeconds(reloadDelay);
        }

        if(isEmpty && isAds)
        {
            animator.SetBool("isReload", true);
            yield return new WaitForSeconds(emptyADSRelaodDelay);
        }
        if (isTac && isAds)
        {
            animator.SetBool("isTacReload", true);
            yield return new WaitForSeconds(ADSReloadDelay);
        }



         //wait for reload delay Hi Nate! -Gabe

        animator.SetBool("isReload", false);
        animator.SetBool("isTacReload", false);
        currentAmmo = magSize; //refil mag
        isTac = false;
        isEmpty = false;
        isReload = false;
    }
    public IEnumerator Deploy(bool deploy)
    {
        if (deploy)
        {
            animator.SetBool(weaponDeployBoolName, true);
            yield return new WaitForSeconds(weaponDeployTime);
            isEquip = true;
        }

        if (!deploy)
        {
            animator.SetBool(weaponDeployBoolName, false);

            yield return new WaitForSeconds(weaponDeployTime);
            isEquip = false;
            this.gameObject.SetActive(false);
        }

    }

}
