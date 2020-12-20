using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string projectileType;
    public Grapple grappleScript;
    public Rigidbody projectileRb;
    
    //GunController gunScript;

    private void Awake()
    {
        if (projectileType != "grappling hook")
            Debug.LogError("Invalid projectile type on" + gameObject.name + " @Projectile.cs L15");
        if (projectileType == "grappling hook" && grappleScript == null)
            Debug.LogError("Grapple Script component unnasigned! " + gameObject.name + " @projectile.cs L17");
        projectileRb = GetComponent<Rigidbody>();
        if (projectileRb == null)
            Debug.LogError("Projectile rigidbody not found! " + gameObject.name + " @projectile.cs L19");
        if (projectileRb != null)
            Physics.IgnoreLayerCollision(9,9); // ignore player
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("fired collision");
        if (projectileType == "grappling hook")
        {
            ContactPoint contact = collision.contacts[0]; // congrats the first array in the entirety of caliber sprint and the first one in any of my games
            Collider collider = collision.collider;
            grappleScript.Collided(collider, contact);
        }
    }

}
