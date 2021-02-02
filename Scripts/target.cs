using UnityEngine;

public class target : MonoBehaviour
{
    public float health = 100f;
    public string deathMethod;

    public Transform head;
    public Transform spine;
    public Transform rightArm;
    public Transform rightForeArm;
    public Transform rightHand;
    public Transform leftArm;
    public Transform leftForeArm;
    public Transform leftHand;
    public Transform torso;
    [HideInInspector] public Rigidbody torsoRB;
    public Transform rightThigh;
    public Transform rightAnkle;
    public Transform rightFoot;
    public Transform leftThigh; // hot
    public Transform leftAnkle;
    public Transform leftFoot;

    public void Start()
    {
        if(deathMethod == null)
        {
            Debug.Log("no death method selected for" + this.tag + "defaulting to destroy"); // leave the this, im pretty sure its important
            deathMethod = "destroy";
        }
    }

    public void Awake()
    {
        // uhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhhh
        if(torso != null && deathMethod == "die ragdoll")
            torsoRB = torso.GetComponent<Rigidbody>();
        // yeah
    }

    public void takeDamage(float amount)
    {
        health -= amount;
        if(health <= 0.0f)
        {
            if (deathMethod == "destroy")
            {
                dieDestroy();
            }
        }
    }

    void dieDestroy()
    {
        Destroy(gameObject);
    }

}
