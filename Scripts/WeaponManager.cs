using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int selectedWeapon = 0;
    public Animator animator;
    private bool time;
    public GameObject weapon1;
    public float weapon1UndeployTime = 0.1f;
    public GameObject weapon2;
    public float weapon2UndeployTime = 0.1f;
    public GameObject weapon3;
    public float weapon3UndeployTime = 0.1f;
    public GameObject weapon4;
    public float weapon4UndeployTime = 0.1f;
    public GameObject weapon5;
    public float weapon5UndeployTime = 0.1f;
    public GameObject weapon6;
    public float weapon6UndeployTime = 0.1f;
    public GameObject weapon7;
    public float weapon7UndeployTime = 0.1f;
    public GameObject weapon8;
    public float weapon8UndeployTime = 0.1f;
    public GameObject weapon9;
    public float weapon9UndeployTime = 0.1f;
    public GameObject weapon10;
    public float weapon10UndeployTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {


        #region le repetitive if statements have arrived

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5) && transform.childCount >= 5)
        {
            selectedWeapon = 4;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6) && transform.childCount >= 6)
        {
            selectedWeapon = 5;
        }

        if (Input.GetKeyDown(KeyCode.Alpha7) && transform.childCount >= 7)
        {
            selectedWeapon = 6;
        }

        if (Input.GetKeyDown(KeyCode.Alpha8) && transform.childCount >= 8)
        {
            selectedWeapon = 7;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9) && transform.childCount >= 9)
        {
            selectedWeapon = 8;
        }

        if (Input.GetKeyDown(KeyCode.Alpha0) && transform.childCount >= 10)
        {
            selectedWeapon = 9;
        }

        #endregion

        StartCoroutine(SelectWeapon());
    }

    IEnumerator SelectWeapon()
    {
        if (selectedWeapon == 0)
        {
            weapon1.SetActive(true);
        } else
        {
            yield return new WaitForSeconds(weapon1UndeployTime);
            weapon1.SetActive(false);
        }
        if (selectedWeapon == 1)
        {
            weapon2.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds(weapon2UndeployTime);
            weapon2.SetActive(false);
        }

    }

}
