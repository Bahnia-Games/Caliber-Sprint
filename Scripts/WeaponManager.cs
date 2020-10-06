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

        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(check);

        #region flash (q)
        if (Input.GetKeyDown(KeyCode.Q) && thisGrenadeEquip && !check)
        {
            stop = false;
            thisGrenade.SetActive(true);
            thisGrenadeController = thisGrenade.GetComponent<GrenadeController>();
            StartCoroutine(SwitchGAndBack());
        }

        if (check && Input.GetKeyDown(KeyCode.Q))
        {
            switchBack = true;
            check = false;
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

        
    }

    void pickup()
    {

    }

    void SelectWeapon(int weaponID)
    {
        thisWeaponController = thisWeapon.GetComponent<GunController>();
        selectedWeapon = weaponID;
        thisWeapon.SetActive(true);

        weaponDeployTime = thisWeaponController.weaponDeployTime;
        weaponDeployBoolName = thisWeaponController.weaponDeployBoolName;

        animator.SetBool(weaponDeployBoolName, true);
    }

    IEnumerator SwitchGAndBack()
    {
        

        if (!check && !stop)
        {
            StartCoroutine(thisWeaponController.Deploy(false));
            yield return new WaitForSeconds(thisWeaponController.weaponDeployTime);
            stop = true;
            
        }

        StartCoroutine(thisGrenadeController.HoldGrenade(true));
        yield return new WaitForSeconds(thisGrenadeController.grenadeEquipTime);
        check = true;

        if (switchBack)
        {
            StartCoroutine(thisGrenadeController.HoldGrenade(false));
            yield return new WaitForSeconds(thisGrenadeController.grenadeEquipTime);
            thisGrenade.SetActive(false);
            StartCoroutine(thisWeaponController.Deploy(false));
            yield return new WaitForSeconds(thisWeaponController.weaponDeployTime);
        }
        

    }
        

}
