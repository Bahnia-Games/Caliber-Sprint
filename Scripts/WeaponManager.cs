using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    



    // Start is called before the first frame update
    void Start()
    {
        
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
        } else
        {

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            thisWeaponController = thisWeapon2.GetComponent<GunController>();
            selectedWeapon = 1;
            thisWeapon2.SetActive(true);

            weaponDeployTime = thisWeaponController.weaponDeployTime;
            weaponDeployBoolName = thisWeaponController.weaponDeployBoolName;
        } else
        {

        }

        
        StartCoroutine(Deploy(weaponDeployTime, weaponDeployBoolName));
    }


    IEnumerator Deploy(float deployTime, string weaponDeployBoolName)
    {
        
        animator.SetBool(weaponDeployBoolName, true);

        yield return new WaitForSeconds(0.0001f);

    }

}
