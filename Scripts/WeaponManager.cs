using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] int selectedWeapon;
    public GameObject weaponHolder;
    public Animator animator;
    private GunController thisWeapon;
    private string weaponDeployBoolName;
    private float weaponDeployTime;
    [HideInInspector] public bool canFire;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }


        StartCoroutine(SelectWeapon());
    }

    IEnumerator SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in weaponHolder.transform)
        {
            if (i == selectedWeapon)
            {
                
                weapon.gameObject.SetActive(true);
                //thisWeapon = weaponHolder.GetComponentInChildren<GameObject>();
                thisWeapon.GetComponentInChildren<GunController>();
                //weaponDeployBoolName = GunController.deployAnimBoolName;
                weaponDeployTime = GunController.undeployTime;
                //animator.SetBool(weaponDeployBoolName, true);
                yield return new WaitForSeconds(weaponDeployTime);
                
                    //WHY WONT IT SHOW UP IAGDSIAVDIVAIDVIADBIB
                
            }
            else
            {
                animator.SetBool(weaponDeployBoolName, false);
                yield return new WaitForSeconds(weaponDeployTime);
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

    void pickup()
    {
        //TO BE IMPLIMENTED LATER!

    }
}
