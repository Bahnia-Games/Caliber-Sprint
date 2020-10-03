using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class WeaponManager : MonoBehaviour
{
    private bool check;
    private bool switchBack;
    public Animator animator;
    private GunController gunController;
    private float weaponDeployTime;
    private string weaponDeployBoolName;

    private int selectedWeapon;

    private GunController thisWeaponController;
    private GrenadeController thisGrenadeController;

    public GameObject thisGrenade;
    [HideInInspector] public bool thisGrenadeEquip;

    public GameObject thisWeapon; //testing
    [HideInInspector] public bool thisWeaponEquip;

    public GameObject thisWeapon2;
    // GunController thisWeapon2GunController;

    public GameObject thisWeapon3;

    



    // Start is called before the first frame update
    void Start()
    {
        thisGrenadeEquip = true;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.G) && thisGrenadeEquip && !check)
        {
            StartCoroutine(SwitchGAndBack());
        }

        if (check && Input.GetKeyDown(KeyCode.G))
        {
            switchBack = true;
            check = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectWeaponZero();
        } else
        {
            //i forgot what to put in these *shrug*
        }

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

        
    }

    void pickup()
    {

    }

    void SelectWeaponZero()
    {
        thisWeaponController = thisWeapon.GetComponent<GunController>();
        selectedWeapon = 0;
        thisWeapon.SetActive(true);

        weaponDeployTime = thisWeaponController.weaponDeployTime;
        weaponDeployBoolName = thisWeaponController.weaponDeployBoolName;

        animator.SetBool(weaponDeployBoolName, true);
    }

    IEnumerator SwitchGAndBack()
    {
        if (!check)
        {
            StartCoroutine(thisWeaponController.Deploy(false));
            yield return new WaitForSeconds(thisWeaponController.weaponDeployTime);
        }
        StartCoroutine(thisGrenadeController.HoldGrenade(true));
        yield return new WaitForSeconds(thisGrenadeController.grenadeEquipTime);
        check = true;

        if (switchBack)
        {
            StartCoroutine(thisGrenadeController.HoldGrenade(false));
            yield return new WaitForSeconds(thisGrenadeController.grenadeEquipTime);
            StartCoroutine(thisWeaponController.Deploy(false));
            yield return new WaitForSeconds(thisWeaponController.weaponDeployTime);
        }
        

    }
        

}
