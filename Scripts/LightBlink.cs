using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightblink : MonoBehaviour
{
    // yes i made a script to flash a light... no im not stupid... i didnt want to make a shader for it shut the fuck up
    public GameObject lightGameObject;
    [HideInInspector] public Light light;
    [HideInInspector] public Material lightMat;
    public float flashSpeed;
    [HideInInspector] public bool isflash;
    [HideInInspector] public bool isSteady;
    // Start is called before the first frame update
    void Start()
    {
        light = lightGameObject.GetComponent<Light>();
        lightMat = lightGameObject.GetComponent<Material>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isflash)
        {

        }

        if (isSteady)
        {

        }
    }
}
