using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string projectileType;
    public Grapple grappleScript;
    //GunController gunScript;

    private void Awake()
    {
        if (projectileType != "grappling hook")
            Debug.LogError("Invalid projectile type on" + this.gameObject.name + " @Projectile.cs L13");
        if (projectileType == "grappling hook" && grappleScript == null)
            Debug.LogError("Grapple Script component unnasigned! " + this.gameObject.name + " @projectile.cs L16");
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (projectileType == "grappling hook")
        {
            ContactPoint contact = collision.contacts[0]; // congrats the first array in the entirety of caliber sprint and the first one in any of my games
            Collider collider = collision.collider;
            grappleScript.Collided(collider, contact);
        }
    }

}
