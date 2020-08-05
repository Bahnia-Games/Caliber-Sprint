using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoWeapon : MonoBehaviour
{
    public float damage;
    public float range;
    public Camera camera;
    private int layerMask = 1 << 8;
    public float firerate = 1f;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;
    public float impactForce = 50f;

    private float nextTimeToFire = 0f;


    public void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / firerate;
            shoot();
        }
    }

    void shoot()
    {
        RaycastHit hit;

        muzzleFlash.Play();

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, range, layerMask))
        {
            Debug.Log(hit.transform.name);

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
}
