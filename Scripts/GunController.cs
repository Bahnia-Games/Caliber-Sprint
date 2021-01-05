using System;
using System.Collections;
using System.Collections.Generic;
//using System.Security.Policy; < cause errors, idfk why this is here anyway
//using UnityEditorInternal;
using UnityEngine;

#pragma warning disable CS0108

public class GunController : MonoBehaviour
{

    #region global variables (weapon parameters and other stuff i guess)

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
    private bool isAltAds;

    private bool isEmpty;
    private bool isTac;

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

    public PlayerAnimationController playerAnimationController;
    #endregion

    #region global local variables

    private float randX;
    private float randY;
    private float randZ;

    private bool fireStage;

    private SightState sightState;

    #endregion

    #region animation states

    // THESE ARE ANIMATOR STATE NAMES!!! NOT ANIMATION CLIP NAMES!!!!!!
    public string DEPLOY;
    public string UN_DEPLOY;
    public string IDLE;
    public string ADS;
    public string UN_ADS;
    public string ADS_IDLE;
    public string ADS_SWITCH_SIGHT;
    public string ADS_UN_SWITCH_SIGHT;
    public string ADS_ALT_IDLE;
    public string FIRE;
    public string FIRE_2;
    public string ADS_FIRE;
    public string ADS_FIRE_2;
    public string RELOAD;
    public string TAC_RELOAD;


    #endregion

    private void Awake()
    {
        if (DEPLOY != null)
        {
            //animator.SetBool(weaponDeployBoolName, true);
        } else
            Debug.Log("No Deploy Animation Added!");

        if (fireMode == null) //Check if no fire mode is entered. Default is Semi Auto
            fireMode = "SemiAuto";


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
            StartCoroutine(Shoot());
        if (Input.GetKey(KeyCode.Mouse0) && fireMode == "Auto" && !isReload && !isFire) //Fire every frame mouse is held (Auto)
            StartCoroutine(Shoot());

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

        if (Input.GetKeyDown(KeyCode.Mouse1) && !isAds && !isFire && !isReload) // ads
            StartCoroutine(AimDownSight(SightState.ADS));
        if (Input.GetKeyDown(KeyCode.T) && ADS_SWITCH_SIGHT != null && isAds && !isFire && !isReload)
            StartCoroutine(AimDownSight(SightState.SightToggleOn));
        else if (Input.GetKeyDown(KeyCode.T) && isAltAds && ADS_UN_SWITCH_SIGHT != null && !isFire && !isReload)
            StartCoroutine(AimDownSight(SightState.SightToggleOff));
        #warning might cause issues with transitions, switch to !getkey and nest if else v ^
        if (Input.GetKeyUp(KeyCode.Mouse1) && !isFire && !isReload && !isFire && !isReload) 
            StartCoroutine(AimDownSight(SightState.UnADS));

        if (Input.GetKeyUp(KeyCode.Mouse1) && isReload) // check for ads changes while reloading
            isAds = false;
        if (Input.GetKeyDown(KeyCode.Mouse1) && isReload)
            isAds = true;
        #endregion

        #region undeploy

        if (Input.GetKeyDown(KeyCode.E))
            StartCoroutine(Deploy(false));

        #endregion
    }

    private IEnumerator Shoot()
    {
        isFire = true;
        currentAmmo--; //subtract 1 bullet
        if (!isAds)
        {
            if (fireStage)
            {
                playerAnimationController.PlayAnim(FIRE);
                fireStage = false;
            }
            if (!fireStage)
            {
                playerAnimationController.PlayAnim(FIRE_2);
                fireStage = true;
            }
        }
        if (isAds)
        {
            if (fireStage)
            {
                playerAnimationController.PlayAnim(ADS_FIRE);
                fireStage = false;
            }
            if (!fireStage)
            {
                playerAnimationController.PlayAnim(ADS_FIRE_2);
                fireStage = true;
            }
        }
            

        #region muzzle flashing

        StartCoroutine(SpriteMuzzleFlash()); //da

        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (muzzleFlash2 != null)
            muzzleFlash2.Play();

        if (muzzleFlash3 != null)
            muzzleFlash3.Play();

        if (muzzleLight != null)
            muzzleLight.gameObject.SetActive(true);

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
                target.takeDamage(damage);

            if (hit.rigidbody != null)
                hit.rigidbody.AddForce(-hit.normal * impactForce);

            GameObject impactGameObj = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGameObj, 1f);

            #endregion

            #region impact effects
            bool isOtherType = false;
            // hitObject = hit.transform.GetComponent<GameObject>();
            if (hit.transform.tag == "glass" && glassBulletImpact != null)
            {
                InstantiateImpact(ImpactType.glass);
                isOtherType = true;
            }
            if (hit.transform.tag == "enemy" && fleshBulletImpact != null)
            {
                InstantiateImpact(ImpactType.enemy);
                isOtherType = true;
            }
            if (hit.transform.tag == "metal" && metalBulletImpact != null)
            {
                InstantiateImpact(ImpactType.metal);
                isOtherType = true;
            }

            if (hit.point != null && defaultBulletImapct != null && !isOtherType)
                InstantiateImpact(ImpactType.defaultt);
            #endregion
        }
        float del = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(del); //wait for fire delay

        muzzleLight.gameObject.SetActive(false);
        isFire = false;
    }
    private void InstantiateImpact(ImpactType type)
    {
        float _randZ = UnityEngine.Random.Range(360, -360);
        Vector3 posMod = hit.normal * 0.001f;
        Quaternion rotMod = Quaternion.LookRotation(hit.normal) * Quaternion.Euler(new Vector3(90, 0, 0)) * Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(360, -360), 0));
        Transform hitObject = hit.transform;
        if (type == ImpactType.glass)
        {
            GameObject impact = Instantiate(glassBulletImpact, hit.point + posMod, rotMod);
            impact.transform.parent = hitObject;
            Destroy(impact, instantiatedObjectLifetime);
        }
        if (type == ImpactType.enemy){
            GameObject impact = Instantiate(fleshBulletImpact, hit.point + posMod, rotMod);
            impact.transform.parent = hitObject;
            Destroy(impact, instantiatedObjectLifetime);
        }
        if (type == ImpactType.metal)
        {
            GameObject impact = Instantiate(metalBulletImpact, hit.point + posMod, rotMod);
            impact.transform.parent = hitObject;
            Destroy(impact, instantiatedObjectLifetime);
        }
        if (type == ImpactType.defaultt)
        {
            GameObject impact = Instantiate(defaultBulletImapct, hit.point + posMod, rotMod);
            impact.transform.parent = hitObject;
            Destroy(impact, instantiatedObjectLifetime);
        }
        if (type != ImpactType.glass && type != ImpactType.enemy && type != ImpactType.metal && type != ImpactType.defaultt)
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
    private IEnumerator ShellEject(SpecialState specialState = SpecialState.SFSNull)
    {
        yield return new WaitForSeconds(ejectionTuneTime);
        if (fireMode != "derringer")
        {
            Instantiate(shellCasingPrefab, shellCasingInstantiationPoint.position, Quaternion.Euler(ejectRotationTune));
            GameObject thisShellCaseGO = GameObject.FindGameObjectWithTag("active_shell");
            Rigidbody thisShellCaseRB = thisShellCaseGO.GetComponent<Rigidbody>();
            thisShellCaseRB.AddForce(shellCasingInstantiationPoint.up * shellEjectionForce);
            Vector3 randomRot = new Vector3(UnityEngine.Random.Range(ejectShellTorque.x, ejectShellTorque.y), UnityEngine.Random.Range(ejectShellTorque.x, ejectShellTorque.y), UnityEngine.Random.Range(ejectShellTorque.x, ejectShellTorque.y)); // leave as is
            thisShellCaseRB.AddTorque(randomRot);
            Destroy(thisShellCaseGO, instantiatedObjectLifetime);
            thisShellCaseGO.tag = "spent_shell";
        } else if (fireMode == "derringer") // note this only happens during reload
        {
            if (specialState == SpecialState.derringerTac)
            {

            } if (specialState == SpecialState.derringerEmpty)
            {

            }
        }


    }
    private IEnumerator Reload(SpecialState specialFireState = SpecialState.SFSNull)
    {
        isReload = true;
        if (fireMode != "derringer")
        {
            if (isEmpty && !isAds) // empty reload
            {
                playerAnimationController.PlayAnim(RELOAD);
                float del = animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(del);
                //HandleIdle(IdleState.hip);
            }
            if (isTac && !isAds) // tac reload
            {
                playerAnimationController.PlayAnim(TAC_RELOAD);
                float del = animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(del);
                //HandleIdle(IdleState.hip);
            }

            if (isEmpty && isAds) // ads empty reload
            { // this needs to be simplified...
                playerAnimationController.PlayAnim(UN_ADS);
                float del = animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(del);
                playerAnimationController.PlayAnim(RELOAD);
                float del1 = animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(del1);
                //HandleIdle(IdleState.ads);
            }
            if (isTac && isAds) // ads tac reload ((borked atm))
            {
                playerAnimationController.PlayAnim(UN_ADS);
                float del = animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(del);
                playerAnimationController.PlayAnim(TAC_RELOAD);
                float del1 = animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(del1);
                //HandleIdle(IdleState.ads);
            }
        }
        else if (fireMode == "derringer"){ // implimented later
            // reload stuff
            if (currentAmmo == 1)
            {
                StartCoroutine(ShellEject(SpecialState.derringerTac));
            }
            if (currentAmmo == 0)
            {
                StartCoroutine(ShellEject(SpecialState.derringerEmpty));
            }
        }

        if (isAds) // is the player still ads?
        {
            playerAnimationController.PlayAnim(ADS);
            float del = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(del);
        }

         //wait for reload delay Hi Nate! -Gabe
        currentAmmo = magSize; //refil mag
        isTac = false;
        isEmpty = false;
        isReload = false;
    }
    private IEnumerator AimDownSight(SightState _sightState)
    {
       if (_sightState == SightState.ADS) // call aim down sight
        {
            Debug.Log("Aiming down...");
            isAds = true;
            playerAnimationController.PlayAnim(ADS);
            crosshair.SetActive(false);
            sightState = SightState.ADS;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            //HandleIdle(IdleState.ads);
        }
       if (_sightState == SightState.UnADS) // call go back to hip
        {
            isAds = false;
            playerAnimationController.PlayAnim(UN_ADS);
            crosshair.SetActive(true);
            sightState = SightState.UnADS;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            //HandleIdle(IdleState.hip);
        }
       if (_sightState == SightState.SightToggleOn && isAds) // if aiming, call secondary sight
        {
            isAltAds = true;
            playerAnimationController.PlayAnim(ADS_SWITCH_SIGHT);
            sightState = SightState.SightToggleOn;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            //HandleIdle(IdleState.secondaryAds);
        }
       if (_sightState == SightState.SightToggleOff && isAds) // if aiming, call primary sight
        {
            isAltAds = false;
            playerAnimationController.PlayAnim(ADS_UN_SWITCH_SIGHT);
            sightState = SightState.SightToggleOff;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            //HandleIdle(IdleState.ads);
        }
    }
    public IEnumerator Deploy(bool deploy)
    {
        if (deploy)
        {
            playerAnimationController.PlayAnim(DEPLOY);
            float del = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(del);
            isEquip = true;
        }

        if (!deploy)
        {
            playerAnimationController.PlayAnim(UN_DEPLOY);
            float del = animator.GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(del);
            isEquip = false;
            gameObject.SetActive(false);
        }

    }
    private enum IdleState
    {
        hip,
        ads,
        secondaryAds
    }
    private enum ImpactType
    {
        glass,
        enemy,
        metal,
        defaultt
    }
    private enum SightState
    {
        ADS,
        UnADS,
        SightToggleOn,
        SightToggleOff
    }
    private enum SpecialState
    {
        SFSNull,
        derringerTac,
        derringerEmpty
    }
    private void HandleIdle(IdleState state) // depricated
    {
        if (state == IdleState.hip)
            playerAnimationController.PlayAnim(IDLE); 
        if (state == IdleState.ads)
            playerAnimationController.PlayAnim(ADS_IDLE); Debug.Log("idle ads");
        if (state == IdleState.secondaryAds)
            playerAnimationController.PlayAnim(ADS_ALT_IDLE);
    }

}
