using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//most broken piece of shit code on the fucking planet
//^fax
public class SemiAutoWeapon : MonoBehaviour
{
    [Header("Depricated")]

    public float damage;
    public float range;
    public Camera camera;
    private int layerMask = 1 << 8;
    public float firerate = 1f;

    public int maxAmmo;
    private int currentAmmo;
    public float reloadTime;
    private bool isReload;
    private bool isTacReload;

    public ParticleSystem muzzleFlash;
    public ParticleSystem muzzleFlash2;
    public ParticleSystem muzzleFlash3;
    public GameObject impactEffect;
    public float impactForce = 50f;
    public bool isShoot;
    public string weaponType;
    public Animator animator;
    private float nextTimeToFire = 0f;



    public void Start()
    {
        currentAmmo = maxAmmo;
    }

    public void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentAmmo);   
        if (weaponType == "NH9MKII" || weaponType == "NH9" || weaponType == "NH-9")
        {
            NH9MKIIFire();
            NH9MKIIReload();
        }

        if (isReload)
        {
            return;
        }

        if(currentAmmo <= 1)
        {

            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time >= nextTimeToFire && !isReload)
        {
            nextTimeToFire = Time.time + 1f / firerate;
            shoot();
            isShoot = true;
            
        } else
        {
            isShoot = false;
            
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            isTacReload = true;
            StartCoroutine(Reload());
        }
        

    }

    void shoot()
    {
        RaycastHit hit;

        currentAmmo--;

        muzzleFlash.Play();

        if (muzzleFlash2 != null)
        {
            muzzleFlash2.Play();
        }
        if (muzzleFlash3 != null)
        {
            muzzleFlash3.Play();
        }

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range, layerMask))
        {
            //Debug.Log(hit.transform.name);

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
    }

    IEnumerator Reload()
    {
        //Debug.Log("Reload Fired");
        
        isShoot = false;

        if (!isTacReload)
        {
            isReload = true;
        }

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReload = false;
        isTacReload = false;

        //this v is a bug workaround please fix later k thx 
        disableFire();
    }

    void disableFire()
    {
        animator.SetBool("isNH9MKIIFire", false);
    }

    void NH9MKIIFire()
    {
        if (isShoot && currentAmmo > 0)
        {
            animator.SetBool("isNH9MKIIFire", true);
        }
        

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.SetBool("isNH9MKIIFire", false);
        }
    }

    void NH9MKIIReload()
    {
        if (isReload)
        {
            animator.SetBool("isNH9MKIIEmptyReload", true);

        }
        if (!isReload)
        {
            animator.SetBool("isNH9MKIIEmptyReload", false);
        }

        if (isTacReload)
        {
            animator.SetBool("isNH9MKIIReload", true);
        }
        if (!isTacReload)
        {
            animator.SetBool("isNH9MKIIReload", false);
        }
    }
}
