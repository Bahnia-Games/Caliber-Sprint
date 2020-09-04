using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS0108

public class GunController : MonoBehaviour
{
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
    private int currentAmmo;
    public string fireMode = "SemiAuto";
    public Camera camera;
    public Animator animator;

    public ParticleSystem muzzleFlash;
    public ParticleSystem muzzleFlash2;
    public ParticleSystem muzzleFlash3;
    public Light muzzleLight;
    public GameObject impactEffect;
    private int layerMask = 1 << 8;
    public GameObject crosshair;

    private bool isFire;
    private bool isReload;
    private bool isAds;

    private bool isEmpty;
    private bool isTac;

    public string weaponDeployBoolName;
    public float weaponDeployTime;
		
		private bool bulletFired;
		private GameObject projectileGameObj;
		
		[Header("Do not use if Ballistic isnt your fire mode type!")]
		public float bulletForce;
		public string projectileType;
		public GameObject projectilePrefab;
		public Transform muzzleTransform;

    private void Awake()
    {
        if (weaponDeployBoolName != null)
        {
            animator.SetBool(weaponDeployBoolName, true);
        } else
        {
            Debug.Log("No Deploy Animation Added!");
        }

        if (fireMode == null) //Check if no fire mode is entered. Default is Semi Auto
        {
					debug.Log("No firemode set (is it named peoperly? Defaulting to SemiAuto");
            fireMode = "SemiAuto";
        }
				
				if (fireMode == "Ballistic" && bulletForce == null) {
					debug.Log("No bullet force set (did you add it in the inspector?), defaulting to 100")
						bulletForce = 100;
				}
				
				if (fireMode == "Ballistic" && projectileType == null){
					debug.Log("No projectile type entered (did you misspell it or forget to add it?) Defaulting to Bullet");
					peojectileType = "Bullet";
				}
        

    }

    void Start() //sex
    {
        currentAmmo = magSize;

    }
		
		// like update but for physics
		private void FixedUpdate(){
			if(bulletFired) {
				CheckBallisticState();
			}
		}
		

    void Update()
    {
        #region firing
        // SA Hitscan
        if (Input.GetKeyDown(KeyCode.Mouse0) && fireMode == "SemiAuto" && !isReload && !isFire) //Fire everytime mouse is clicked (Semi Auto)
        {
            StartCoroutine(Shoot());
        }
				// A Hitscan
        if (Input.GetKey(KeyCode.Mouse0) && fireMode == "Auto" && !isReload && !isFire) //Fire every frame mouse is held (Auto)
        {
            StartCoroutine(Shoot());
        }
				// projectile based firing
				if (Input.GetKey(KeyCode.Mouse0) && fireMode == "Ballistic" && !isReload && !isFire) //Fire Ballistic (Semi Auto only, this was intended for heavy rifles)
				{
					StartCoroutine(BallisticFire(projectileType));
				}

        #endregion

        #region reloading

        if (Input.GetKeyDown(KeyCode.R) && isFire == false && currentAmmo >= 1 && currentAmmo != magSize) // if the gun isnt firing and r is pressed, determine reload type
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

        #region testStuff

        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Undeploy());
        }

        #endregion
    }

    IEnumerator Shoot()
    {
        isFire = true;
        currentAmmo--; //subtract 1 bullet

        if (!isAds)
        {
            animator.SetBool("isFire", true);
        }
        if (isAds)
        {
            animator.SetBool("isAdsFire", true);
        }

        #region muzzle flashing

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

        yield return new WaitForSeconds(0.001f);

        muzzleLight.gameObject.SetActive(false);
        #endregion

        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range, layerMask))
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
        }

        yield return new WaitForSeconds(fireDelay); //wait for fire delay

        muzzleLight.gameObject.SetActive(false);
        animator.SetBool("isFire", false);
        animator.SetBool("isAdsFire", false);
        isFire = false;
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
        isReload = false;
        currentAmmo = magSize; //refil mag
        isTac = false;
        isEmpty = false;
    }

    IEnumerator Undeploy()
    {
        animator.SetBool(weaponDeployBoolName, false);

        yield return new WaitForSeconds(weaponDeployTime);
        this.gameObject.SetActive(false);
    }
		
		IENumerator BallisticFire(string projectileType){
			isFire = true;
			animator.SetBool("isFire", true);
			
			projectileGameObj = Instantiate(projectilePrefab, muzzleTransform.position, muzzleTransform.position.forward);
			bulletFired = true;
			
			Collider projectileCollider =  projectileGameObj.GetComponent<Collider>();
			
			if(projectileType == "Bullet" && projectileCollider.isCollided){
				bulletFired = false;
			}
			
			yield return new WaitForSeconds(fireDelay);
			
		}
		
		void CheckBallisticState(){
			Collider projectileCollider = ProjectileGameObj.getComponent<Collider>();
		}

}
