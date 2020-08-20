using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//using System;

/*
#pragma warning disable
[CLSCompliant(false)]
*/

public class WeaponManager : MonoBehaviour
{

    public Animator animator;
    private GunController gunController;
    private float weaponDeployTime;
    private string weaponDeployBoolName;

    private int selectedWeapon;

    public GameObject thisWeapon; //testing
    private GunController thisWeaponController; //testing

    public GameObject thisWeapon2;
    // GunController thisWeapon2GunController;

    public GameObject thisWeapon3;

    



    // Start is called before the first frame update
    void Start()
    {
        //animator.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            thisWeaponController = thisWeapon.GetComponent<GunController>();
            selectedWeapon = 0;
            thisWeapon.SetActive(true);

            weaponDeployTime = thisWeaponController.weaponDeployTime;
            weaponDeployBoolName = thisWeaponController.weaponDeployBoolName;

            animator.SetBool(weaponDeployBoolName, true);
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

        
        //StartCoroutine(Deploy(weaponDeployTime, weaponDeployBoolName));
    }

    //change this to a regular fucntion whenever... its cleaner that way but idrc
    IEnumerator Deploy(float deployTime, string weaponDeployBoolName) 
    {
        //animator.SetBool(weaponDeployBoolName, true);

        yield return new WaitForSeconds(0.0001f);

    }

}
