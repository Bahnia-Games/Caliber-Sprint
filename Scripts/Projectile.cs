using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string projectileType;

    private void Awake()
    {
        if (projectileType != "grapling hook")
            Debug.LogError("Invalid projectile type on" + this.gameObject.name + "@Projectile.cs L12");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (projectileType == "grapple")
        {
            Grapple grapple = this.GetComponentInParent<Grapple>();
            grapple.Collided(collision.collider);
        }
    }

}
