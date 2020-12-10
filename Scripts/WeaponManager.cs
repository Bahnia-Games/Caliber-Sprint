using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class WeaponManager : MonoBehaviour
{
#pragma warning disable CS1030

    public Animator animator;
    private GunController gunController;
    private float weaponDeployTime;
    private string weaponDeployBoolName;

    private int selectedWeapon;
    private bool isAnyEquip;

    private GunController thisWeaponController;
    private GrenadeController thisGrenadeController;
    private Grapplev2 thisGrappleController;

    public GameObject thisGrenade;
    [HideInInspector] public bool thisGrenadeEquip;

    //public GameObject thisGrenade2;

    public GameObject thisWeapon; //testing
    [HideInInspector] public bool thisWeaponEquip;

    public GameObject thisWeapon2;
    // GunController thisWeapon2GunController;

    public GameObject thisWeapon3;


    private bool stop;
    


    // Start is called before the first frame update
    void Start()
    {

        //mainly for testing purposes


        thisGrenadeEquip = true;

        isAnyEquip = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region grapple (g)
        if (Input.GetKeyDown(KeyCode.G) && thisGrappleController.grappleEquipped == false)
        {
            thisGrappleController.EquipGrapple(true);
        }
        #endregion

        #region flash (q)
        if (Input.GetKeyDown(KeyCode.Q) && thisGrenadeEquip)
        {
            stop = false;
            thisGrenade.SetActive(true);
            thisGrenadeController = thisGrenade.GetComponent<GrenadeController>();
            StartCoroutine(SwitchGAndBack());
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {

        }

        #endregion

        #region weapon 1 (1, ID 0)

        if (!isAnyEquip && Input.GetKeyDown(KeyCode.Alpha1))
        {
            thisWeaponController = thisWeapon.GetComponent<GunController>();
            SelectWeapon(0);
        } else
        {
            //i forgot what to put in these *shrug*
        }

        #endregion

        #region weapon 2 (2, ID 1)
        if (!isAnyEquip && Input.GetKeyDown(KeyCode.Alpha2))
        {
            thisWeaponController = thisWeapon2.GetComponent<GunController>();
            SelectWeapon(1);
        } else
        {

        }
        #endregion

        #region weapon 3 (3, ID 2)

        if (!isAnyEquip && Input.GetKeyDown(KeyCode.Alpha3))
        {
            thisWeaponController = thisWeapon3.GetComponent<GunController>();
            SelectWeapon(2);
        }

        #endregion

        #region undeploy

        if (Input.GetKeyDown(KeyCode.E) && isAnyEquip)
        {
            isAnyEquip = false;
            StartCoroutine(thisWeaponController.Deploy(false));
        }

        #endregion

    }

    void pickup()
    {
        // to be added in version 3 or whenever i get not lazy
    }

    void SelectWeapon(int weaponID)
    {
        isAnyEquip = true;
        //thisWeaponController = thisWeapon.GetComponent<GunController>();
        selectedWeapon = weaponID; // I ALMOST DELETED THIS!!! FUTURE SELF DO NOT DELETE!
        
        // *sigh*
        if(weaponID == 0)
            thisWeapon.SetActive(true);
        if(weaponID == 1)
            thisWeapon2.SetActive(true);
        if(weaponID == 2)
            thisWeapon3.SetActive(true);
        // and so on, continue support

        weaponDeployTime = thisWeaponController.weaponDeployTime;
        weaponDeployBoolName = thisWeaponController.weaponDeployBoolName;

        animator.SetBool(weaponDeployBoolName, true);
    }

    IEnumerator SwitchGAndBack()
    {
        bool hasWeapon;
        if (isAnyEquip)
        {
            hasWeapon = true;
            StartCoroutine(thisWeaponController.Deploy(false));
            yield return new WaitForSeconds(thisWeaponController.weaponDeployTime);
        }
        else
            hasWeapon = false;
        StartCoroutine(thisGrenadeController.HoldGrenade(true)); //very slow, optimize HoldGrenade()
        //yield return new WaitForSeconds(thisGrenadeController.grenadeEquipTime + 0.05f);
        StartCoroutine(thisGrenadeController.Flash()); // coroutine animation doesnt play, optimize times, reduce animation time, and debug this
        yield return new WaitForSeconds(thisGrenadeController.throwAnimationTime);

        if (hasWeapon)
        {
            //#warning this dont work v
            //StartCoroutine(thisWeaponController.Deploy(true)); // as expected this doesnt work, see above method
            SelectWeapon(selectedWeapon);
            yield return new WaitForSeconds(thisWeaponController.weaponDeployTime);
        }

        StartCoroutine(thisGrenadeController.HoldGrenade(false));
        yield return new WaitForSeconds(thisGrenadeController.grenadeEquipTime);
        thisGrenade.SetActive(false);
    }
        

}
