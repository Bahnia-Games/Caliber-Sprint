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

        thisWeaponController = thisWeapon.GetComponent<GunController>();

        thisGrenadeEquip = true;

        isAnyEquip = false;
    }

    // Update is called once per frame
    void Update()
    {


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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeapon(0);
        } else
        {
            //i forgot what to put in these *shrug*
        }

        #endregion

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            thisWeaponController = thisWeapon2.GetComponent<GunController>();
            selectedWeapon = 1;
            thisWeapon2.SetActive(true);

            weaponDeployTime = thisWeaponController.weaponDeployTime;
            weaponDeployBoolName = thisWeaponController.weaponDeployBoolName;

            animator.SetBool(weaponDeployBoolName, true);
        } else
        {

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            thisWeaponController = thisWeapon3.GetComponent<GunController>();
            selectedWeapon = 2;
            thisWeapon3.SetActive(true);

            weaponDeployTime = thisWeaponController.weaponDeployTime;
            weaponDeployBoolName = thisWeaponController.weaponDeployBoolName;
            animator.SetBool(weaponDeployBoolName, true);
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            isAnyEquip = false;
            StartCoroutine(thisWeaponController.Deploy(false));
        }

    }

    void pickup()
    {

    }

    void SelectWeapon(int weaponID)
    {
        isAnyEquip = true;
        thisWeaponController = thisWeapon.GetComponent<GunController>();
        selectedWeapon = weaponID;
        thisWeapon.SetActive(true);

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
        yield return new WaitForSeconds(thisGrenadeController.grenadeEquipTime + 0.05f);
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
